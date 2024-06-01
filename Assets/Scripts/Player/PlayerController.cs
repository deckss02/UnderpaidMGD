using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
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

    private bool facingRight = true;
    private GameObject Enemy;

    private Vector2 startTouchPosition, endTouchPosition;
    private Touch touch;
    private bool CoroutineAllowed;

  
    // Start is called before the first frame update
    void Start()
    {
        //Ground detection variables

        rb = GetComponent<Rigidbody2D>(); //Get and store a reference to the Rigidbody2D component so that we can access it

        myAnim = GetComponent<Animator>(); //Get and store a reference to the Animator component so that we can access it
        respawnPosition = transform.position; //When game starts, respawn position equals to the current players position
        theLevelManager = FindObjectOfType<LevelManager>();
        CoroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
        }

        if (touch.phase == TouchPhase.Began)
        {
            startTouchPosition = touch.position;
        }

        if (Input.touchCount > 0 && touch.phase == TouchPhase.Ended && CoroutineAllowed)
        {
            endTouchPosition = touch.position;

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, realGround);

            if (knockbackCounter <= 0 && canMove) //If there is no knockback
            {
                bool swipingLeft = ((endTouchPosition.x < startTouchPosition.x) && (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y)));
                bool swipingRight = ((endTouchPosition.x > startTouchPosition.x) && (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y)));
                bool swipingUp = ((endTouchPosition.y > startTouchPosition.y) && (Mathf.Abs(touch.deltaPosition.y) > Mathf.Abs(touch.deltaPosition.x)));
                bool swipingDown = ((endTouchPosition.x < startTouchPosition.x) && (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y)));

                if ((Input.GetAxisRaw("Horizontal") > 0) || swipingRight)
                {
                    rb.velocity = new Vector2(moveSpeed, rb.velocity.y); //Move to the right 
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    facingRight = true;
                }

                else if ((Input.GetAxisRaw("Horizontal") < 0) || swipingLeft)

                {
                    rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); //Move to the left
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    facingRight = false;
                }

                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);  //if input is staying at 0, player should be standing still
                }


                if ((Input.GetButtonDown("Jump")) || swipingUp)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed); //Move Up
                    jumpSound.Play();
                }

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

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
        }
    }

        void Fire()
        {
            bulletPos = transform.position; //Set the position of the bullet to be player position
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
            if (other.tag == "KillPlane")
            {
                //GameObject.SetActive(false);  //Set the PLayer to inactive
                //transform.position = respawnPosition; //set the position to respawnPosition when it dies
                theLevelManager.healthCount -= 100;
                theLevelManager.UpdateHeartMeter();
                theLevelManager.Respawn();
            }
        }

        public void Knockback()
        {
            knockbackCounter = knockbackLength;
        }


    }