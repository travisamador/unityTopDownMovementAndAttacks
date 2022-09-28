using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class Enemy : MonoBehaviour
{
    //create rigidbody and animator to be set to the ones on the sprite in the inspector
    public Animator animator;
    public Rigidbody2D body;
    public Transform enemyPosition;
    string direction;
    string lastDirection;

    public ChefMovement2 player;

    public Transform enemyAttackBox;
    //set attack range
    public float enemyAttackRange = .05f;
    //set how much damage attack does
    public int enemyAttackDamage = 10;
    //create layermask to detect enemies hit
    public LayerMask playerLayer;

    //change colliders based on direction because side view is smaller
    public Collider2D frontCollider;
    public Collider2D sideCollider;

    //used to set attackrate smaller attack rate = slower consecutive attacks, set to .5 to eliminate some ghost attacks/double damage per attack
    public float attackRate = .5f;
    float nextAttackTime = 0;

    public AIPath path;

    Vector2 startPosition;
    Vector2 movement;

    public int maxHealth = 100;
    int currentHealth;

    private void Awake()
    {
        enemyPosition = GetComponent<Transform>();
        startPosition = enemyPosition.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        setDirection();
        if(lastDirection != direction)
        {
            SetAnimations();
        }
        lastDirection = direction;
        startPosition = enemyPosition.position;
        if (path.reachedDestination && Time.time >= nextAttackTime && !player.isDead)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    //sets parameters for idle, movement, and attack animations, moves and resizes attack box based on direction, changes collider for front/back or left/right 
    void SetAnimations()
    {
        //set idle going to the right side
        if (enemyPosition.position.x > startPosition.x && Math.Abs(startPosition.x - enemyPosition.position.x) > Math.Abs(startPosition.y - enemyPosition.position.y))
        {
            animator.SetFloat("IdleFace", 1f);
            animator.SetFloat("Vertical", 0f);
            animator.SetFloat("Horizontal", 1f);
            animator.SetFloat("Speed", 1);
            frontCollider.enabled = false;
            sideCollider.enabled = true;
            enemyAttackBox.localPosition = new Vector3(0.144f, -0.035f, 0f);
            enemyAttackRange = .62f;
        }
        else if (enemyPosition.position.x < startPosition.x && Math.Abs(startPosition.x - enemyPosition.position.x) > Math.Abs(startPosition.y - enemyPosition.position.y))
        {
            //set idle going to the left side
            animator.SetFloat("IdleFace", 0.66f);
            animator.SetFloat("Horizontal", -1f);
            animator.SetFloat("Vertical", 0f);
            animator.SetFloat("Speed", 1);
            frontCollider.enabled = false;
            sideCollider.enabled = true;
            enemyAttackBox.localPosition = new Vector3(-0.144f, -0.035f, 0f);
            enemyAttackRange = .62f;
        }
        else if (enemyPosition.position.y > startPosition.y && Math.Abs(startPosition.y - enemyPosition.position.y) > Math.Abs(startPosition.x - enemyPosition.position.x))
        {
            //set idle going up
            animator.SetFloat("IdleFace", 0.33f);
            animator.SetFloat("Vertical", 1f);
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Speed", 1);
            frontCollider.enabled = true;
            sideCollider.enabled = false;
            enemyAttackBox.localPosition = new Vector3(0.007f, 0.121f, 0f);
            enemyAttackRange = 0.5f;
        }
        else if (enemyPosition.position.y < startPosition.y) 
        {
            //set idle going down
            animator.SetFloat("IdleFace", 0f);
            animator.SetFloat("Vertical", -1f);
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Speed", 1);
            frontCollider.enabled = true;
            sideCollider.enabled = false;
            enemyAttackBox.localPosition = new Vector3(0.013f, -0.119f, 0f);
            enemyAttackRange = 0.62f;
        }
        else if (path.reachedEndOfPath)
        {
            animator.SetFloat("Vertical", 0f);
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Speed", 0);
        }
    }

    public void setDirection()
    {
        //set idle going to the right side
        if (enemyPosition.position.x > startPosition.x)
        {
            direction = "Right";
        }
        else if (enemyPosition.position.x < startPosition.x)
        {
            direction = "Left";
        }
        else if (enemyPosition.position.y > startPosition.y)
        {
            direction = "Up";
        }
        else if (enemyPosition.position.y < startPosition.y)
        {
            direction = "Down";
        }
        else
        {
            direction = "Idle";
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //play hurt animation
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        //Die Animation
        animator.SetBool("IsDead", true);
        path.canMove = false;
        //body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        //GetComponent<Collider2D>().enabled = false;
        frontCollider.enabled = false;
        sideCollider.enabled = false;
        this.enabled = false;
    }

    void Attack()
    {
        //matches idle float for correct direction attack animation
        animator.SetFloat("Attack", animator.GetFloat("IdleFace"));
        animator.Play("Attack");
        //get layer mask containing player(s) hit
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(enemyAttackBox.position, enemyAttackRange, playerLayer);
        //Damage player
        foreach (Collider2D player in hitPlayer)
        {
            Debug.Log("Enemy hit " + player.name);
            player.GetComponent<ChefMovement2>().TakeDamage(enemyAttackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (enemyAttackBox == null)
        {
            return;
        }
        //shows circle around attack box
        Gizmos.DrawWireSphere(enemyAttackBox.position, enemyAttackRange);
    }
}
