using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"Enemy Health initialized with {currentHealth} health points.");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"Enemy took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("Enemy health reached zero. Destroying enemy.");
            Destroy(gameObject);
        }
    }
}
