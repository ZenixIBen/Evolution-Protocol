using UnityEngine;

public class Player_bow : MonoBehaviour
{
    public Transform LaunchPoint;
    public GameObject ArrowPrefab;
    public Vector2 aimDirection = Vector2.right;

    public float shootCooldown = .5f;
    public float shootTimer;
    public int damage = 2;

    // Update is called once per frame
    void Update()
    {

        shootTimer -= Time.deltaTime;


        handleAiming();

        if (Input.GetButtonDown("Shoot") && shootTimer <= 0 )
        {
            Shoot();
        }
    }

    private void handleAiming()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            aimDirection = new Vector2(horizontal, vertical).normalized;
        }
    }
    public void Shoot()
    {
        Arrow arrow = Instantiate(ArrowPrefab, LaunchPoint.position, Quaternion.identity).GetComponent<Arrow>();
        arrow.direction = aimDirection;
        shootTimer = shootCooldown;
    }

        private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        }
    }

}
