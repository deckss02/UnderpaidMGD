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
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage((int)damageAmount);
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

    // Optional: Visualize the damage area in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // Adjust the radius as needed
    }
}
