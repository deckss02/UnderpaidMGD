using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 120f;
    public float currentHealth;
    public Slider healthBar; // Reference to a health bar UI component to display boss health
    public GameObject theWinScreen;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Get the Animator component
        //theWinScreen.SetActive(false);
    }

    void Update()
    {
        healthBar.value = currentHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount; // Decrease current health by the damage amount
        if (currentHealth <= 0) // Check if health is zero or below
        {
            Die(); // Call Die method to handle the boss's death
        }
        else
        {
            // Trigger the damage animation through the animator
            if (animator != null)
            {
                animator.SetTrigger("Damage");
            }
        }
    }

    public void StartHairBallRollAttack()
    {
        StartCoroutine(HairBallRollAttackCoroutine());
    }

    private IEnumerator HairBallRollAttackCoroutine()
    {
        // HairBallRoll attack logic
        // For example:
        Debug.Log("Starting HairBallRoll attack");
        yield return new WaitForSeconds(1.0f); // Replace this with your actual hairball attack duration

        // After completing the attack, transition to cooldown state
        if (animator != null)
        {
            animator.SetBool("Cooldown", true);
        }
        Debug.Log("HairBallRoll attack completed");
    }

    void Die()
    {
        // Handle the death of the boss (e.g., play animation, trigger events, etc.)
        Debug.Log("Boss defeated!"); // Log a message for debugging purposes

        // Set the "isDead" parameter to true to trigger death animation
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        Destroy(gameObject); // Destroy the boss GameObject
        theWinScreen.SetActive(true);
    }
}

