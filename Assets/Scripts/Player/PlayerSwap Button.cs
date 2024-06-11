using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlayerSwapButton : MonoBehaviour
{
    public PlayerControllera controller;
    public GameObject Player; //Make a direct reference to the Player
    public GameObject FollowingPlayer;

    public LevelManager levelManager;

    public int CorneliusHealth;
    public int RheaHealth;

    public Sprite Corn; //Sprites of Player Characters
    public Sprite Rhe;
    public RuntimeAnimatorController p1Anim; //Switches the player's animation controller during gameplay.
    public RuntimeAnimatorController p2Anim;
    public RuntimeAnimatorController p1AnimF; //Switches the following characer's animator during gameplay.
    public RuntimeAnimatorController p2AnimF; 


    public int ChangeNumber = 0; //This integer details which Character is in use.

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<GameObject>();
        controller = GetComponent<PlayerControllera>();
        FollowingPlayer = GetComponent<GameObject>();
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

        //Switch Statement allows smooth back-&-forth swapping.
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

        FollowingPlayer.GetComponent<SpriteRenderer>().sprite = Rhe;
        FollowingPlayer.GetComponent<Animator>().runtimeAnimatorController = p2AnimF; ;
    }

    void Rhea()
    {
        Player.GetComponent<SpriteRenderer>().sprite = Rhe;
        Player.GetComponent<Animator>().runtimeAnimatorController = p2Anim;

        FollowingPlayer.GetComponent<SpriteRenderer>().sprite = Corn;
        FollowingPlayer.GetComponent<Animator>().runtimeAnimatorController = p1AnimF; ;
    }    
}
