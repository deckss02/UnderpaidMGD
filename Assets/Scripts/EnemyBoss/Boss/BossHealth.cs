using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 120f;
    public float currentHealth;
    public Slider healthBar; // Reference to a health bar UI component to display boss health
    public GameObject theWinScreen;
    private Animator animator;
    private SpriteRenderer bossSpriteRenderer;
    private Color originalColor; // Store the original color of the boss sprite

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration = 50.0f; // Increase this value for a longer invincibility period
    [SerializeField] private int numberOfFlashes = 1;

    private Collider2D[] weakPointColliders; // References to all weak point colliders
    private bool isInvincible = false;
    private PlayerControllera playerController; // Reference to the player's controller script

    void Start()
    {
        // Initialize health and components
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Get the Animator component
        bossSpriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component from the current GameObject

        // Store the original color of the boss sprite
        originalColor = bossSpriteRenderer.color;

        // Find all game objects with the tag "WeakPoint" and get their colliders
        GameObject[] weakPoints = GameObject.FindGameObjectsWithTag("WeakPoint");
        weakPointColliders = new Collider2D[weakPoints.Length];
        for (int i = 0; i < weakPoints.Length; i++)
        {
            weakPointColliders[i] = weakPoints[i].GetComponent<Collider2D>();
            weakPointColliders[i].enabled = false; // Disable the weak point colliders at the start
        }

        // Find the player controller in the scene
        playerController = FindObjectOfType<PlayerControllera>();
    }

    void Update()
    {
        // Update the health bar value
        healthBar.value = currentHealth;

        // Check if the boss is dead and ensure all attacks and cooldowns are stopped
        if (currentHealth <= 0 && !animator.GetBool("isDead"))
        {
            Die();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        // Ignore damage if invincible
        if (isInvincible) return;

        // Decrease current health by the damage amount
        currentHealth -= damageAmount;

        // Check if health is zero or below
        if (currentHealth <= 0)
        {
            // Call Die method to handle the boss's death
            Die();
        }
        else
        {
            // Trigger the damage animation through the animator
            if (animator != null)
            {
                animator.SetTrigger("Damage");
                StartCoroutine(Invincibility());
            }
        }
    }

    public void TriggerUltimateDamage(float damageAmount)
    {
        TakeDamage(damageAmount);

        // Check if the boss is dead and trigger the win screen
        if (currentHealth <= 0)
        {
            TriggerWinScreen();
        }
    }

    private IEnumerator Invincibility()
    {
        isInvincible = true; // Set invincibility to true

        // Ignore collisions with bullets during invincibility
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        List<Collider2D> bulletColliders = new List<Collider2D>();

        foreach (GameObject bullet in bullets)
        {
            Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
            if (bulletCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, GetComponent<Collider2D>(), true);
                bulletColliders.Add(bulletCollider);
            }
        }

        // Flashing effect to indicate invincibility
        for (int i = 0; i < numberOfFlashes; i++)
        {
            bossSpriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            bossSpriteRenderer.color = originalColor;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        // Re-enable collisions with bullets after invincibility
        foreach (Collider2D bulletCollider in bulletColliders)
        {
            Physics2D.IgnoreCollision(bulletCollider, GetComponent<Collider2D>(), false);
        }

        isInvincible = false; // Set invincibility to false
    }

    public void EnableInvincibility()
    {
        isInvincible = true;
    }

    public void DisableInvincibility()
    {
        isInvincible = false;
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

        // Freeze player controls
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Stop all attacks and cooldowns
        StopAllActions();

        // Optionally, disable the Animator
        if (animator != null)
        {
            animator.enabled = false;
        }

        // Freeze the game
        Time.timeScale = 0f;

        // Trigger the win screen
        TriggerWinScreen();
    }

    public void StopAllActions()
    {
        // Stop all attacks and cooldowns
        animator.SetBool("Summon", false);
        animator.SetBool("Paw8", false);
        animator.SetBool("Claw", false);
        animator.SetBool("HairBall", false);
        animator.SetBool("CoolDown", false);
        animator.SetBool("Damage", false);
        animator.SetBool("Idle", false);
    }

    public void TriggerWinScreen()
    {
        StartCoroutine(TriggerWinScreenCoroutine());
    }

    private IEnumerator TriggerWinScreenCoroutine()
    {
        // Wait for a moment before displaying the win screen to ensure it's visible
        yield return new WaitForSecondsRealtime(1.0f); // Adjust this delay if needed
        theWinScreen.SetActive(true);
        Time.timeScale = 1f; // Restore the time scale if you need interactions in the win screen
    }

    public void EnableWeakPoints(bool enable)
    {
        // Enable or disable weak point colliders
        foreach (var collider in weakPointColliders)
        {
            if (collider != null)
            {
                collider.enabled = enable;
            }
        }
    }
}
