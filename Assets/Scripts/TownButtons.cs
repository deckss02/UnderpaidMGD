using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InnMove()
    {
        SceneManager.LoadScene("Inn");
    }

    public void TheatreMove()
    {
        SceneManager.LoadScene("Theatre");

    }

    public void GuildMove()
    {
        SceneManager.LoadScene("Guild");
    }
}
