using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    public GameObject IntroPopUp;
    private int popUpIndex;
    public float waitTime = 2f;

    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;

    public GameObject Wolf;
    private bool movedUp = false;
    private bool movedDown = false;
    private bool movedLeft = false;
    private bool movedRight = false;

    public Button Rhea;
    public Button Corn;
    public Button Swap;
    public Button Ultimate;
    public PlayerControllera player;

    public LevelManager levelManager; // Reference to LevelManager
    public GameObject boss;

    public GameObject BossObject; // Reference to the Boss GameObject
    public GameObject Spawners; // Reference to Spawners
    public EC Ec; // Reference to the EnemyController script
    public GameObject White; // Reference to the White GameObject

    private bool isIntroPopUpDone = false; // Flag to check if IntroPopUp is done
    private bool isCornSkillPressed = false; // Flag to check if Corn's skill has been pressed
    private bool isSwapButtonPressed = false; // Flag to check if Swap button has been interacted with
    private bool hasAppliedDamage = false; // Flag to ensure damage is only applied once
    private Vector3 wolfOriginalPosition; // Original position of Wolf

    void Start()
    {
        // Assign button click events to corresponding methods
        upButton.onClick.AddListener(MoveUp);
        downButton.onClick.AddListener(MoveDown);
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);
        player.jumpSpeed = 0f;

        // Assign Corn and Swap button click events
        Corn.onClick.AddListener(OnCornSkillPressed);
        Swap.onClick.AddListener(OnSwapButtonPressed);

        // Set the original position for Wolf
        wolfOriginalPosition = new Vector3(-0.28f, 1.753f, 0f); // Set to your desired position

        // Start the IntroPopUp coroutine
        StartCoroutine(ShowIntroPopUp());
    }

    // Coroutine to show IntroPopUp, wait, and then hide it
    IEnumerator ShowIntroPopUp()
    {
        IntroPopUp.SetActive(true); // Show the IntroPopUp
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        IntroPopUp.SetActive(false); // Hide the IntroPopUp

        isIntroPopUpDone = true; // Set flag to true when IntroPopUp is done
    }

    // Update is called once per frame
    void Update()
    {
        if (isIntroPopUpDone) // Only update popUps if IntroPopUp is done
        {
            for (int i = 0; i < popUps.Length; i++)
            {
                popUps[i].SetActive(i == popUpIndex);
            }

            if (popUpIndex == 0)
            {
                if (movedLeft && movedRight)
                {
                    popUpIndex++;
                    ResetMovement(); // Reset movement flags after tutorial step is completed
                }
            }
            else if (popUpIndex == 1)
            {
                player.jumpSpeed = 10.7f;
                if (movedUp && movedDown)
                {
                    popUpIndex++;
                    ResetMovement();
                }
            }
            else if (popUpIndex == 2)
            {
                // Spawn Wolf and wait for it to die
                if (Wolf != null && !Wolf.activeInHierarchy)
                {
                    Instantiate(Wolf, wolfOriginalPosition, Quaternion.identity); // Spawn at original position
                    Wolf.SetActive(true);
                }

                if (Wolf != null && !Wolf.activeInHierarchy) // Assuming Wolf is deactivated upon death
                {
                    popUpIndex++;
                }
            }
            else if (popUpIndex == 3)
            {
                // Check if Swap button is pressed
                if (isSwapButtonPressed)
                {
                    Debug.Log("Step 3 complete: Swap button has been pressed");
                    popUpIndex++;
                }
            }
            if (popUpIndex == 4)
            {
                // Call the function to apply damage if it hasn't been applied yet
                if (!hasAppliedDamage)
                {
                    ApplyDamageToPlayer(200);
                    Debug.Log("Step 4: Checking health status of Corn and Rhea");
                }

                // Check if Corn and Rhea's health is at full
                if (levelManager != null &&
                    levelManager.CornHealth == levelManager.maxHealth &&
                    levelManager.RheaHealth == levelManager.maxHealth)
                {
                    Debug.Log("Step 4 complete: Both Corn and Rhea are at full health");
                    popUpIndex++;
                }
            }
            else if (popUpIndex == 5)
            {
                // Check if Corn's Skill has been pressed
                if (isCornSkillPressed)
                {
                    popUpIndex++;
                }
            }
            else if (popUpIndex == 6)
            {
                Starting();
                // Activate EC script, BossObject, and Spawners
                if (Ec != null && BossObject != null && Spawners != null)
                {
                    Ec.enabled = true; // Activate EnemyController script
                    BossObject.SetActive(true); // Activate BossObject
                    Spawners.SetActive(true); // Activate Spawners
                    // Check if all enemies are defeated
                    StartCoroutine(CheckEnemiesLeft());
                }
            }
        }
    }

    private void ApplyDamageToPlayer(int damage)
    {
        if (levelManager != null)
        {
            // Apply damage to the active player
            levelManager.HurtPlayer(damage);

            // Set the flag to true to prevent further damage application in this phase
            hasAppliedDamage = true;
        }
    }

    IEnumerator Starting()
    {
        yield return new WaitForSeconds(4f);
    }

    // Method called when Corn's skill button is pressed
    void OnCornSkillPressed()
    {
        isCornSkillPressed = true;
    }

    // Method called when Swap button is pressed
    void OnSwapButtonPressed()
    {
        isSwapButtonPressed = true;
    }

    // Coroutine to check if all enemies are defeated
    IEnumerator CheckEnemiesLeft()
    {
        // Wait for a moment to allow enemies to spawn
        yield return new WaitForSeconds(1f);

        while (Ec.enemiesLeftToKill <= 0)
        {
            yield return new WaitForSeconds(0.4f);
        }
        // Proceed to next popUpIndex once all enemies are defeated
        popUpIndex++;
    }

    void MoveUp()
    {
        movedUp = true;
    }

    void MoveDown()
    {
        movedDown = true;
    }

    void MoveLeft()
    {
        movedLeft = true;
    }

    void MoveRight()
    {
        movedRight = true;
    }

    void ResetMovement()
    {
        movedUp = false;
        movedDown = false;
        movedLeft = false;
        movedRight = false;
    }
}
