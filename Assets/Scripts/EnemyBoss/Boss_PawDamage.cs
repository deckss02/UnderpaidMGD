using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_PawDamage : MonoBehaviour
{
    public int attackDamage = 20;
    public float attackRange = 2f; // Range within which the boss can attack
    public LayerMask playerLayer; // Layer mask for the player
    public Transform attackPoint; // Point from where the attack originates
    public float attackRate = 2f; // Attack rate in attacks per second
    private float nextAttackTime = 0f; // Time when the boss can perform the next attack

    private LevelManager levelManager; // Reference to the LevelManager script

    void Start()
    {
        // Get reference to the LevelManager script
        levelManager = FindObjectOfType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("LevelManager script not found in the scene!");
        }
    }

    // Animation event function to trigger the attack
    public void PerformBossAttack()
    {
        // Check if it's time to perform the attack based on attack rate
        if (Time.time >= nextAttackTime)
        {
            // Perform the attack
            Attack();
            
            // Set the next attack time based on attack rate
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    // Function to perform the attack
    void Attack()
    {
        // Detect player in range
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        // Damage each player in range
        foreach (Collider player in hitPlayers)
        {
            // Deal damage to the player's health through the LevelManager
            if (levelManager != null)
            {
                levelManager.HurtPlayer(attackDamage);
            }
            else
            {
                Debug.LogError("LevelManager reference is null!");
            }
        }
    }

    // Draw a visual representation of the attack range in the scene view (for debugging purposes)
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
