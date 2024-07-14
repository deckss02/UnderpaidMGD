using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSummonManager : MonoBehaviour
{
    public event System.Action OnAllRatsDestroyed; // Event to notify when all rats are destroyed

    public GameObject ratPrefab; // Prefab for the rat enemy
    public Transform[] spawnPoints; // Points where rats will be spawned

    private List<GameObject> activeRats = new List<GameObject>(); // List to track active rats

    // Method to start summoning rats
    public void StartSummoning(int numberOfRats)
    {
        // Example logic to summon rats
        for (int i = 0; i < numberOfRats; i++)
        {
            // Instantiate rats at spawn points
            GameObject ratInstance = Instantiate(ratPrefab, GetRandomSpawnPoint(), Quaternion.identity);
            activeRats.Add(ratInstance);
        }
    }

    // Method to check if all summoned rats are destroyed
    public bool AreAllRatsDestroyed()
    {
        foreach (GameObject rat in activeRats)
        {
            if (rat != null)
                return false;
        }
        // If no rats are active, invoke the event
        OnAllRatsDestroyed?.Invoke();
        return true;
    }

    // Example method to get random spawn points
    private Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }
}
