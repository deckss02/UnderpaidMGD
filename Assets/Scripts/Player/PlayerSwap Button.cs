using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSwap_Button : MonoBehaviour
{
    public PlayerControllera controller;
    public GameObject Player; // Reference to the Player
    public GameObject FollowingPlayer;

    public Image CP; // Reference to the UI Image component
    public Sprite firstSprite; // Reference to the first sprite
    public Sprite secondSprite; // Reference to the second sprite

    public LevelManager levelManager;

    public int CorneliusHealth;
    public int RheaHealth;

    public Sprite Corn; // Sprites of Player Characters
    public Sprite Rhe;
    public RuntimeAnimatorController p1Anim; // Switches the player's animation controller during gameplay.
    public RuntimeAnimatorController p2Anim;
    public RuntimeAnimatorController p1AnimF; // Switches the following character's animator during gameplay.
    public RuntimeAnimatorController p2AnimF;

    public int ChangeNumber = 0; // This integer details which Character is in use.

    void Start()
    {
        controller = FindObjectOfType<PlayerControllera>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        // This method can be called from a UI button
    }

    public void SwitchCharacter()
    {
        ChangeNumber = ChangeNumber + 1;

        if (ChangeNumber > 1)
        {
            ChangeNumber = 0;
        }

        // Switch Statement allows smooth back-&-forth swapping.
        switch (ChangeNumber)
        {
            case 0:
                Cornelius();
                break;
            case 1:
                Rhea();
                break;
        }

        levelManager.SwapHealth(); // Sync health after switching characters
    }

    void Cornelius()
    {
        Player.GetComponent<SpriteRenderer>().sprite = Corn;
        CP.sprite = firstSprite;
        Player.GetComponent<Animator>().runtimeAnimatorController = p1Anim;

        FollowingPlayer.GetComponent<SpriteRenderer>().sprite = Rhe;
        FollowingPlayer.GetComponent<Animator>().runtimeAnimatorController = p2AnimF;
    }

    void Rhea()
    {
        Player.GetComponent<SpriteRenderer>().sprite = Rhe;
        CP.sprite = secondSprite;
        Player.GetComponent<Animator>().runtimeAnimatorController = p2Anim;

        FollowingPlayer.GetComponent<SpriteRenderer>().sprite = Corn;
        FollowingPlayer.GetComponent<Animator>().runtimeAnimatorController = p1AnimF;
    }
}
