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

    // Timer for down movement
    private float downTimer = 0f;
    private const float downDuration = 0.5f;

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
                Move(moveDirection);
            }

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

            // Check for touch input
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    if (touchPosition.x > transform.position.x)
                    {
                        moveDirection = 1;
                    }
                    else
                    {
                        moveDirection = -1;
                    }
                    isMoving = true;
                }
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

        // Update down movement timer
        if (downTimer > 0)
        {
            downTimer -= Time.deltaTime;
            if (downTimer <= 0)
            {
                groundCheckRadius = 0.2f; // Restore ground detection radius
            }
        }

        // Update animator parameters
        myAnim.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); // Corrected Mathf.Abs
        myAnim.SetBool("Ground", isGrounded);
    }

    // Method for player movement
    public void Move(float dir)
    {
        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

        if (dir > 0 && !facingRight)
        {
            Flip();
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (dir < 0 && facingRight)
        {
            Flip();
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

        if (rb.velocity.x == 0)
        {
            isMoving = false; // Stop moving if velocity is zero
        }
    }

    // Method for player jump
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed); // Corrected Vector2
        jumpSound.Play();
    }

    // Method for player jump down
    public void Down()
    {
        groundCheckRadius = 0; // Temporarily disable ground detection
        rb.velocity = new Vector2(rb.velocity.x, -jumpSpeed); // Jump down by setting negative velocity
        downTimer = downDuration; // Start down movement timer
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