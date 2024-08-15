using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    //public AudioSource MusicSource;

    [SerializeField] private GameObject SettingsPanel; // Reference to the instruction panel
    [SerializeField] private Button closeButton; // Reference to the close button
    [SerializeField] private Button SettingsButton; // Reference to the close button

    //public Slider MusicSlider;
    //public Slider SFXSlider;
    //
    //private float musicVolume = 1.0f;
    //private float SFXVolume = 1.0f;
    //
    //public Animator Transitionanim;
    // Start is called before the first frame update
    void Start()
    {

        //musicVolume = PlayerPrefs.GetFloat("volume");
        //SFXVolume = PlayerPrefs.GetFloat("SFX");
        //
        //MusicSource.volume = musicVolume;
        //MusicSlider.value = musicVolume;
        //
        //AudioListener.volume = SFXVolume;
        //SFXSlider.value = SFXVolume;

        // Add listener to the close button
        closeButton.onClick.AddListener(HideSettings);
    }

    void Update()
    {
        // Add listener to the close button
        SettingsButton.onClick.AddListener(ShowSettings);

        //MusicSource.volume = musicVolume;
        //
        //AudioListener.volume = SFXVolume;
        //
        //PlayerPrefs.SetFloat("Volume", musicVolume);
        //PlayerPrefs.SetFloat("SFX", SFXVolume);
    }

    //public void Updatevolume(float Volume)
    //{
    //    musicVolume = Volume;
    //}
    //
    //
    //public void UpdateSound(float SFX)
    //{
    //    SFXVolume = SFX;
    //}
    
    //public void Tutorial()
    //{
    //    StartCoroutine(LoadingTutorial());
    //}

    //IEnumerator LoadingTutorial()
    //{
    //    Transitionanim.SetTrigger("Start");
    //    yield return new WaitForSeconds(1.5f);
    //    SceneManager.LoadScene("Town");
    //}

    private void ShowSettings()
    {
        SettingsPanel.SetActive(true); // Hide the instruction panel
    }

    private void HideSettings()
    {
        SettingsPanel.SetActive(false); // Hide the instruction panel
    }
}
