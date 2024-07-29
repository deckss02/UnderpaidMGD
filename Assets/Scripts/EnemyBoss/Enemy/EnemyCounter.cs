using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    public int enemiesLeftToKill = 0;
    public GameObject boss;
    public Button Ultimate;
    public TextMeshProUGUI enemiesLeftText;
    public float freezeTime = 5.0f; // Duration to freeze the player
    private PlayerControllera playerController; // Reference to the PlayerController script
    public GameObject ES1;
    public GameObject ES2;
    public GameObject ES3;

    void Start()
    {
        Ultimate.gameObject.SetActive(false);

        // Update the TextMeshPro with initial enemies count
        UpdateEnemyCountText();

        // Deactivate boss initially
        boss.SetActive(false);

        // Find the PlayerController in the scene
        playerController = FindObjectOfType<PlayerControllera>();

        // Check if PlayerController was found
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
    }

    // Function to be called when an enemy is killed
    public void EnemyKilled()
    {
        Debug.Log("EnemyKilled method called");

        // Reduce the count of enemies left to kill
        enemiesLeftToKill--;

        // Update the TextMeshPro with new enemies count
        UpdateEnemyCountText();

        // If no enemies left, start the boss activation sequence
        if (enemiesLeftToKill <= 0)
        {
            StartCoroutine(ActivateBossSequence());
        }
    }

    // Coroutine to handle the boss activation sequence
    private IEnumerator ActivateBossSequence()
    {
        // Freeze the player
        FreezePlayer(true);

        // Destroy all remaining enemies
        DestroyAllEnemies();

        ES1.gameObject.SetActive(false);
        ES2.gameObject.SetActive(false);
        ES3.gameObject.SetActive(false);

        // Wait for the specified freeze time
        yield return new WaitForSeconds(freezeTime);

        // Activate the boss
        ActivateBoss();

        // Unfreeze the player
        FreezePlayer(false);
    }

    // Function to freeze or unfreeze the player
    private void FreezePlayer(bool freeze)
    {
        if (playerController != null)
        {
            // Toggle the player's ability to move
            playerController.canMove = !freeze;
        }
    }

    // Function to destroy all remaining enemies in the scene
    void DestroyAllEnemies()
    {
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