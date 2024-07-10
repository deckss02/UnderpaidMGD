using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; // Amount of damage to apply

    private EnemyHealth enemyHealth;

    void Start()
    {
        // Try to find the EnemyHealth component on the current GameObject or its parent
        enemyHealth = GetComponentInParent<EnemyHealth>();

        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component not found on the specified boss object or its parents.");
        }
        else
        {
            Debug.Log("EnemyHealth component found.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider is tagged as a Bullet and if EnemyHealth is not null
        if (collision.CompareTag("Bullet") && enemyHealth != null)
        {
            Debug.Log($"Bullet hit enemy. Applying {damageAmount} damage.");
            enemyHealth.TakeDamage(damageAmount);
            Destroy(collision.gameObject); // Destroy the bullet after hitting the enemy
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
}