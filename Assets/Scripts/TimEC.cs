using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimEC : MonoBehaviour
{
    public int enemiesLeftToKill = 0;
    public GameObject boss;
    public Button Ultimate;
    public GameObject BossHealth;

    public PlayerControllera playerController; // Reference to the player's controller script
    public TextMeshProUGUI enemiesLeftText;
    public float freezeTime = 5.0f; // Duration to freeze the player
    public GameObject ES1;
    public GameObject ES2;
    public GameObject ES3;
    public GameObject ES4;
    public GameObject ES5;
    public GameObject ES6;
    public GameObject ES7;
    public GameObject ES8;
    public GameObject ES9;
    public GameObject ES10;

    public GameObject Skull;
    public GameObject Sparkle;

    private SpriteRenderer bossSpriteRenderer; // Reference to the boss's SpriteRenderer
    public float fadeDuration = 2.0f; // Duration for the boss to fade in

    void Start()
    {
        BossHealth.gameObject.SetActive(false);
        Ultimate.gameObject.SetActive(false);
        playerController = FindObjectOfType<PlayerControllera>();
        UpdateEnemyCountText();

        boss.SetActive(false); // Initially deactivate the boss
        bossSpriteRenderer = boss.GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component of the boss

        // Set the initial alpha of the boss to 0 (fully transparent)
        if (bossSpriteRenderer != null)
        {
            Color color = bossSpriteRenderer.color;
            color.a = 0;
            bossSpriteRenderer.color = color;
        }

        Skull.gameObject.SetActive(false);
        Sparkle.gameObject.SetActive(false);
    }

    public void EnemyKilled()
    {
        Debug.Log("EnemyKilled method called");

        enemiesLeftToKill--;
        UpdateEnemyCountText();

        if (enemiesLeftToKill <= 0)
        {
            StartCoroutine(ActivateBossSequence());
        }
    }

    private IEnumerator ActivateBossSequence()
    {
        playerController.FreezePlayer(true);
        DestroyAllEnemies();

        ES1.gameObject.SetActive(false);
        ES2.gameObject.SetActive(false);
        ES3.gameObject.SetActive(false);
        ES4.gameObject.SetActive(false);
        ES5.gameObject.SetActive(false);
        ES6.gameObject.SetActive(false);
        ES7.gameObject.SetActive(false);
        ES8.gameObject.SetActive(false);
        ES9.gameObject.SetActive(false);
        ES10.gameObject.SetActive(false);

        Skull.gameObject.SetActive(true);
        Sparkle.gameObject.SetActive(true);

        yield return new WaitForSeconds(freezeTime);

        BossHealth.gameObject.SetActive(true);
        Ultimate.gameObject.SetActive(true);

        // Activate the boss and start the fade-in coroutine
        StartCoroutine(FadeInBoss());
        boss.SetActive(true);

        playerController.FreezePlayer(false);
    }

    private IEnumerator FadeInBoss()
    {
        if (bossSpriteRenderer != null)
        {
            float elapsedTime = 0f;
            Color color = bossSpriteRenderer.color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                bossSpriteRenderer.color = color;
                yield return null;
            }

            // Ensure the boss is fully visible after the fade-in
            color.a = 1f;
            bossSpriteRenderer.color = color;
        }
    }

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
        boss.SetActive(true);
        Ultimate.gameObject.SetActive(true);
    }

    void UpdateEnemyCountText()
    {
        enemiesLeftText.text = "Enemies Left: " + enemiesLeftToKill;
    }
}
