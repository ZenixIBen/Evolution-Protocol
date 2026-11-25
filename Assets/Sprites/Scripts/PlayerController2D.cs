using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public int facingDirection = 1; // 1 for right, -1 for left
    public string attackTrigger = "Attack";
    public float attackRange = 0.75f;
    public float attackOffset = 0.5f;

    public Rigidbody2D rb;
    public Animator anim;

    private Vector2 movement;
    private bool isKnockedBack;

    // Called by the new Input System
    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        if (anim != null)
        {
            anim.SetTrigger(attackTrigger);
        }

        if (StatsManager.Instance == null || StatsManager.Instance.damage <= 0)
        {
            return;
        }

        Vector2 attackPosition = (Vector2)transform.position
            + Vector2.right * facingDirection * attackOffset;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPosition, attackRange);
        foreach (Collider2D hit in hits)
        {
            Enemy_Movement enemy = hit.GetComponentInParent<Enemy_Movement>();
            if (enemy != null)
            {
                enemy.TakeDamage(StatsManager.Instance.damage);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (rb != null && isKnockedBack == false)
        {
            if (anim != null)
            {
                anim.SetFloat("horizontal", Mathf.Abs(movement.x));
                anim.SetFloat("vertical", Mathf.Abs(movement.y));
            }

            if (movement.x > 0 && transform.localScale.x < 0 ||
                (movement.x < 0 && transform.localScale.x > 0))
            {
                Flip();
            }

            if (StatsManager.Instance != null)
            {
                rb.linearVelocity = movement * StatsManager.Instance.speed;
            }
        }
    }


    void Flip()
    {
        facingDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = transform.position - enemy.position;
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * force;
            StartCoroutine(KnockbackCounter(stunTime));
        }

    }



    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }


}


