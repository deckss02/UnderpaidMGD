using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // Speed at which the enemy moves
    [SerializeField] private int damageAmount = 1; // Amount of damage to apply
    private Rigidbody2D enemyRigidbody; // Reference to the Rigidbody2D component
    private EnemyHealth enemyHealth; // Reference to the EnemyHealth script

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        enemyHealth = GetComponent<EnemyHealth>(); // Get the EnemyHealth component attached to the enemy

        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component not found on the specified boss object or its parents.");
        }
        else
        {
            Debug.Log("EnemyHealth component found.");
        }
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
        // Check if the collider is a boundary
        if (collision.CompareTag("Boundary"))
        {
            Flip();
        }

        // Check if the collider is tagged as a Bullet and if EnemyHealth is not null
        if (collision.CompareTag("Bullet") && enemyHealth != null)
        {
            Debug.Log($"Bullet hit enemy. Applying {damageAmount} damage.");
            enemyHealth.TakeDamage(damageAmount);
            Destroy(collision.gameObject); // Destroy the bullet after hitting the enemy

            // Check if the enemy is destroyed (health <= 0) and handle accordingly
            if (enemyHealth.currentHealth <= 0)
            {
                Die(); // Destroy the enemy
            }
        }
        else
        {
            // Log a message if the collision is not with a Bullet
            if (!collision.CompareTag("Bullet"))
            {
                Debug.Log("Collision detected but not with a Bullet.");
            }
            // Log an error if EnemyHealth component is missing
            if (enemyHealth == null)
            {
                Debug.Log("Cannot apply damage because EnemyHealth component is missing.");
            }
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    // Destroy the enemy game object
    private void Die()
    {
        Destroy(gameObject); // Destroy the enemy game object
    }
}

