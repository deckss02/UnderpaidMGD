using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public GameObject theWinScreen;
    private PlayerController thePlayer;
    private LevelManager theLevelManager;
    public GameObject UIHolder;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theLevelManager = FindObjectOfType<LevelManager>();
        UIHolder.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0) //Check if game is paused
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
        Time.timeScale = 0; //Freeze the game
        theWinScreen.SetActive(true);
        thePlayer.canMove = false;
        theLevelManager.levelMusic.Pause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f; //resume back to normal real time
        //theWinScreen.SetActive(false);
        thePlayer.canMove = true;
        theLevelManager.levelMusic.Play();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1.0f; //Avoid game stuck at frozen when changing
        SceneManager.LoadScene("StartGame");
        UIHolder.SetActive(false);

    }

}
