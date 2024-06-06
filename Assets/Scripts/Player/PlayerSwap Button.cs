using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlayerSwapButton : MonoBehaviour
{
    public PlayerController controller;
    public GameObject Player;

    public LevelManager levelManager;

    public int CorneliusHealth;
    public int RheaHealth;
    public int ChangeNumber;

    public SpriteRenderer SR;
    public Sprite Corn;
    public Sprite Rhe;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        Player = GetComponent<GameObject>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (ChangeNumber)
        {
            case 0:
                Cornelius();
                break;
            case 1:
                Rhea();
                break;
        }
    }

    void Cornelius()
    {
       levelManager.healthCount += CorneliusHealth;
        SR.sprite = Corn;
    }

    void Rhea()
    {
        levelManager.healthCount += RheaHealth;
        SR.sprite = Rhe;

    }    
}
