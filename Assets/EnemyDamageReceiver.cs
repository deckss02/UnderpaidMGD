using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; // Amount of damage to apply
    [SerializeField] private GameObject enemy; // Reference to the boss object

    private EnemyHealth enemyHealth;

    void Start()
    {
        if (enemy == null)
        {
            enemyHealth = GetComponentInParent<EnemyHealth>();
            Debug.Log("No enemy specified, checking parent for EnemyHealth component.");
        }
        else
        {
            enemyHealth = enemy.GetComponent<EnemyHealth>();
            Debug.Log($"Checking specified enemy object: {enemy.name} for EnemyHealth component.");
        }

        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component not found on the specified boss object or its parents.");
        }
        else
        {
            Debug.Log("EnemyHealth component found.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && enemyHealth != null)
        {
            Debug.Log($"Bullet hit enemy. Applying {damageAmount} damage.");
            enemyHealth.TakeDamage(damageAmount);
            Destroy(collision.gameObject);
        }
        else
        {
            if (!collision.CompareTag("Bullet"))
            {
                Debug.Log("Collision detected but not with a Bullet.");
            }
            if (enemyHealth == null)
            {
                Debug.Log("Cannot apply damage because EnemyHealth component is missing.");
            }
        }
    }
}
