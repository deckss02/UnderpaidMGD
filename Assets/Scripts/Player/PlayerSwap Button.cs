using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlayerSwapButton : MonoBehaviour
{
    public PlayerController controller;
    public GameObject Player; //Make a direct reference to the Player

    public LevelManager levelManager;

    public int CorneliusHealth;
    public int RheaHealth;

    public Sprite Corn; //Sprites of Player Characters
    public Sprite Rhe;
    public RuntimeAnimatorController p1Anim; //Switches the player's animation controller during gameplay.
    public RuntimeAnimatorController p2Anim;

    public int ChangeNumber = 0; //This integer details which Character is in use.

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        Player = GetComponent<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCharacter()
    {
        ChangeNumber = ChangeNumber + 1;

        if (ChangeNumber > 1)
        {
            ChangeNumber = 0;
        }

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
        Player.GetComponent<SpriteRenderer>().sprite = Corn;
        Player.GetComponent<Animator>().runtimeAnimatorController = p1Anim;
    }

    void Rhea()
    {
        Player.GetComponent<SpriteRenderer>().sprite = Rhe;
        Player.GetComponent<Animator>().runtimeAnimatorController = p2Anim;
    }    
}
