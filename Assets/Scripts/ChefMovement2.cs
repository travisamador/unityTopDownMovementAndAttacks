using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//monobehaviour - unity inherited class for working between unity and visual studio
public class ChefMovement2 : MonoBehaviour
{
    //sets speed/force for movement
    public float moveSpeed = 5f;
    //create rigidbody and animator to be set to the ones on the sprite in the inspector
    public Rigidbody2D rb;
    public Animator animator;
    //Vector to track movement
    Vector2 movement;

    //add transform for attackBox position
    public Transform AttackBox;
    //set attack range
    public float attackRange = .05f;
    //set how much damage attack does
    public int attackDamage = 25;
    //create layermask to detect enemies hit
    public LayerMask enemyLayers;

    public int maxHealth = 100;
    int currentHealth;
    public bool isDead = false;

    //awake is called first when game object is instantiated
    private void Awake()
    {
        //set rigidbody and animator to match the ones on the sprite in unity
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //get movement from axis
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //normalize movement to prevent speed increase when moving diagonal
        movement = movement.normalized;

        if (movement.x != 0)
        {
            animator.SetFloat("Vertical", 0);
            animator.SetFloat("Horizontal", movement.x);
        }
        if (movement.y != 0)
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", movement.y);
        }
        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        //updates sprite position
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Attack()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            animator.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackBox.position, attackRange, enemyLayers);
            //Damage them
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit " + enemy.name);
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        //}
    }

    void OnDrawGizmosSelected()
    {
        if (AttackBox == null)
        {
            return;
        }
        //draw circle around attack box when player is selected in hierarchy
        Gizmos.DrawWireSphere(AttackBox.position, attackRange); 
    }

    public void TakeDamage(int damage)
    {
        //subtract enemy damage from health
        currentHealth -= damage;
        //player dies if health hits 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        isDead = true;
        //stop collisions
        GetComponent<Collider2D>().enabled = false;
        //remove sprite from screen
        GetComponent<SpriteRenderer>().enabled = false;
        //disable this script
        this.enabled = false;
    }
}