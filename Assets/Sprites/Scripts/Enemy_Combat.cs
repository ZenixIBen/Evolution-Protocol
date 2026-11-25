using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange;
    public float knockbackForce = 5f;
    public float stunTime;
    public LayerMask playerlayer;

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.tag == "Player")
    //     {
    //         collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
    //     }
    // }

    public void Attack()
    {
        if (attackPoint == null)
        {
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerlayer);
        if (hits.Length > 0)
        {
            PlayerHealth playerHealth = hits[0].GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-damage);
            }

            PlayerMovement playerMovement = hits[0].GetComponentInParent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Knockback(transform, knockbackForce, stunTime);
            }
        }
    }
}
