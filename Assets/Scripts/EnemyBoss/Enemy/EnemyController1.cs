using UnityEngine;

public class EnemyController1 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // Speed at which the enemy moves
    [SerializeField] private int damageAmount = 1; // Amount of damage to apply
    [SerializeField] private int maxHealth = 1; // Maximum health for the enemy
    private int currentHealth; // Current health of the enemy
    private Rigidbody2D enemyRigidbody; // Reference to the Rigidbody2D component
    public TimEC timEC;

    public Transform groundCheck;  //Declare variable to store position of groundCheck Object
    public float groundCheckRadius; //Declare variable to store the radius of a circle to be created
    public LayerMask realGround;   //Declare variable to identify which layer in unity is enabled
    public bool isGrounded;        //Boolean to determine whether player touch ground
    private Animator myAnim;

    private bool isKilled = false; // Flag to check if the enemy is already counted as killed
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        myAnim = GetComponent<Animator>(); //Get and store a reference to the Animator component so that we can access it
        currentHealth = maxHealth; // Initialize current health
        timEC = FindObjectOfType<TimEC>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, realGround);

        if (IsFacingRight())
        {
            enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            enemyRigidbody.velocity = new Vector2(-moveSpeed, 0f);
        }

       //if (enemyRigidbody.velocity.x < -0.01f)
       //{
       //    GetComponent<SpriteRenderer>().flipX = true;
       //
       //} else if (enemyRigidbody.velocity.x > 0.01f)
       //{
       //    GetComponent<SpriteRenderer>().flipX = false;
       //}
        myAnim.SetBool("Ground", isGrounded);
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider is a boundary
        if (collision.CompareTag("Boundary"))
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Check if the collider is tagged as a Bullet and apply damage
        if (collision.collider.CompareTag("Bullet"))
        {
            Debug.Log($"Bullet hit enemy. Applying {damageAmount} damage.");
            TakeDamage(damageAmount); // Apply damage to the enemy
            Destroy(collision.collider.gameObject); // Destroy the bullet after hitting the enemy
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // Reduce current health
        if (currentHealth <= 0)
        {
            Die(); // Destroy the enemy if health is 0 or less
        }
    }

    private void Flip()
    {
         transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        //enemyRigidbody.velocity = -enemyRigidbody.velocity;
    }

    public void Die()
    {
        if (!isKilled) // Check if the enemy is already killed
        {
            Debug.Log("Enemy is dying");
            isKilled = true; // Set the flag to true
            timEC.EnemyKilled();
        }
        Destroy(gameObject); // Destroy the enemy game object
    }
}