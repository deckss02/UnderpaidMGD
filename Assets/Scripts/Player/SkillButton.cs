using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    private LevelManager theLevelManager;
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
            theLevelManager.Heal(healAmount);
        }
    }
}

