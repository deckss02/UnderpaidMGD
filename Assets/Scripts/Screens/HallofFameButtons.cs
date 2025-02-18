using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HallofFameButtons : MonoBehaviour
{
    public int canvasNumber;
    public Button Leftbutton;
    public Button Rightbutton;

    public GameObject HelpersCanvas;
    public GameObject MembersCanvas;
    public GameObject AssetsCanvas;
    public GameObject Assets2Canvas;

    public Animator Transitionanim;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (canvasNumber)
        {
            case 0:
                AssetsCanvas.SetActive(true);
                HelpersCanvas.SetActive(false);
                MembersCanvas.SetActive(false);
                Assets2Canvas.SetActive(false);
                break;
            case 1:
                HelpersCanvas.SetActive(true);
                AssetsCanvas.SetActive(false);
                MembersCanvas.SetActive(false);
                Assets2Canvas.SetActive(false);
                break;
            case 2:
                HelpersCanvas.SetActive(false);
                AssetsCanvas.SetActive(false);
                MembersCanvas.SetActive(true);
                Assets2Canvas.SetActive(false);
                break;
            case 3:
                HelpersCanvas.SetActive(false);
                AssetsCanvas.SetActive(false);
                MembersCanvas.SetActive(false);
                Assets2Canvas.SetActive(true);
                break;
        }
    }

    
    public void LeftButton()
    {
        if (canvasNumber != 0)
        {
            canvasNumber -= 1;
            Leftbutton.gameObject.SetActive(true);
        }
        else
        {
            canvasNumber = 3;
        }
    }

    public void RightButton()
    {
        if (canvasNumber != 3)
        {
            canvasNumber += 1;
            Rightbutton.gameObject.SetActive(true);
        }
        else
        {
            canvasNumber = 0;
        }
    }


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
}
