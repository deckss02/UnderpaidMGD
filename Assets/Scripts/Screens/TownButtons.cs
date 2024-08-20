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
        StartCoroutine(LoadingInn());

    }

    public void TheatreMove()
    {
        StartCoroutine(LoadingTheatre());
    }

    public void GuildMove()
    {
        StartCoroutine(LoadingGuild());
    }

    IEnumerator LoadingInn()
    {
        Transitionanim.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Inn");
    }

    IEnumerator LoadingTheatre()
    {
        Transitionanim.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("HallofFame");
    }

    IEnumerator LoadingGuild()
    {
        Transitionanim.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Guild");
    }
}
