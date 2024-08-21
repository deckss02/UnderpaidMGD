using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllera : MonoBehaviour
{
    public Transform groundCheck;  // Declare variable to store position of groundCheck Object
    public float groundCheckRadius; // Declare variable to store the radius of a circle to be created
    public LayerMask realGround;   // Declare variable to identify which layer in unity is enabled
    public bool isGrounded;        // Boolean to determine whether player touches ground

    public float moveSpeed; // Control the speed that player is moving around the world
    private Rigidbody2D rb;
    public float jumpSpeed; // Control the speed that player is moving when jumping
    private Animator myAnim;

    private Collider2D playerCollider;  // Reference to the player's collider
    public LayerMask enemyLayer;        // LayerMask for identifying enemies


    public LevelManager theLevelManager; // Make a reference to LevelManager

    public float knockbackForce;
    public float knockbackLength; // Amount of time the player is being knocked back
    private float knockbackCounter; // Count down if time for player being knocked back
    public AudioSource jumpSound;
    public AudioSource hurtSound;
    public AudioSource SwordSound;
    public bool canMove = true; // When game is paused, player cannot move

    private bool facingRight = true;
    private GameObject Enemy;

    // Jump cooldown variables
    public float jumpCooldown = 1.0f; // Adjust this value to set the cooldown duration
    private float nextJumpTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get and store a reference to the Rigidbody2D component so that we can access it
        myAnim = GetComponent<Animator>(); // Get and store a reference to the Animator component so that we can access it
        theLevelManager = FindObjectOfType<LevelManager>();
        playerCollider = GetComponent<Collider2D>();  // Get the player's collider
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, realGround);

        if (knockbackCounter <= 0 && canMove) // If there is no knockback and the player can move
        {
            // Jump input handling with cooldown check
            if (Input.GetButtonDown("Jump") && Time.time >= nextJumpTime)
            {
                Jump();
                jumpSound.Play();
                nextJumpTime = Time.time + jumpCooldown; // Set the next allowed jump time
            }
        }

        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;   // Time Count down

            if (transform.localScale.x > 0)
            {
                rb.velocity = new Vector3(-knockbackForce, knockbackForce, 0.0f); // The force to push the player back
            }
            else
            {
                rb.velocity = new Vector3(knockbackForce, knockbackForce, 0.0f); // The force to push the player back
            }
        }

        myAnim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        myAnim.SetBool("Ground", isGrounded);

        // Ignore collisions between the player and all enemies on the enemy layer
        Collider2D[] enemyColliders = FindObjectsOfType<Collider2D>();
        foreach (Collider2D enemyCollider in enemyColliders)
        {
            if (((1 << enemyCollider.gameObject.layer) & enemyLayer) != 0)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCollider);
            }
        }
    }

    public void Move(float dir)
    {
        if (!canMove) return; // Prevent movement if the player is frozen

        if (dir > 0)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            SwordSound.Play();
            facingRight = true;
        }
        else if (dir < 0)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); // Move to the left
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            SwordSound.Play();
            facingRight = false;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void Jump()
    {
        // Cannot jump if not on the ground or if the cooldown hasn't expired.
        if (!isGrounded || Time.time < nextJumpTime || !canMove)
            return;

        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        nextJumpTime = Time.time + jumpCooldown; // Update the cooldown timer
        myAnim.SetBool("Ground", false);
    }

    void Flip()
    {
        // Flip the player's facing direction
        facingRight = !facingRight;

        // Flip the player object
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Knockback()
    {
        knockbackCounter = knockbackLength;
    }

    public bool IsMoving
    {
        get { return rb.velocity.x != 0; }  // Check if the player has horizontal velocity
    }

    // Method to freeze or unfreeze the player
    public void FreezePlayer(bool freeze)
    {
        canMove = !freeze;

        if (freeze)
        {
            StartCoroutine(theLevelManager.Invulnerability());
        }
    }
}
