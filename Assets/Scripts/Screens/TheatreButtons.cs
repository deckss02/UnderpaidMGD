using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheatreButtons : MonoBehaviour
{
    public GameObject MainCanvas;

    public GameObject PerformanceCanvas;
    public GameObject ExihibitionCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PerformanceAccess()
    {
        MainCanvas.SetActive(false);
        PerformanceCanvas.SetActive(true);
    }

    public void ExhibitionAccess()
    {
        MainCanvas.SetActive(false);
        ExihibitionCanvas.SetActive(true);
    }

    public void VIPAccess()
    {
        SceneManager.LoadScene("HallofFame");
    }

    public void Back()
    {
        MainCanvas.SetActive(true);
        PerformanceCanvas.SetActive(false);
        ExihibitionCanvas.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Town");
    }
}
