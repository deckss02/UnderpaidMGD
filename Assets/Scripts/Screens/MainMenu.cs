using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //New Game
    public void NewGame()
    {
        SceneManager.LoadScene("MGD");
    }
    
    //Quit Game
    public void QuitGame()
    {
        Application.Quit();
    }
}
