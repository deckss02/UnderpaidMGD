using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashDamage : MonoBehaviour
{
    public float damageAmount = 20f; // Amount of damage to deal

    // This method will be called by the animation event
    public void DealDamage()
    {
        // Find all colliders overlapping with the slash position
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f); // Adjust the radius as needed
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyController enemyController = hitCollider.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.TakeDamage((int)damageAmount);
                }
            }
            else if (hitCollider.CompareTag("Rat"))
            {
                RatController ratController = hitCollider.GetComponent<RatController>();
                if (ratController != null)
                {
                    ratController.TakeDamage((int)damageAmount);
                }
            }
            else if (hitCollider.CompareTag("WeakPoint"))
            {
                BossHealth bossHealth = hitCollider.GetComponentInParent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(damageAmount);
                }
            }
            else if (hitCollider.CompareTag("Boss"))
            {
                BossHealth bossHealth = hitCollider.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(damageAmount);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision with the Boss using trigger detection
        if (collision.CompareTag("Boss"))
        {
            BossHealth bossHealth = collision.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damageAmount);
            }
        }
    }
}
