using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Skip : MonoBehaviour
{
    //public GameObject dialogueBox;

    public PlayableDirector PlayableDirector;
    public float UpdateTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime -= Time.deltaTime;
    }

    public void SkipDialogue()
    {
        UpdateTime = 0;
        if (UpdateTime  <= 0)
        {
            SceneManager.LoadScene("MGD");
        }
        //dialogueBox.SetActive(false);
    }

}
