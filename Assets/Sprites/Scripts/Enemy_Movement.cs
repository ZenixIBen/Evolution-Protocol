using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    private bool isChasing;
    private int facingDirection = 2; // 2 for right, -2 for left

    private Rigidbody2D rb;
    private Transform Player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isChasing) 
        {
            if(Player.position.x > transform.position.x && facingDirection == 2||
                Player.position.x < transform.position.x && facingDirection == -2) 
            {
                Flip();
            }

            Vector2 direction = (Player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
    }

    void Flip() 
    { 
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    //if player enters Enemy detection range
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(Player == null)
            {
                Player = collision.transform;
            }
            
            isChasing = true;
        }
    }

    //if player leaves Enemy detection range
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.linearVelocity = Vector2.zero;
            isChasing = false;
        }
    }
}
