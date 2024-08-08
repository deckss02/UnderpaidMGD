using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemySpawningIn;
    public GameObject spawnerPosition;
    public float enemySpawnRate;
    private bool _spawnEnemies;

    IEnumerator SpawnRoutine()
    {
        Vector2 spawnPoint = spawnerPosition.transform.position;
        while (_spawnEnemies)
        {
            yield return new WaitForSeconds(enemySpawnRate);

            GameObject newEnemy = Instantiate(enemySpawningIn, spawnPoint, Quaternion.identity);
            EnemyController enemyController = newEnemy.GetComponent<EnemyController>();

        }
    }

    public void StartSpawning()
    {
        _spawnEnemies = true;
        StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        _spawnEnemies = false;
    }

    void Start()
    {
        StartSpawning();
    }
}
