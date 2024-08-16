using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 120f;
    public FollowPath followPathComponent;
    public float currentHealth;
    public Slider healthBar; // Reference to a health bar UI component
    public GameObject theWinScreen;
    private Animator animator;
    private SpriteRenderer bossSpriteRenderer;
    private Color originalColor;
    public GameObject boss;
    public GameObject Effect;
    public float ultimateEffectDuration = 2.0f; // Duration for the ultimate effect animation

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration = 50.0f;
    [SerializeField] private int numberOfFlashes = 1;

    private Collider2D[] weakPointColliders;
    private bool isInvincible = false;
    private PlayerControllera playerController;

    [Header("Death Animation")]
    [SerializeField] private float deathAnimationDuration = 3.0f;

    // References to PawMouth and PawEye animators
    public Animator pawMouthAnimator;
    public Animator pawEyeAnimator;

    // Reference to PawMouth and PawEye GameObjects
    public GameObject pawMouth;
    public GameObject pawEye;

    // New public transform points
    public Transform fadePoint1;
    public Transform fadePoint2;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        bossSpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = bossSpriteRenderer.color;

        GameObject[] weakPoints = GameObject.FindGameObjectsWithTag("WeakPoint");
        weakPointColliders = new Collider2D[weakPoints.Length];
        for (int i = 0; i < weakPoints.Length; i++)
        {
            weakPointColliders[i] = weakPoints[i].GetComponent<Collider2D>();
            weakPointColliders[i].enabled = false;
        }

        playerController = FindObjectOfType<PlayerControllera>();
        followPathComponent = GetComponent<FollowPath>();
    }

    void Update()
    {
        healthBar.value = currentHealth;

        if (currentHealth <= 0 && !animator.GetBool("isDead"))
        {
            Die();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isInvincible) return;

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("Damage");

                pawMouthAnimator.SetTrigger("Da");
                pawEyeAnimator.SetTrigger("Da");

                StartCoroutine(Invincibility());
            }
        }
    }

    private IEnumerator Invincibility()
    {
        isInvincible = true;

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
            bossSpriteRenderer.color = originalColor;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        foreach (Collider2D bulletCollider in bulletColliders)
        {
            Physics2D.IgnoreCollision(bulletCollider, GetComponent<Collider2D>(), false);
        }

        isInvincible = false;
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
        Debug.Log("Boss defeated!");

        StopAllActions();

        if (animator != null)
        {
            animator.SetBool("isDead", true);

            pawMouthAnimator.SetTrigger("Death");
            pawEyeAnimator.SetTrigger("Death");

            DeactivateFollowPath();
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        StartCoroutine(HandleDeathAnimation());
    }

    public void DeactivateFollowPath()
    {
        if (followPathComponent != null)
        {
            followPathComponent.SetActive(false);
        }
    }

    private IEnumerator HandleDeathAnimation()
    {
            Effect.gameObject.SetActive(true);
            //// Optionally, you can ensure the particle system is played correctly
            //ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            //if (ps != null)
            //{
            //    ps.Play();
            //}


        // Play the death animation
        if (animator != null)
        {
            animator.SetBool("isDead", true);
            DeactivateFollowPath();
        }

        // Deactivate FollowPath scripts and move the Paws
        if (pawMouth != null)
        {
            pawMouth.GetComponent<FollowPath>().SetActive(false);
            pawMouth.transform.position = fadePoint1.position;
        }
        if (pawEye != null)
        {
            pawEye.GetComponent<FollowPath>().SetActive(false);
            pawEye.transform.position = fadePoint2.position;
        }

        // Wait for the ultimate effect to finish
        yield return new WaitForSeconds(ultimateEffectDuration);

        // Wait for the death animation to finish before triggering the win screen
        yield return new WaitForSeconds(deathAnimationDuration);

        // Disable the animator and pause the game
        if (animator != null)
        {
            animator.enabled = false;
        }

        Time.timeScale = 0f;
        TriggerWinScreen();
    }

    public void StopAllActions()
    {
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
        yield return new WaitForSecondsRealtime(1.0f);
        theWinScreen.SetActive(true);
        Time.timeScale = 1f;
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

    public void OnUltimateEffectTriggered()
    {
        // This method is called by UltimateEffect when the collider is triggered
        StartCoroutine(HandleDeathAnimation());
    }
}
