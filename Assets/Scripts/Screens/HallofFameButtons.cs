using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HallofFameButtons : MonoBehaviour
{
    public int canvasNumber;

    public GameObject HelpersCanvas;
    public GameObject MembersCanvas;
    public GameObject AssetsCanvas;
    public GameObject Assets2Canvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    public void LeftButton()
    {
        if (MembersCanvas)
    }

    public void RighttButton()
    {
        if (MembersCanvas)
    }

    */

    public void Shuffle()
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


    public void MainMenu()
    {
        SceneManager.LoadScene("Town");
    }
}
