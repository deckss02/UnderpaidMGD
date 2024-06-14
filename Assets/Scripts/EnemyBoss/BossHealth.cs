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
    private SpriteRenderer bossSpriteRenderer;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration = 50.0f; // Increase this value for a longer invincibility period
    [SerializeField] private int numberOfFlashes = 1;

    private Collider2D[] weakPointColliders; // References to all weak point colliders

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Get the Animator component
        //theWinScreen.SetActive(false);
        bossSpriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component from the current GameObject
        // Find all game objects with the tag "WeakPoint" and get their colliders
        GameObject[] weakPoints = GameObject.FindGameObjectsWithTag("WeakPoint");
        weakPointColliders = new Collider2D[weakPoints.Length];
        for (int i = 0; i < weakPoints.Length; i++)
        {
            weakPointColliders[i] = weakPoints[i].GetComponent<Collider2D>();
            weakPointColliders[i].enabled = false; // Disable the weak point colliders at the start
        }
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
                StartCoroutine(Invincibility());

            }
        }
    }

    private IEnumerator Invincibility()
    {
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

        for (int i = 0; i < numberOfFlashes; i++)
        {
            bossSpriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            bossSpriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        foreach (Collider2D bulletCollider in bulletColliders)
        {
            Physics2D.IgnoreCollision(bulletCollider, GetComponent<Collider2D>(), false);
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

    public void EnableWeakPoints(bool enable)
    {
        foreach (var collider in weakPointColliders)
        {
            if (collider != null)
            {
                collider.enabled = enable;
            }
        }
    }
}

