using UnityEngine;

public class EnemyController1 : MonoBehaviour
{
    public float moveSpeed = 1f; // Speed at which the enemy moves
    public int direction = 1;
    public SpriteRenderer spriteRenderer;

    [SerializeField] private int damageAmount = 1; // Amount of damage to apply
    [SerializeField] private int maxHealth = 1; // Maximum health for the enemy
    private int currentHealth; // Current health of the enemy
    private Rigidbody2D enemyRigidbody; // Reference to the Rigidbody2D component
    public TimEC timEC;

    private SimpleFlash simpleFlash;
    private bool isKilled = false; // Flag to check if the enemy is already counted as killed
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        currentHealth = maxHealth; // Initialize current health
        timEC = FindObjectOfType<TimEC>();
        simpleFlash = GetComponent<SimpleFlash>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the movement and the sprite face appropriately
        GetComponent<Rigidbody2D>().velocity = direction * moveSpeed * Vector2.right;
        spriteRenderer.flipX = direction < 0;
        // We use direction instead of velocity because
        // (a) easier to compare ints
        // (b) can handle a sprite that faces the wrong way
        //     simply by setting a negative moveSpeed.
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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

    private void Die()
    {
        if (!isKilled)
        {
            isKilled = true;
            simpleFlash.Flash(); // Start the flash effect
            timEC.EnemyKilled();
            Destroy(gameObject, 0.3f);
        }
    }
}