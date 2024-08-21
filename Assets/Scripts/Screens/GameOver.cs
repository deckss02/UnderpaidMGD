using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject UIHolder;

    [SerializeField] private GameObject tryAgainButton;

    void Start()
    {
        // Initialization if needed
    }

    void Update()
    {
        // Update logic if needed
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Town");
        Time.timeScale = 0;
        UIHolder.SetActive(false);
    }

    // Quit Game
    public void QuitGame()
    {
        Application.Quit();
        UIHolder.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the current scene
        SceneManager.LoadScene(currentScene.name);

        // Optionally manage UI visibility here
        tryAgainButton.SetActive(true);
        UIHolder.SetActive(true);
    }
}
