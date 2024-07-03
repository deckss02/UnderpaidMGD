using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public float waitToRespawn;
    public PlayerController thePlayer; // Reference to the player's controller script
    public GameObject deathSplosion;
    public AudioSource coinSound;
    public AudioSource levelMusic;
    public AudioSource gameOverMusic;
    public int maxHealth;
    public int healthCount;

    private Animator playerAnimator; // Reference to the player's Animator component

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer playerSpriteRenderer;

    public int expCount; //Keep track of number of coins that the player collected
    public TextMeshProUGUI expText;
    public GameObject gameOverScreen; //Referring to the Game Over Screen game object

    public Image heart1;
    public Image heart2; //Make a reference to 2 heart images

    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty; //Store sprites images heartFull, heartHalf & heartEmpty

    private bool respawning;
    private float moveDirection; // Added declaration for moveDirection

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        expText.text = "Exp: " + expCount;
        healthCount = maxHealth;
        playerSpriteRenderer = thePlayer.GetComponent<SpriteRenderer>();
        playerAnimator = thePlayer.GetComponent<Animator>(); // Get the Animator component from the player
    }

    // Update is called once per frame
    void Update()
    {
        if (healthCount <= 0 && !respawning)
        {
            Respawn();
            respawning = true;
        }

    }

    public void HurtPlayer(int damageToTake)
    {
        healthCount -= damageToTake;
        UpdateHeartMeter(); // Update the heart meter when player respawns
        thePlayer.Knockback();
        thePlayer.hurtSound.Play();
        StartCoroutine(Invincibility());
    }

    private IEnumerator Invincibility()
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

    public void Respawn()
    {
        if (healthCount > 0)
        {
            StartCoroutine("RespawnCo");  // In the () is the string name of the Coroutine
        }
        else
        {
            thePlayer.gameObject.SetActive(false); // Deactivate the player in the world
            Instantiate(deathSplosion, thePlayer.transform.position, thePlayer.transform.rotation);
            gameOverScreen.SetActive(true);
            levelMusic.Stop();
            gameOverMusic.Play();
        }
    }

    // Update the heart meter
    public void UpdateHeartMeter()
    {
        switch (healthCount)
        {
            case 400:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                break;

            case 300:
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                break;

            case 200:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                break;

            case 100:
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                break;

            case 0:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                break;

            default:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                break;
        }
    }

    public void Heal(int healAmount)
    {
        healthCount += healAmount;
        if (healthCount > maxHealth)
        {
            healthCount = maxHealth;
        }
        coinSound.Play();
        UpdateHeartMeter();
        PlayHealingAnimation();
        Debug.Log("Player healed. Current health: " + healAmount);
    }

    private void PlayHealingAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsHealing", true);
        }
    }

    public void AddExp(int ExpToAdd)
    {
        expCount += ExpToAdd;
        expText.text = "Exp: " + expCount; // When coin is collected, update the expCount value and display in the text UI
        coinSound.Play();
    }

    public IEnumerator RespawnCo()
    {
        thePlayer.gameObject.SetActive(false); // Deactivate the player in the world
        Instantiate(deathSplosion, thePlayer.transform.position, thePlayer.transform.rotation); // Create Object
        yield return new WaitForSeconds(waitToRespawn); // How many seconds we want the game to wait for
        respawning = false;
        thePlayer.transform.position = thePlayer.respawnPosition; // Move the player to respawn position
        thePlayer.gameObject.SetActive(true); // Reactivate the player in the world
    }

    // Method to move the player
    public void Move(float dir)
    {
        if (dir > 0)
        {
            thePlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(thePlayer.moveSpeed, thePlayer.GetComponent<Rigidbody2D>().velocity.y);
            thePlayer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (dir < 0)
        {
            thePlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(-thePlayer.moveSpeed, thePlayer.GetComponent<Rigidbody2D>().velocity.y);
            thePlayer.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            thePlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, thePlayer.GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    // Method to set the move direction from external inputs
    public void SetMoveDirection(float direction)
    {
        moveDirection = direction;
    }
}