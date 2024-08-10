using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuildButtons : MonoBehaviour
{
    public int LevelNumber;

    public GameObject LevelOnePrimerCanvas;
    public GameObject LevelTwoPrimerCanvas;

    public Animator Transitionanim;
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
    
    public void AcceptQuest()
    {
        switch (LevelNumber)
        {

            case 0:
                StartCoroutine(LoadingLevelOne());
                break;
            case 1:
                StartCoroutine(LoadingLevelTwo());
                break;
        }
    }

 /*
    public void StartLevelOne()
    {
        SceneManager.LoadScene("MGD");
    }
*/
    public void MainMenu()
    {
        StartCoroutine(LoadingMainMenu());
    }

    IEnumerator LoadingMainMenu()
    {
        Transitionanim.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Town");
    }

    IEnumerator LoadingLevelOne()
    {
        Transitionanim.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Level 1");
    }

    IEnumerator LoadingLevelTwo()
    {
        Transitionanim.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Level 2");
    }
}
