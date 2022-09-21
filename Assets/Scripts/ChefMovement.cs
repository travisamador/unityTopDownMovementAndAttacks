using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//monobehaviour - unity inherited class for working between unity and visual studio
public class ChefMovement : MonoBehaviour
{
    //sets speed/force for movement
    public float moveSpeed = 5f;
    //create rigidbody and animator to be set to the ones on the sprite in the inspector
    public Rigidbody2D rb;
    public Animator animator;

    //delay for attack
    public float delay = 0.3f;
    public bool attackBlocked;

    //Vector to track movement
    Vector2 movement;

    //awake is called first when game object is instantiated
    private void Awake()
    {
        //set rigidbody and animator to match the ones on the sprite in unity
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        Attack();
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
        }
        else if (movement.x < 0)
        {
            //set idle going to the left side
            animator.SetFloat("IdleFace", 0.66f);
        }
        else if (movement.y > 0)
        {
            //set idle going up
            animator.SetFloat("IdleFace", 0.33f);
        }
        else if (movement.y < 0)
        {
            //set idle going down
            animator.SetFloat("IdleFace", 0f);
        }
    }

    void Attack()
    {
        //matches idle float for correct direction attack animation
        animator.SetFloat("Attack", animator.GetFloat("IdleFace"));
        if (Input.GetButtonDown("Jump") && !animator.GetBool("IsAttacking"))
        {
            //set attack boolean to true
            animator.SetBool("IsAttacking", true);
            animator.Play("Attack");
            
        }
        //makes sure attack boolean gets set back to false after a delay
        Invoke("ResetAttack", delay);
    }

    //method to reset attack boolean
    void ResetAttack()
    {
        animator.SetBool("IsAttacking", false);
    }
}
