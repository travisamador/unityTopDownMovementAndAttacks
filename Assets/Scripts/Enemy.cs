using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    //create rigidbody and animator to be set to the ones on the sprite in the inspector
    public Animator animator;
    public Rigidbody2D body;

    //for tracking and updating enemy position
    public Transform enemyPosition;
    Vector3 startPosition;
    public AIPath path;

    public ChefMovement2 player;

    //attack box to track if player is hit
    public Transform enemyAttackBox;
    //set attack range
    public float enemyAttackRange = .05f;
    //set how much damage attack does
    public int enemyAttackDamage = 10;
    //create layermask to detect enemies hit
    public LayerMask playerLayer;
    //distance away from player to start attacking, set to a little longer than the path's end reached distance
    public float attackDistance;

    //change colliders based on direction because side view is smaller
    public Collider2D frontCollider;
    public Collider2D sideCollider;

    //used to set attackrate smaller attack rate = slower consecutive attacks
    public float attackRate = .5f;
    float nextAttackTime = 0;

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
        SetAnimations();
        startPosition = enemyPosition.position;
        if (path.remainingDistance < attackDistance && Time.time >= nextAttackTime && !player.isDead)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
        else if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    //sets parameters for idle, movement, and attack animations 
    void SetAnimations()
    {
        if(startPosition != enemyPosition.position)
        {
            animator.SetBool("IsWalking", true);
        }
        //going to the right side
        if (enemyPosition.position.x > startPosition.x && Mathf.Abs(startPosition.x - enemyPosition.position.x) > Mathf.Abs(startPosition.y - enemyPosition.position.y))
        {
            animator.SetFloat("Vertical", 0f);
            animator.SetFloat("Horizontal", 1f);
            frontCollider.enabled = false;
            sideCollider.enabled = true;
        }
        else if (enemyPosition.position.x < startPosition.x && Mathf.Abs(startPosition.x - enemyPosition.position.x) > Mathf.Abs(startPosition.y - enemyPosition.position.y))
        {
            //going to the left side
            animator.SetFloat("Horizontal", -1f);
            animator.SetFloat("Vertical", 0f);
            frontCollider.enabled = false;
            sideCollider.enabled = true;
        }
        else if (enemyPosition.position.y > startPosition.y && Mathf.Abs(startPosition.y - enemyPosition.position.y) > Mathf.Abs(startPosition.x - enemyPosition.position.x))
        {
            //going up
            animator.SetFloat("Vertical", 1f);
            animator.SetFloat("Horizontal", 0f);
            frontCollider.enabled = true;
            sideCollider.enabled = false;
        }
        else if (enemyPosition.position.y < startPosition.y) 
        {
            //going down
            animator.SetFloat("Vertical", -1f);
            animator.SetFloat("Horizontal", 0f);
            frontCollider.enabled = true;
            sideCollider.enabled = false;
        }
        else if (path.reachedEndOfPath)
        {
            animator.SetBool("IsWalking", false);
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
        animator.SetTrigger("Attack");
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
