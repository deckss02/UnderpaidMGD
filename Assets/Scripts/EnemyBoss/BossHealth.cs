using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 120f;
    private float currentHealth;
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
            animator.SetBool("Damage", true);
        }
        else
        {
            TriggerDamageAnimation();
        }
    }

    void TriggerDamageAnimation()
    {
        if (animator != null)
        {
            Debug.Log("Took damage animation");
            animator.SetBool("Damage", true);
        }
    }

    public void TakeDamageAnimationStart()
    {
        if (animator != null)
        {
            animator.SetBool("Damage", true);
        }
    }

    public void TakeDamageAnimationEnd()
    {
        if (animator != null)
        {
            animator.SetBool("Damage", false);
        }
        // Additional logic when damage animation ends can go here
    }

    void Die()
    {
        // Handle the death of the boss (e.g., play animation, trigger events, etc.)
        Debug.Log("Boss defeated!"); // Log a message for debugging purposes


        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        Destroy(gameObject); // Destroy the boss GameObject
        theWinScreen.SetActive(true);
    }
}
