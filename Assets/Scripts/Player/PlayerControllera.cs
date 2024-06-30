using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllera : MonoBehaviour
{
    // Variables for checking if the player is grounded
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

    // Reference to the level manager
    public LevelManager theLevelManager;

    // Knockback variables
    public float knockbackForce;
    public float knockbackLength;
    private float knockbackCounter;

    // Audio sources for jump and hurt sounds
    public AudioSource jumpSound;
    public AudioSource hurtSound;

    // Movement control variable
    public bool canMove = true;

    // Shooting variables
    public GameObject bulletToRight;
    public GameObject bulletToLeft;
    private Vector2 bulletPos;
    public float fireRate;
    private float nextFire;

    // Variable to check which direction the player is facing
    private bool facingRight = true;
    private GameObject Enemy;

    void Start()
    {
        // Initialize Rigidbody2D and Animator components
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        // Set the initial respawn position to the player's starting position
        respawnPosition = transform.position;

        // Find the LevelManager in the scene
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, realGround);

        // If not in knockback and can move
        if (knockbackCounter <= 0 && canMove)
        {
            // Move right
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                facingRight = true;
            }
            // Move left
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                facingRight = false;
            }
            // Idle
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            // Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpSound.Play();
            }

            // Fire a bullet
            if (Input.GetKeyDown(KeyCode.L) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Fire();
            }
        }

        // If in knockback
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;

            // Apply knockback force
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

    // Method to fire a bullet
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

    // Method to start knockback
    public void Knockback()
    {
        knockbackCounter = knockbackLength;
    }
}
