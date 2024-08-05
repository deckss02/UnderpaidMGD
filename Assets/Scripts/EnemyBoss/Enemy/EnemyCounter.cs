using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    public int enemiesLeftToKill = 0;
    public GameObject boss;
    public Button Ultimate;

    public PlayerControllera playerController; // Reference to the player's controller script
    public TextMeshProUGUI enemiesLeftText;
    public float freezeTime = 5.0f; // Duration to freeze the player
    public GameObject ES1;
    public GameObject ES2;
    public GameObject ES3;
    public GameObject ES4;

    public GameObject shootingPawPrefab; // Prefab for the first Shooting Paw
    public GameObject shootingPawPrefab1; // Prefab for the second Shooting Paw
    public int shootingPawSpawnThreshold = 15; // Number of enemies left when the Shooting Paws should spawn

    private GameObject shootingPawInstance1; // Instance of the first Shooting Paw
    private GameObject shootingPawInstance2; // Instance of the second Shooting Paw

    void Start()
    {
        Ultimate.gameObject.SetActive(false);
        playerController = FindObjectOfType<PlayerControllera>();
        // Update the TextMeshPro with initial enemies count
        UpdateEnemyCountText();

        // Deactivate boss initially
        boss.SetActive(false);

        // Deactivate shooting paws initially
        if (shootingPawPrefab != null)
            shootingPawPrefab.SetActive(false);
        if (shootingPawPrefab1 != null)
            shootingPawPrefab1.SetActive(false);
    }

    // Function to be called when an enemy is killed
    public void EnemyKilled()
    {
        Debug.Log("EnemyKilled method called");

        // Reduce the count of enemies left to kill
        enemiesLeftToKill--;

        // Update the TextMeshPro with new enemies count
        UpdateEnemyCountText();

        // Check if the Shooting Paws should be activated
        CheckShootingPaws();

        // If no enemies left, start the boss activation sequence
        if (enemiesLeftToKill <= 0)
        {
            StartCoroutine(ActivateBossSequence());
        }
    }

    // Function to check if the Shooting Paws should be activated or destroyed
    void CheckShootingPaws()
    {
        if (enemiesLeftToKill <= shootingPawSpawnThreshold)
        {
            if (shootingPawInstance1 == null)
            {
                shootingPawInstance1 = Instantiate(shootingPawPrefab, transform.position, Quaternion.identity);
                shootingPawInstance1.SetActive(true);
            }
            if (shootingPawInstance2 == null)
            {
                shootingPawInstance2 = Instantiate(shootingPawPrefab1, transform.position, Quaternion.identity);
                shootingPawInstance2.SetActive(true);
            }
        }
    }

    // Coroutine to handle the boss activation sequence
    private IEnumerator ActivateBossSequence()
    {
        // Freeze the player
        playerController.FreezePlayer(true);

        // Destroy all remaining enemies
        DestroyAllEnemies();

        ES1.gameObject.SetActive(false);
        ES2.gameObject.SetActive(false);
        ES3.gameObject.SetActive(false);
        ES4.gameObject.SetActive(false);

        // Wait for the specified freeze time
        yield return new WaitForSeconds(freezeTime);

        // Activate the boss
        ActivateBoss();

        // Unfreeze the player
        playerController.FreezePlayer(false);
    }

    // Function to destroy all remaining enemies in the scene
    void DestroyAllEnemies()
    {
        shootingPawPrefab.SetActive(false);
        shootingPawPrefab1.SetActive(false);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    void ActivateBoss()
    {
        Debug.Log("All enemies killed. Activating boss.");
        // Activate the boss GameObject
        boss.SetActive(true);
        Ultimate.gameObject.SetActive(true);
    }

    // Update the TextMeshPro with the remaining enemies count
    void UpdateEnemyCountText()
    {
        enemiesLeftText.text = "Enemies Left: " + enemiesLeftToKill;
    }
}
