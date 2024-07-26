using System.Collections; // Ensure this is included
using UnityEngine;

public class RatController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // Speed at which the enemy moves
    [SerializeField] private int damageAmount = 1; // Amount of damage to apply

    [SerializeField] private int maxHealth = 1; // Maximum health for the rat
    private int currentHealth; // Current health of the rat
    private Rigidbody2D enemyRigidbody; // Reference to the Rigidbody2D component
    public Boss boss;

    private Collider2D ratCollider; // Reference to the rat's collider
    private bool hasTakenDamage = false; // Flag to track if damage has been applied

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        boss = FindObjectOfType<Boss>();
        currentHealth = maxHealth; // Initialize current health
        ratCollider = GetComponent<Collider2D>(); // Get the Collider2D component attached to the rat
    }

    void Update()
    {
        if (IsFacingRight())
        {
            enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            enemyRigidbody.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debugging log to check collision
        Debug.Log($"Collision detected with {collision.gameObject.name}");

        // Check if the collider is a boundary
        if (collision.CompareTag("Boundary"))
        {
            Flip();
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
        else
        {
            // Log a message if the collision is not with a Bullet
            if (!collision.CompareTag("Bullet"))
            {
                Debug.Log("Collision detected but not with a Bullet.");
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

    private void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    private void Die()
    {
        Debug.Log("Rat is dying");
        if (boss != null)
        {
            boss.KillRat(); // Notify the boss
        }
        Destroy(gameObject); // Destroy the rat game object
    }

    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ratCollider.enabled = true; // Re-enable the collider after the delay
        hasTakenDamage = false; // Reset the damage flag
    }
}
