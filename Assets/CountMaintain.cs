using System.Collections.Generic;
using UnityEngine;

public class CountMaintain : MonoBehaviour
{
    public int maxEnemiesAllowed = 10;
    private int currentEnemyCount = 0;
    public List<EnemySpawner> enemySpawners;

    void Start()
    {
        // Find all EnemySpawner instances in the scene
        enemySpawners = new List<EnemySpawner>(FindObjectsOfType<EnemySpawner>());
    }

    void Update()
    {
        // Update the enemy count and manage spawning
        currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        ManageSpawners();
    }

    void ManageSpawners()
    {
        if (currentEnemyCount >= maxEnemiesAllowed)
        {
            // Stop all spawners if the enemy count exceeds the limit
            foreach (var spawner in enemySpawners)
            {
                spawner.StopSpawning();
            }
        }
        else
        {
            // Start all spawners if the enemy count is below the limit
            foreach (var spawner in enemySpawners)
            {
                spawner.StartSpawning();
            }
        }
    }
}
