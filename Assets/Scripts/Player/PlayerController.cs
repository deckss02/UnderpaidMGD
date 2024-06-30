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
            Move(Input.GetAxis("Horizontal"));

            // Jump if grounded and jump button is pressed
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }

            // Fire if fire button is pressed and fire rate allows
            if (Input.GetKeyDown(KeyCode.L) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Fire();
            }
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

        // Update animator parameters
        myAnim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        myAnim.SetBool("Ground", isGrounded);
    }

    float moveDirection = 0;

    // Method for player movement
    public void Move(float dir)
    {
        if (dir > 0)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            facingRight = true;
        }
        else if (dir < 0)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            facingRight = false;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        moveDirection = dir;
    }

    // Method for player jump
    public void Jump()
    {
        rb.velocity += new Vector2(0, jumpSpeed); // Corrected Vector2
        jumpSound.Play();
    }

    // Method for firing bullets
    void Fire()
    {
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
