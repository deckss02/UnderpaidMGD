using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public GameObject theWinScreen;
    private PlayerControllera thePlayer;
    private LevelManager theLevelManager;
    public GameObject UIHolder;

    void Start()
    {
        // Attempt to find the PlayerController in the scene
        thePlayer = FindObjectOfType<PlayerControllera>();
        if (thePlayer == null)
        {
            Debug.LogError("PlayerController not found!");
        }

        // Attempt to find the LevelManager in the scene
        theLevelManager = FindObjectOfType<LevelManager>();
        if (theLevelManager == null)
        {
            Debug.LogError("LevelManager not found!");
        }

        // Ensure UIHolder is set
        if (UIHolder != null)
        {
            UIHolder.SetActive(true);
        }
        else
        {
            Debug.LogError("UIHolder not assigned!");
        }
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            PauseGame(); // When game is running and press Escape button, pause game
        }
        else
        {
            ResumeGame(); //When game is paused and press Escape button, resume game
        }
    }

    public void PauseGame()
    {
        if (thePlayer != null && theLevelManager != null)
        {
            Time.timeScale = 0; // Freeze the game
            theWinScreen.SetActive(true);
            thePlayer.canMove = false;
            theLevelManager.levelMusic.Pause();
        }
        else
        {
            Debug.LogError("PauseGame called but references are not set!");
        }
    }

    public void ResumeGame()
    {
        if (thePlayer != null && theLevelManager != null)
        {
            Time.timeScale = 1.0f; // Resume back to normal real time
            // theWinScreen.SetActive(false);
            thePlayer.canMove = true;
            theLevelManager.levelMusic.Play();
        }
        else
        {
            Debug.LogError("ResumeGame called but references are not set!");
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1.0f; // Avoid game stuck at frozen when changing
        SceneManager.LoadScene("StartGame");
        if (UIHolder != null)
        {
            UIHolder.SetActive(false);
        }
        else
        {
            Debug.LogError("UIHolder is not assigned!");
        }
    }
}
