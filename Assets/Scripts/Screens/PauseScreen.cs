using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private PlayerControllera thePlayer;
    private LevelManager theLevelManager;
    public GameObject Button;

    public float countdownTime;
    public TextMeshProUGUI countdownDisplay;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerControllera>();
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    { 
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Freeze the game
        countdownTime = 3;
        pauseMenu.SetActive(true);
        Button.SetActive(false);
        countdownDisplay.gameObject.SetActive(true);

        countdownDisplay.text = " ";

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

        StartCoroutine(Countdown());

        Time.timeScale = 1.0f; // Resume back to normal real-time

        
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

    IEnumerator Countdown()
    {
        while(countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        pauseMenu.SetActive(false);
        Button.SetActive(true);

        countdownDisplay.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(1f);

        
        ResumeGame();
    }
}
