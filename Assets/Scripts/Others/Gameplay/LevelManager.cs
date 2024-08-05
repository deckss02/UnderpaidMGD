using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public float waitToRespawn;
    public PlayerControllera thePlayer; // Reference to an object of PlayerController

    public GameObject animationPrefab;

    public AudioSource coinSound;
    public AudioSource levelMusic;
    public AudioSource gameOverMusic;
    public int maxHealth;
    public int healthCount;

    public PlayerSwap_Button swap_Button;
    public GameObject FollowingPlayer;
    public int CornHealth;
    public int RheaHealth;

    public bool CornDeath = false;
    public bool RheaDeath = false;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer playerSpriteRenderer;

    public GameObject gameOverScreen; // Referring to the Game Over Screen game object

    public Image heart1;
    public Image heart2;
    public Image heart3; // Reference to 3 heart images

    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty; // Store sprites images heartFull, heartHalf & heartEmpty

    //private bool respawning;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerControllera>();
        swap_Button = FindObjectOfType<PlayerSwap_Button>();
        healthCount = maxHealth;
        playerSpriteRenderer = thePlayer.GetComponent<SpriteRenderer>();

        // Initialize character health
        CornHealth = maxHealth;
        RheaHealth = maxHealth;
    }

    void Update()
    {
        if (swap_Button.isCorneliusActive && !CornDeath)
        {
            healthCount = CornHealth;
        }
        else if (!swap_Button.isCorneliusActive && !RheaDeath)
        {
            healthCount = RheaHealth;
        }

        if (healthCount <= 0)
        {
            if (swap_Button.isCorneliusActive)
            {
                CornDeath = true;
                swap_Button.ForceSwitchCharacter(); // Switch to Rhea if Cornelius dies
            }
            else
            {
                RheaDeath = true;
                swap_Button.ForceSwitchCharacter(); // Switch to Cornelius if Rhea dies
            }

            if (CornDeath && RheaDeath)
            {
                thePlayer.gameObject.SetActive(false);
                gameOverScreen.SetActive(true);
                levelMusic.Stop();
                gameOverMusic.Play();
            }
        }

        UpdateHeartMeter();
    }

    public void HurtPlayer(int damageToTake)
    {
        if (swap_Button.isCorneliusActive)
        {
            CornHealth -= damageToTake;
        }
        else
        {
            RheaHealth -= damageToTake;
        }

        healthCount = swap_Button.isCorneliusActive ? CornHealth : RheaHealth;
        UpdateHeartMeter();
        thePlayer.Knockback();
        thePlayer.hurtSound.Play();
        StartCoroutine(Invulnerability());
    }

    public void SwapHealth()
    {
        if (swap_Button.isCorneliusActive && !CornDeath)
        {
            healthCount = CornHealth;
        }
        else if (!swap_Button.isCorneliusActive && !RheaDeath)
        {
            healthCount = RheaHealth;
        }
    }

    public IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            playerSpriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            playerSpriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    //Update the heart meter
    public void UpdateHeartMeter()
    {
        switch (healthCount)
        {
            //When healthCount = 600, full healthCount
            case 600:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                break; //Keyword, jumps the code execution of the switch

            //Take away half of the heart when player gets hit once
            case 500:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartHalf;
                break;

            case 400:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;
                break;

            case 300:
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                heart3.sprite = heartEmpty;
                break;

            case 200:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break;

            case 100:
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break;

            case 0:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break;

            //Any other situations 
            default:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break;
        }
    }

    public void Heal(int healAmount)
    {
        // Determine which character has the lowest health
        if (CornHealth < RheaHealth)
        {
            // Heal Cornelius if his health is lower
            CornHealth += healAmount;
            if (CornHealth > maxHealth)
            {
                CornHealth = maxHealth;
            }
        }
        else
        {
            // Heal Rhea if her health is lower or if both are equal
            RheaHealth += healAmount;
            if (RheaHealth > maxHealth)
            {
                RheaHealth = maxHealth;
            }
        }

        // Update the health count to reflect the currently active character's health
        healthCount = swap_Button.isCorneliusActive ? CornHealth : RheaHealth;

        // Play the coin sound and update the heart meter
        coinSound.Play();
        UpdateHeartMeter();
        Debug.Log("Player healed. Current health: " + healthCount);

        // Instantiate the animation at the player's position
        InstantiateAnimationAtPlayerPosition();
    }

    private void InstantiateAnimationAtPlayerPosition()
    {
        if (animationPrefab != null && thePlayer != null)
        {
            Instantiate(animationPrefab, thePlayer.transform.position, Quaternion.identity);
        }
    }
}
