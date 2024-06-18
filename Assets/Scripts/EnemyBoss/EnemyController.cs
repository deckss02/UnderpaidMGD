using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // Speed at which the enemy moves

    private Rigidbody2D enemyRigidbody; // Reference to the Rigidbody2D component
    public EnemyCounter theEnemyCounter; // Reference to the EnemyCounter script
    public EnemyHealth enemyHealth; // Reference to the EnemyHealth script

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        theEnemyCounter = FindObjectOfType<EnemyCounter>(); // Find the EnemyCounter component in the scene
        enemyHealth = GetComponent<EnemyHealth>(); // Get the EnemyHealth component attached to the enemy
    }

    // Update is called once per frame
    void Update()
    {
        // Check the direction the enemy is facing and set its velocity accordingly
        if (IsFacingRight())
        {
            enemyRigidbody.velocity = new Vector2(moveSpeed, 0f); // Move right
        }
        else
        {
            enemyRigidbody.velocity = new Vector2(-moveSpeed, 0f); // Move left
        }
    }

    // Check if the enemy is facing right
    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon; // If localScale.x is positive, the enemy is facing right
    }

    // Called when the collider exits another collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Flip the enemy's direction by inverting its localScale.x
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)), transform.localScale.y);
    }

    // Called when the enemy takes damage
    public void TakeDamage(int amount)
    {
        // Reduce health using the EnemyHealth component
        enemyHealth.TakeDamage(amount);

        // Check if the enemy is destroyed (health <= 0) and handle accordingly
        if (enemyHealth.currentHealth <= 0)
        {
            Die(); // Destroy the enemy
        }
    }

    // Destroy the enemy game object
    private void Die()
    {
        Destroy(gameObject); // Destroy the enemy game object
    }
}