using System.Collections;
using UnityEngine;

public class UltimateEffect : MonoBehaviour
{
    private BossHealth bossHealth; // Reference to the BossHealth script
    private float ultimateDamage; // Damage dealt by the ultimate
    private bool hasDamaged = false; // Flag to ensure damage is dealt only once
    public BoxCollider2D boxCollider; // Reference to the BoxCollider2D component
    public float colliderDisableDuration = 4.0f; // Duration to disable the collider

    public void Initialize(BossHealth bossHealth, float ultimateDamage)
    {
        this.bossHealth = bossHealth;
        this.ultimateDamage = ultimateDamage;
    }

    public void ActivateUltimateEffect()
    {
        gameObject.SetActive(true);

        if (boxCollider != null)
        {
            StartCoroutine(HandleColliderDelay());
        }
    }

    private IEnumerator HandleColliderDelay()
    {
        // Disable the collider
        boxCollider.enabled = false;

        // Wait for the specified duration
        yield return new WaitForSeconds(colliderDisableDuration);

        // Re-enable the collider
        boxCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss") && !hasDamaged)
        {
            hasDamaged = true;

            if (bossHealth != null)
            {
                // Deal damage to the boss
                bossHealth.TakeDamage(ultimateDamage);

                // Notify the boss that the ultimate effect has finished
                bossHealth.OnUltimateEffectTriggered();

                Debug.Log("Ultimate collision triggered! Dealt " + ultimateDamage + " damage.");
            }

            Destroy(gameObject);
        }
    }
}
