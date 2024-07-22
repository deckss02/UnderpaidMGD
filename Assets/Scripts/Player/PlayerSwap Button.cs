using System.Collections;
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

    public GameObject CSkill; // Reference to the first skill GameObject
    public GameObject RSkill; // Reference to the second skill GameObject

    public Sprite RheaBox;
    public Sprite CornBox;

    public LevelManager levelManager;

    public Sprite Corn; // Sprites of Player Characters
    public Sprite Rhe;
    public RuntimeAnimatorController p1Anim; // Switches the player's animation controller during gameplay.
    public RuntimeAnimatorController p2Anim;

    public bool isCorneliusActive = true; // Track the currently active character
    public float switchCooldown = 1f; // Cooldown period in seconds
    private float nextSwitchTime = 0f; // Time when switching is allowed again

    private SwordUser sworduser; // Reference to the SwordUser component for the main player

    private bool forceSwitched = false; // Flag to disable swapping after forced switch

    void Start()
    {
        controller = FindObjectOfType<PlayerControllera>();
        levelManager = FindObjectOfType<LevelManager>();
        sworduser = Player.GetComponent<SwordUser>();
        CSkill.SetActive(true);
    }

    public void SwitchCharacter()
    {
        // Check if switching is allowed and enough time has passed to switch
        if (Time.time >= nextSwitchTime && !forceSwitched)
        {
            // Check if the active character is dead and prevent switching to a dead character
            if ((isCorneliusActive && levelManager.CornDeath) || (!isCorneliusActive && levelManager.RheaDeath))
            {
                return;
            }

            // Update the nextSwitchTime based on the cooldown period
            nextSwitchTime = Time.time + switchCooldown;

            // Perform the switch based on the current state
            if (isCorneliusActive)
            {
                SwitchToRhea();
                SwitchToRheaBox();
            }
            else
            {
                SwitchToCornelius();
                SwitchToCornBox();
            }

            levelManager.SwapHealth(); // Sync health after switching character

            // Equip the correct weapon based on the active character
            if (isCorneliusActive)
            {
                sworduser.SwitchToSword();
            }
            else
            {
                sworduser.SwitchToClaymore();
            }

            // Toggle the active character state
            isCorneliusActive = !isCorneliusActive;
        }
    }

    void SwitchToCornBox()
    {
        FollowingPlayer.GetComponent<SpriteRenderer>().sprite = CornBox;
    }

    void SwitchToRheaBox()
    {
        FollowingPlayer.GetComponent<SpriteRenderer>().sprite = RheaBox;
    }

    void SwitchToCornelius()
    {
        Player.GetComponent<SpriteRenderer>().sprite = Corn;
        CP.sprite = firstSprite;
        Player.GetComponent<Animator>().runtimeAnimatorController = p1Anim;
        CSkill.SetActive(true);
        RSkill.SetActive(false);
    }

    void SwitchToRhea()
    {
        Player.GetComponent<SpriteRenderer>().sprite = Rhe;
        CP.sprite = secondSprite;
        Player.GetComponent<Animator>().runtimeAnimatorController = p2Anim;
        CSkill.SetActive(false);
        RSkill.SetActive(true);
    }

    public void ForceSwitchCharacter()
    {
        // Perform the switch based on the current state
        if (isCorneliusActive && levelManager.CornDeath)
        {
            SwitchToRhea();
            SwitchToRheaBox();
            isCorneliusActive = false;
        }
        else if (!isCorneliusActive && levelManager.RheaDeath)
        {
            SwitchToCornelius();
            SwitchToCornBox();
            isCorneliusActive = true;
        }

        // Set the flag to disable further swapping
        forceSwitched = true;
    }
}

