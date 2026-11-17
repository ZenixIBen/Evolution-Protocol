using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    public float attackRange = 2;
    public float attackCooldown = 2;
    public float playerDetectRange = 5;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    private float attackCooldownTimer;
    private int facingDirection = -2; // 2 for right, -2 for left
    private EnemyState enemyState;


    private Rigidbody2D rb;
    private Transform Player;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);

    }

    void Update()
    {
        CheckForPlayer();

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (enemyState == EnemyState.Chasing)
        {
            Chase();
        }
        else if (enemyState == EnemyState.Attacking)
        {
            //attacky stuff
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Chase()
    {

        if ((Player.position.x > transform.position.x && facingDirection == -2) ||
            (Player.position.x < transform.position.x && facingDirection == 2))
        {
            Flip();
        }

        Vector2 direction = (Player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    //if player enters Enemy detection range
    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

        if (hits.Length > 0)
        {
            Player = hits[0].transform;

            //checking if player is in range AND cooldown is ready
            if (Vector2.Distance(transform.position, Player.position) < attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
                GetComponent<Enemy_Combat>().Attack();
            }
            if (Vector2.Distance(transform.position, Player.position) > attackRange)
            {
                ChangeState(EnemyState.Chasing);
            }
        }

        else
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
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", false);

        //Update animation state
        enemyState = newState;

        // Update new animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
}