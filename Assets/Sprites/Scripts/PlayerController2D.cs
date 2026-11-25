using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public int facingDirection = 1; // 1 for right, -1 for left
    public string attackTrigger = "Attack";

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
        if (value.isPressed && anim != null)
        {
            anim.SetTrigger(attackTrigger);
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


