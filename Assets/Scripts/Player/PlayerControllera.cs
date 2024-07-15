using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllera : MonoBehaviour
{
    public Transform groundCheck;  //Declare variable to store position of groundCheck Object
    public float groundCheckRadius; //Declare variable to store the radius of a circle to be created
    public LayerMask realGround;   //Declare variable to identify which layer in unity is enabled
    public bool isGrounded;        //Boolean to determine whether player touch ground

    public float moveSpeed; //Controll the speed that player is moving around the world
    private Rigidbody2D rb;
    public float jumpSpeed; //Controll the speed that player is moving when jumping
    private Animator myAnim;
    public Vector3 respawnPosition;

    public LevelManager theLevelManager; //Make a reference to LvlManager

    public float knockbackForce;
    public float knockbackLength; //Amt of timr the player being knocked back
    private float knockbackCounter; //Count down if time for player being knocked back
    public AudioSource jumpSound;
    public AudioSource hurtSound;
    public bool canMove = true; // When game is paused, player cannot move

    public GameObject bulletToRight;
    public GameObject bulletToLeft; //Game Object will be instantiated when hit the fire button
    private Vector2 bulletPos; //Coordinates where the bullet should be instantiated
    public float fireRate;
    private float nextFire;
    public bool isMoving;

    private bool facingRight = true;
    private GameObject Enemy;

    // Jump cooldown variables
    public float jumpCooldown = 1.2f; // Adjust this value to set the cooldown duration
    private float nextJumpTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Get and store a reference to the Rigidbody2D component so that we can access it

        myAnim = GetComponent<Animator>(); //Get and store a reference to the Animator component so that we can access it
        respawnPosition = transform.position; //When game starts, respawn position equals to the current players position
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, realGround);
        if (knockbackCounter <= 0 && canMove) //If there is no knockback
        {
            // Jump input handling with cooldown check
            if (Input.GetButtonDown("Jump") && Time.time >= nextJumpTime)
            {
                Jump();
                jumpSound.Play();
                nextJumpTime = Time.time + jumpCooldown; // Set the next allowed jump time
            }

            // Firing logic
            if (Input.GetKeyDown(KeyCode.L) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Fire();
            }
        }

        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;   //Time Count down

            if (transform.localScale.x > 0)
            {
                rb.velocity = new Vector3(-knockbackForce, knockbackForce, 0.0f); //The force to push the player back
            }
            else
            {
                rb.velocity = new Vector3(knockbackForce, knockbackForce, 0.0f); //The force to push the player back
            }
        }

        myAnim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        myAnim.SetBool("Ground", isGrounded);
    }

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
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); //Move to the left
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
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
        if (!isGrounded || Time.time < nextJumpTime)
            return;

        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        nextJumpTime = Time.time + jumpCooldown; // Update the cooldown timer
        myAnim.SetBool("Ground", false);
    }

    public void Fire()
    {
        bulletPos = transform.position;

        // Adjust bullet position based on facing direction
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

    void Flip()
    {
        // Flip the player's facing direction
        facingRight = !facingRight;

        // Flip the player object
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "WeakPoint")
        {
            var bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(20f);
            }
        }
    }

    public void Knockback()
    {
        knockbackCounter = knockbackLength;
    }
}
