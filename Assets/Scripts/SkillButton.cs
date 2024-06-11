using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Button skillButton; // Reference to the skill button
    private LevelManager theLevelManager;
    public int healAmount = 200; // Amount of health to restore

    void Start()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
        // Ensure the skill button is assigned
        if (skillButton != null)
        {
            skillButton.onClick.AddListener(UseHealSkill);
        }
        else
        {
            Debug.LogError("Skill button not assigned in the inspector.");
        }

    }

    void UseHealSkill()
    {
        if (theLevelManager != null)
        {
            theLevelManager.Heal(healAmount);
        }
    }
}
