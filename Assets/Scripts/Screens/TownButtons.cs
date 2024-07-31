using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TownButtons : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator Transitionanim;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InnMove()
    {
        StartCoroutine(LoadingScene());
        SceneManager.LoadScene("Inn");
    }

    public void TheatreMove()
    {
        StartCoroutine(LoadingScene());
        SceneManager.LoadScene("Theatre");
    }

    public void GuildMove()
    {
        StartCoroutine(LoadingScene());
        SceneManager.LoadScene("Guild");
    }

    IEnumerator LoadingScene()
    {
        Transitionanim.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
    }
}
