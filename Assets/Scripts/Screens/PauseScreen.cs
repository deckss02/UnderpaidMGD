using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public GameObject thePauseScreen;
    private PlayerController thePlayer;
    private LevelManager theLevelManager;
    public Button pauseButton; // Reference to the pause button

    public GameObject UIHolder;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theLevelManager = FindObjectOfType<LevelManager>();

        if (theLevelManager == null)
        {
            Debug.LogError("LevelManager not found in the scene.");
        }
        else if (theLevelManager.levelMusic == null)
        {
            Debug.LogError("LevelMusic not assigned in LevelManager.");
        }

        // Set up the button listener
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }
        else
        {
            Debug.LogError("Pause button not assigned in the inspector.");
        }
    }

    void TogglePause()
    {
        if (Time.timeScale == 0) // Check if the game is paused
        {
            ResumeGame(); // When the game is paused and the button is pressed, resume the game
        }
        else
        {
            PauseGame(); // When the game is running and the button is pressed, pause the game
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Freeze the game
        thePauseScreen.SetActive(true);
        UIHolder.SetActive(false);
        if (thePlayer != null)
        {
            thePlayer.canMove = false;
        }

        if (theLevelManager != null && theLevelManager.levelMusic != null)
        {
            theLevelManager.levelMusic.Pause();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f; // Resume back to normal real-time

        thePauseScreen.SetActive(false);
        UIHolder.SetActive(true);
        if (thePlayer != null)
        {
            thePlayer.canMove = true;
        }

        if (theLevelManager != null && theLevelManager.levelMusic != null)
        {
            theLevelManager.levelMusic.Play();
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1.0f; // Avoid game stuck at frozen when changing
        SceneManager.LoadScene("StartGame");
        UIHolder.SetActive(false);
    }
}
