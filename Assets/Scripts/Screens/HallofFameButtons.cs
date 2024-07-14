using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HallofFameButtons : MonoBehaviour
{
    public GameObject HelpersCanvas;
    public GameObject MembersCanvas;

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

    public void MainMenu()
    {
        SceneManager.LoadScene("Town");
    }
}
