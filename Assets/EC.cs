using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EC : MonoBehaviour
{
    public int enemiesLeftToKill = 0;
    public GameObject boss;
    public Button Ultimate;
    public GameObject BossHealth;

    public GameObject Intro;
    public PlayerControllera playerController; // Reference to the player's controller script
    public TextMeshProUGUI enemiesLeftText;
    public float freezeTime = 5.0f; // Duration to freeze the player
    public GameObject ES1;
    public GameObject ES2;
    public GameObject ES3;
    public GameObject ES4;

    void Start()
    {
        BossHealth.gameObject.SetActive(false);
        Ultimate.gameObject.SetActive(false);
        playerController = FindObjectOfType<PlayerControllera>();
        // Update the TextMeshPro with initial enemies count
        UpdateEnemyCountText();

        // Deactivate boss initially
        boss.SetActive(false);
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
        playerController.FreezePlayer(true);

        // Destroy all remaining enemies
        DestroyAllEnemies();

        ES1.gameObject.SetActive(false);
        ES2.gameObject.SetActive(false);
        ES3.gameObject.SetActive(false);
        ES4.gameObject.SetActive(false);

        Intro.gameObject.SetActive(true);
        // Wait for the specified freeze time
        yield return new WaitForSeconds(freezeTime);

        BossHealth.gameObject.SetActive(true);
        // Activate the boss
        ActivateBoss();
        Intro.gameObject.SetActive(false);
        // Unfreeze the player
        playerController.FreezePlayer(false);
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
