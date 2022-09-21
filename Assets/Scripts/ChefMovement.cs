using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    //delay for attack
    public float delay = 0.3f;
    public bool attackBlocked;

    Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        IdleFace();
        Attack();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void IdleFace()
    {
        //going to the right side
        if (movement.x > 0)
        {
            animator.SetFloat("IdleFace", 1f);
        }
        else if (movement.x < 0)
        {
            animator.SetFloat("IdleFace", 0.66f);
        }
        else if (movement.y > 0)
        {
            animator.SetFloat("IdleFace", 0.33f);
        }
        else if (movement.y < 0)
        {
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
        //else
        //{
        //    animator.SetBool("IsAttacking", false);
        //}
    }

    //method to reset attack boolean
    void ResetAttack()
    {
        animator.SetBool("IsAttacking", false);
    }

    //public void Attack()
    //{
    //    if (attackBlocked)
    //        return;
    //    animator.SetTrigger("Attacking");
    //    attackBlocked = true;
    //    StartCoroutine(DelayAttack());
    //}

    //private IEnumerator DelayAttack()
    //{
    //    yield return new WaitForSeconds(delay);
    //    attackBlocked = false;
    //}
}
