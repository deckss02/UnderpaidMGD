using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuildButtons : MonoBehaviour
{
    public int LevelNumber;
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
                SceneManager.LoadScene("MGD");
                break;
        }
    }

}
