using UnityEngine;
using UnityEngine.InputSystem;

public class Player_bow : MonoBehaviour
{
    public Transform LaunchPoint;
    public GameObject ArrowPrefab;
    public Vector2 aimDirection = Vector2.right;

    public float shootCooldown = .5f;
    public float shootTimer;
    public int damage = 2;

    private void Update()
    {
        shootTimer -= Time.deltaTime;
    }

    public void OnMove(InputValue value)
    {
        Vector2 movement = value.Get<Vector2>();
        if (movement != Vector2.zero)
        {
            aimDirection = movement.normalized;
        }
    }

    public void OnShoot(InputValue value)
    {
        if (value.isPressed && shootTimer <= 0)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        Arrow arrow = Instantiate(ArrowPrefab, LaunchPoint.position, Quaternion.identity).GetComponent<Arrow>();
        arrow.direction = aimDirection;
        arrow.damage = damage;

        Collider2D[] playerColliders = GetComponentsInChildren<Collider2D>();
        Collider2D[] arrowColliders = arrow.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D playerCollider in playerColliders)
        {
            foreach (Collider2D arrowCollider in arrowColliders)
            {
                Physics2D.IgnoreCollision(playerCollider, arrowCollider);
            }
        }

        shootTimer = shootCooldown;
    }
}
