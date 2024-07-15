using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuildButtons : MonoBehaviour
{
    public int LevelNumber;

    public GameObject LevelOnePrimerCanvas;
    public GameObject LevelTwoPrimerCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        switch (LevelNumber)
        {
            case 0:
                LevelOnePrimerCanvas.SetActive(true);
                break;
            case 1:
                LevelTwoPrimerCanvas.SetActive(true);
                break;
        }
    }

    public void StartLevelOne()
    {
        SceneManager.LoadScene("MGD");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Town");
    }
}
