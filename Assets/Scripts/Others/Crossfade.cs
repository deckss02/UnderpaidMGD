using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crossfade : MonoBehaviour
{
    public Animator Transitionanim;
    public string Scenename;


    // Update is called once per frame
    void SceneClick()
    {
        
    }


    IEnumerator LoadScene()
    {
        Transitionanim.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(Scenename);  
    }
}
