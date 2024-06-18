using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; // Amount of damage to apply
    [SerializeField] private GameObject enemy; // Reference to the boss object

    private EnemyHealth enemyHealth;

    void Start()
    {
        // If the enemy is not set, try to find the EnemyHealth in the parent
        if (enemy == null)
        {
            enemyHealth = GetComponentInParent<EnemyHealth>();
        }
        else
        {
            enemyHealth = enemy.GetComponent<EnemyHealth>();
        }

        // Check if the EnemyHealth component was found
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component not found on the specified boss object or its parents.");
        }
    }

    // Called when another collider enters the trigger collider attached to the enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider has the tag "Bullet"
        if (collision.CompareTag("Bullet") && enemyHealth != null)
        {
            // Inform the EnemyHealth component to take damage
            enemyHealth.TakeDamage(damageAmount);

            // Optionally, you might want to destroy the bullet upon collision
            Destroy(collision.gameObject);
        }
    }
}
