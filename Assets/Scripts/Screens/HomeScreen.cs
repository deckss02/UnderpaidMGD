using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    public GameObject TownCanvas;
    public GameObject UIHolder;
    private PlayerController Player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player = FindObjectOfType<PlayerController>();

    }



    void HomePauseScreen()
    {
        if (Time.timeScale == 0)
        {
            HomeUI();
        }
        else
        {
            UnPauseGame();
        }
    }

    public void HomeUI()
    {
        Time.timeScale = 0;
        TownCanvas.SetActive(true);
        UIHolder.SetActive(false);
        Player.canMove = false;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        TownCanvas.SetActive(false);
        UIHolder.SetActive(true);
        Player.canMove = true;
    }
}
