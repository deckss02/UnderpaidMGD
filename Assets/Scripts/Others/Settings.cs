using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public AudioSource MusicSource;

    public Slider MusicSlider;
    public Slider SFXSlider;

    private float musicVolume = 1.0f;
    private float SFXVolume = 1.0f;

    public Animator Transitionanim;
    // Start is called before the first frame update
    void Start()
    {

        musicVolume = PlayerPrefs.GetFloat("volume");
        SFXVolume = PlayerPrefs.GetFloat("Svolume");

        MusicSource.volume = musicVolume;
        MusicSlider.value = musicVolume;

        AudioListener.volume = SFXVolume;
        SFXSlider.value = SFXVolume;
    }

    void Update()
    {
        MusicSource.volume = musicVolume;

        AudioListener.volume = SFXVolume;

        PlayerPrefs.SetFloat("volume", musicVolume);
        PlayerPrefs.SetFloat("Svolume", SFXVolume);
    }

    public void Updatevolume(float Volume)
    {
        musicVolume = Volume;
    }

    
    public void UpdateSound(float SVolume)
    {
        SFXVolume = SVolume;
    }
    
    public void Tutorial()
    {
        StartCoroutine(LoadingTutorial());
    }

    IEnumerator LoadingTutorial()
    {
        Transitionanim.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Town");
    }
}
