using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SkillButton : MonoBehaviour
{
    private LevelManager theLevelManager;

    public AudioSource audioSource;
    public AudioClip RheaHeals;

    public int healAmount = 200; // Amount of health to restore

    void Start()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    // This method is now public so it can be assigned in the Inspector
    public void UseHealSkill()
    {
        if (theLevelManager != null)
        {
            audioSource.PlayOneShot(RheaHeals);
            theLevelManager.Heal(healAmount);
        }
    }
}

