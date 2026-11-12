using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    private int facingDirection = 2; // 2 for right, -2 for left

    private Rigidbody2D rb;
    private Transform Player;
    private Animator anim;
    private EnemyState enemyState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);

    }

    void Update()
    {
        if (enemyState == EnemyState.Chasing)
        {
            if (Player.position.x > transform.position.x && facingDirection == 2 ||
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
            if (Player == null)
            {
                Player = collision.transform;
            }

            ChangeState(EnemyState.Chasing);
        }
    }

    //if player leaves Enemy detection range
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }


    void ChangeState(EnemyState newState)
    {
        //Exit current Animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);

        //Update animation state
        enemyState = newState;

        // Update new animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
}