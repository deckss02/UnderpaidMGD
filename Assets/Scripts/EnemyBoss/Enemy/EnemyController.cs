using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 1f; // Speed at which the enemy moves
    public int direction = 1;
    public SpriteRenderer spriteRenderer;

    [SerializeField] private int damageAmount = 1; // Amount of damage to apply
    [SerializeField] private int maxHealth = 1; // Maximum health for the enemy
    private int currentHealth; // Current health of the enemy
    private Rigidbody2D enemyRigidbody; // Reference to the Rigidbody2D component
    public EnemyCounter enemycounter;

    private SimpleFlash simpleFlash;
    public bool initialFlipX = false; // Variable to control the initial flip state
    private bool isKilled = false; // Flag to check if the enemy is already counted as killed

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        currentHealth = maxHealth; // Initialize current health
        enemycounter = FindObjectOfType<EnemyCounter>();
        simpleFlash = GetComponent<SimpleFlash>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GetComponent<Rigidbody2D>().velocity = direction * moveSpeed * Vector2.right;
        spriteRenderer.flipX = initialFlipX;

        // Reset initialFlipX to false after the initial flip is applied
        initialFlipX = false;
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.flipX = direction < 0;
        // Did we touch an edge trigger area?
        if (collision.CompareTag("EdgeTrigger"))
        {
            // Reverse direction
            direction = -direction;

            // Set the movement and the sprite face appropriately
            GetComponent<Rigidbody2D>().velocity = direction * moveSpeed * Vector2.right;
            spriteRenderer.flipX = direction < 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Check if the collider is tagged as a Bullet and apply damage
        if (collision.collider.CompareTag("Bullet"))
        {
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

    private void Die()
    {
        if (!isKilled)
        {
            isKilled = true;
            simpleFlash.Flash(); // Start the flash effect
            enemycounter.EnemyKilled();
            Destroy(gameObject, 0.3f);
        }
    }
}