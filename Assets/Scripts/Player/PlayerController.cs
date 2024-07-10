using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables for ground check
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask realGround;
    public bool isGrounded;

    // Movement variables
    public float moveSpeed;
    private Rigidbody2D rb;
    public float jumpSpeed;
    private Animator myAnim;
    public Vector3 respawnPosition;

    // Level manager reference
    public LevelManager theLevelManager;

    // Knockback variables
    public float knockbackForce;
    public float knockbackLength;
    private float knockbackCounter;
    public AudioSource jumpSound;
    public AudioSource hurtSound;
    public bool canMove = true;

    // Shooting variables
    public GameObject bulletToRight;
    public GameObject bulletToLeft;
    private Vector2 bulletPos;
    public float fireRate;
    private float nextFire;

    private bool facingRight = true;
    private bool isMoving = false;
    private float moveDirection = 0;

    void Start()
    {
        // Initialize components and variables
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        respawnPosition = transform.position;
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, realGround);

        if (knockbackCounter <= 0)
        {
            // Stop receiving input if timeScale is 0 or player cannot move
            if (!canMove) return;

            // Code for left and right movement
            if (isMoving)
            {
                //Move(moveDirection);
            }

            // Update animator parameters
            myAnim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            myAnim.SetBool("Ground", isGrounded);
        }
        else
        {
            // Apply knockback effect
            knockbackCounter -= Time.deltaTime;
            if (transform.localScale.x > 0)
            {
                rb.velocity = new Vector3(-knockbackForce, knockbackForce, 0.0f);
            }
            else
            {
                rb.velocity = new Vector3(knockbackForce, knockbackForce, 0.0f);
            }
        }
    }

    // Method for player movement
    public void Move(float dir)
    {
        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);
        print(rb.velocity);

        if (dir > 0 && !facingRight)
        {
            Flip();
        }
        else if (dir < 0 && facingRight)
        {
            Flip();
        }

        if (rb.velocity.x == 0)
        {
            isMoving = false; // Stop moving if velocity is zero
        }
    }

    //// Start moving in the specified direction
    //public void StartMove(float dir)
    //{
    //    moveDirection = dir;
    //    isMoving = true;
    //}

    //// Stop moving
    //public void StopMove()
    //{
    //    moveDirection = 0;
    //    isMoving = false;
    //}

    // Method for player jump
    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            jumpSound.Play();
        }
    }

    // Method for firing bullets
    public void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            bulletPos = transform.position;
            if (facingRight)
            {
                bulletPos += new Vector2(+1f, -0.43f);
                Instantiate(bulletToRight, bulletPos, Quaternion.identity);
            }
            else
            {
                bulletPos += new Vector2(-1f, -0.43f);
                Instantiate(bulletToLeft, bulletPos, Quaternion.identity);
            }
        }
    }

    // Method to flip the player direction
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Handle collision with objects tagged as "WeakPoint"
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "WeakPoint")
        {
            var bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(20f);
                Debug.Log("20 Damage Taken");
            }
        }
    }

    // Method to apply knockback effect
    public void Knockback()
    {
        knockbackCounter = knockbackLength;
    }
}
