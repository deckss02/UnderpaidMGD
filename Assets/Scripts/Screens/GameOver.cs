using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public GameObject UIHolder;

    [SerializeField] private GameObject tryAgainButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Town");
        Time.timeScale = 0;
        UIHolder.SetActive(false);
    }

    //Quit Game
    public void QuitGame()
    {
        Application.Quit();
        UIHolder.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level 1");
        tryAgainButton.SetActive(true);
        UIHolder.SetActive(true);
    }

}
