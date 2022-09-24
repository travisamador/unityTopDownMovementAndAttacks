using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //create rigidbody and animator to be set to the ones on the sprite in the inspector
    public Animator animator;
    public Rigidbody2D body;
    public Transform enemyPosition;
    string direction;
    string lastDirection;

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
    }

    void SetAnimations()
    {
        //set idle going to the right side
        if (enemyPosition.position.x > startPosition.x && startPosition.x - enemyPosition.position.x > startPosition.y - enemyPosition.position.y)
        {
            animator.SetFloat("IdleFace", 1f);
            animator.SetFloat("Vertical", 0f);
            animator.SetFloat("Horizontal", 1f);
            animator.SetFloat("Speed", 1);
        }
        else if (enemyPosition.position.x < startPosition.x && startPosition.x - enemyPosition.position.x > startPosition.y - enemyPosition.position.y)
        {
            //set idle going to the left side
            animator.SetFloat("IdleFace", 0.66f);
            animator.SetFloat("Horizontal", -1f);
            animator.SetFloat("Vertical", 0f);
            animator.SetFloat("Speed", 1);
        }
        else if (enemyPosition.position.y > startPosition.y)
        {
            //set idle going up
            animator.SetFloat("IdleFace", 0.33f);
            animator.SetFloat("Vertical", 1f);
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Speed", 1);
        }
        else if (enemyPosition.position.y < startPosition.y) 
        {
            //set idle going down
            animator.SetFloat("IdleFace", 0f);
            animator.SetFloat("Vertical", -1f);
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Speed", 1);
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
        body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
