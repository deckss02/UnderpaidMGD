using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    //[SerializeField] private float moveSpeed = 1f; // Speed at which the enemy moves
    [SerializeField] private int damageAmount = 1; // Amount of damage to apply
    [SerializeField] private int maxHealth = 1; // Maximum health for the enemy
    private int currentHealth; // Current health of the enemy
    private Rigidbody2D enemyRigidbody; // Reference to the Rigidbody2D component
    public TimEC timEC;

    private SimpleFlash simpleFlash;
    private bool isKilled = false; // Flag to check if the enemy is already counted as killed

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        currentHealth = maxHealth; // Initialize current health
        timEC = FindObjectOfType<TimEC>();
        simpleFlash = GetComponent<SimpleFlash>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            timEC.EnemyKilled();
            Destroy(gameObject, 0.3f);
        }
    }
}
