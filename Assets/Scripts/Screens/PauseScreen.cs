using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private PlayerControllera thePlayer;
    private LevelManager theLevelManager;
    public GameObject Button;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerControllera>();
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Freeze the game
        pauseMenu.SetActive(true);
        Button.SetActive(false);
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

        pauseMenu.SetActive(false);
        Button.SetActive(true);
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
        SceneManager.LoadScene("Town");
        Button.SetActive(false);
    }
}
