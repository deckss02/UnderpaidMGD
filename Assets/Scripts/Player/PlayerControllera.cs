using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllera : MonoBehaviour
{
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask realGround;
    public bool isGrounded;

    public float moveSpeed;
    private Rigidbody2D rb;
    public float jumpSpeed;
    private Animator myAnim;
    public Vector3 respawnPosition;

    public LevelManager theLevelManager;

    public float knockbackForce;
    public float knockbackLength;
    private float knockbackCounter;
    public AudioSource jumpSound;
    public AudioSource hurtSound;
    public bool canMove = true;

    public GameObject bulletToRight;
    public GameObject bulletToLeft;
    private Vector2 bulletPos;
    public float fireRate;
    private float nextFire;

    private bool facingRight = true;
    private GameObject Enemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        respawnPosition = transform.position;
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, realGround);

        if (knockbackCounter <= 0 && canMove)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                facingRight = true;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                facingRight = false;
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
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

        myAnim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        myAnim.SetBool("Ground", isGrounded);
    }

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

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Knockback()
    {
        knockbackCounter = knockbackLength;
    }
}
