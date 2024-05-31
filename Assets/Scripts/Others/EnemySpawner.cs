using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemySpawningIn;

    public GameObject spawnerPosition;

    public float enemySpawnRate;
    public bool _spawnEnemies;

    IEnumerator SpawnRoutine()
    {
        Vector2 spawnPoint = spawnerPosition.transform.position;

        while (_spawnEnemies == true)
            {
            yield return new WaitForSeconds(enemySpawnRate);
            Instantiate(enemySpawningIn, spawnPoint, Quaternion.identity);

        }
    }

    public void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
}
