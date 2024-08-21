using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UEW : MonoBehaviour
{
    public AudioSource UltimateSource;
    public AudioClip UltimateClip;

    private BossHealthWolf bossHealthW; // Reference to the BossHealth script
    private float ultimateDamage; // Damage dealt by the ultimate
    private bool hasDamaged = false; // Flag to ensure damage is dealt only once
    public BoxCollider2D boxCollider; // Reference to the BoxCollider2D component
    public float colliderDisableDuration = 4.0f; // Duration to disable the collider

    public void Initialize(BossHealthWolf bossHealthW, float ultimateDamage)
    {
        this.bossHealthW = bossHealthW;
        this.ultimateDamage = ultimateDamage;
    }

    public void ActivateUltimateEffect()
    {
        UltimateSource.PlayOneShot(UltimateClip);

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

            if (bossHealthW != null)
            {
                // Deal damage to the boss
                bossHealthW.TakeDamage(ultimateDamage);

                // Notify the boss that the ultimate effect has finished
                bossHealthW.OnUltimateEffectTriggered();

                Debug.Log("Ultimate collision triggered! Dealt " + ultimateDamage + " damage.");
            }

            Destroy(gameObject);
        }
    }

}
