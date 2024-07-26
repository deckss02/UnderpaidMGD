using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public Transform[] spawnPoints; // Array to hold spawn points
    public Color gizmoColor = Color.green; // Color of the gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        // Draw gizmos for each spawn point
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null) // Check if the spawn point is not null
            {
                Gizmos.DrawWireSphere(spawnPoint.position, 0.2f);
            }
        }
    }
}
