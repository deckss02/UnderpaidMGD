using System.Collections; // Ensure this is included
using UnityEngine;

public class RatController : MonoBehaviour
{
    public float moveSpeed = 1f; // Speed at which the enemy moves
    public int direction = 1;
    public SpriteRenderer spriteRenderer;

    [SerializeField] private int damageAmount = 1; // Amount of damage to apply
    [SerializeField] private int maxHealth = 1; // Maximum health for the enemy
    private int currentHealth; // Current health of the enemy
    private Rigidbody2D enemyRigidbody; // Reference to the Rigidbody2D component
    public Boss boss;

    private Collider2D ratCollider; // Reference to the rat's collider
    private bool hasTakenDamage = false; // Flag to track if damage has been applied
    private bool isKilled = false; // Flag to check if the enemy is already counted as killed
    private SimpleFlash simpleFlash;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        boss = FindObjectOfType<Boss>();
        currentHealth = maxHealth; // Initialize current health
        ratCollider = GetComponent<Collider2D>(); // Get the Collider2D component attached to the rat
        simpleFlash = GetComponent<SimpleFlash>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the movement and the sprite face appropriately
        GetComponent<Rigidbody2D>().velocity = direction * moveSpeed * Vector2.right;
        spriteRenderer.flipX = direction < 0;
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

        // Check if the collider is tagged as a Bullet and apply damage if not already done
        if (collision.CompareTag("Bullet"))
        {
            if (!hasTakenDamage)
            {
                Debug.Log($"Bullet hit rat. Applying {damageAmount} damage.");
                TakeDamage(damageAmount); // Apply damage to the rat
                Destroy(collision.gameObject); // Destroy the bullet after hitting the rat
                hasTakenDamage = true; // Set the flag to true to prevent further damage
                // Optionally, disable the collider to prevent multiple hits
                ratCollider.enabled = false;
                // Re-enable the collider after a short delay
                StartCoroutine(EnableColliderAfterDelay(0.6f));
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // Reduce current health
        if (currentHealth <= 0)
        {
            Die(); // Destroy the rat if health is 0 or less
        }
    }

    private void Die()
    {
        if (!isKilled)
        {
            isKilled = true;
            simpleFlash.Flash(); // Start the flash effect
            boss.KillRat(); // Notify the boss
            Destroy(gameObject, 0.3f);
        }
    }

    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ratCollider.enabled = true; // Re-enable the collider after the delay
        hasTakenDamage = false; // Reset the damage flag
    }
}
