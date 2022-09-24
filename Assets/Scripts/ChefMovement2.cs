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
    public Transform playerPosition;

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

    //awake is called first when game object is instantiated
    private void Awake()
    {
        //set rigidbody and animator to match the ones on the sprite in unity
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerPosition = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //get movement from axis
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //normalize movement to prevent speed increase when moving diagonal
        movement = movement.normalized;

        //set movement and speed floats
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        //sqrmagnitude can be preferrable to magnitude
        animator.SetFloat("Speed", movement.sqrMagnitude);
        //sets direction for idle
        IdleFace();
        //sets attack direction and enables attack
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

    void IdleFace()
    {
        //set idle going to the right side
        if (movement.x > 0)
        {
            animator.SetFloat("IdleFace", 1f);
            AttackBox.localPosition = new Vector3(0.071f, -0.015f, 0f);
        }
        else if (movement.x < 0)
        {
            //set idle going to the left side
            animator.SetFloat("IdleFace", 0.66f);
            AttackBox.localPosition = new Vector3(-0.071f, 0.015f, 0f);
        }
        else if (movement.y > 0)
        {
            //set idle going up
            animator.SetFloat("IdleFace", 0.33f);
            AttackBox.localPosition = new Vector3(-0.023f, 0.04f, 0f);
        }
        else if (movement.y < 0)
        {
            //set idle going down
            animator.SetFloat("IdleFace", 0f);
            AttackBox.localPosition = new Vector3(0.032f, -0.047f, 0f);
        }
    }

    void Attack()
    {
        //matches idle float for correct direction attack animation
        animator.SetFloat("Attack", animator.GetFloat("IdleFace"));
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            animator.Play("Attack");
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
        Gizmos.DrawWireSphere(AttackBox.position, attackRange); 
    }
}