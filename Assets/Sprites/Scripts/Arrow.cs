using UnityEngine;

public class Arrow : MonoBehaviour
{

    public Rigidbody2D rb;
    public Vector2 direction = Vector2.right;
    public float lifespawn = 200;
    public float speed;
    public int damage = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = direction * speed;
        RotateArrow();
        Destroy(gameObject, lifespawn);
    }

    private void RotateArrow()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy_Movement enemy = collision.gameObject.GetComponentInParent<Enemy_Movement>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
