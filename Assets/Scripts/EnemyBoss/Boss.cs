using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject hairballPrefab;
    public Transform[] firePoints;
    public int minHairballs = 3;
    public int maxHairballs = 4;
    public float minDelayBetweenShots = 0.1f;
    public float maxDelayBetweenShots = 1.0f;

    public GameObject ratPrefab; // Reference to the rat prefab
    public Transform[] spawnPoints; // References to the spawn points
    public int numberOfRats = 5; // Number of rats to spawn
    public PlayerControllera playerController; // Reference to the player's controller script

    private int summonsLeftToKill = 0;
    private bool died = false; // Flag to track if all summons are dead

    // Attack stage names
    private string[] attackStages = { "PawAttack", "HairballAttack", "ClawAttack", "SummonMinions" };
    private int currentAttackStage = 0; // Current attack stage index

    private Animator myAnim;
    public bool attacking;

    public PawEye pawEye;
    public PawMouth pawMouth;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        attacking = false;
    }

    private void Update()
    {

    }

    public void StartHairBallRollAttack(System.Action onComplete)
    {
        attacking = true;
        StartCoroutine(HairBallRollAttackCoroutine(onComplete));
    }

    public void StartClawAttack(System.Action onComplete)
    {
        attacking = true;
        StartCoroutine(ClawAttackCoroutine(onComplete));
    }

    public void StartSlamingAttack(System.Action onComplete)
    {
        attacking = true;
        StartCoroutine(SlamingAttackCoroutine(onComplete));
    }

    public void StartSummoningAttack(System.Action onComplete)
    {
        attacking = true;
        StartCoroutine(SummoningAttackCoroutine(onComplete));
    }

    // Method to freeze or unfreeze the player
    private void FreezePlayer(bool freeze)
    {
        if (playerController != null)
        {
            playerController.canMove = !freeze;
        }
    }

    private IEnumerator HairBallRollAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting HairBallRoll attack");

        // Randomize the number of hairballs
        int numberOfHairballs = Random.Range(minHairballs, maxHairballs);
        List<Transform> usedFirePoints = new List<Transform>();

        for (int i = 0; i < numberOfHairballs; i++)
        {
            // Select a random fire point that hasn't been used yet
            Transform firePoint;
            do
            {
                firePoint = firePoints[Random.Range(0, firePoints.Length)];
            } while (usedFirePoints.Contains(firePoint));

            // Add the selected fire point to the used list
            usedFirePoints.Add(firePoint);

            // Instantiate the hairball at the selected fire point
            Instantiate(hairballPrefab, firePoint.position, firePoint.rotation);

            // Randomize the delay between shots
            float delayBetweenShots = Random.Range(minDelayBetweenShots, maxDelayBetweenShots);
            yield return new WaitForSeconds(delayBetweenShots);
        }

        Debug.Log("HairBallRoll attack completed");

        // Call the callback to signal completion
        onComplete?.Invoke();
        attacking = false;
    }

    private IEnumerator ClawAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Claw attack");

        pawMouth.ActivatePaw();

        // Simulate the Claw attack duration
        yield return new WaitForSeconds(20.0f);

        Debug.Log("Claw attack completed");

        pawMouth.DeactivatePaw();
        // Call the callback to signal completion
        onComplete?.Invoke();
        attacking = false;
    }

    private IEnumerator SlamingAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Paw Slamming attack");
        pawMouth.ActivatePaw();
        pawEye.ActivatePaw();

        // Simulate the Paw Slamming attack duration
        yield return new WaitForSeconds(20.0f);

        Debug.Log("Paw Slamming attack completed");

        pawEye.DeactivatePaw();
        pawMouth.DeactivatePaw();
        // Call the callback to signal completion
        onComplete?.Invoke();
        attacking = false;
    }

    private IEnumerator SummoningAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Summoning attack");

        // Freeze the player controls
        FreezePlayer(true);

        // Initialize the number of rats to be killed
        summonsLeftToKill = numberOfRats;
        Debug.Log("Number of rats to summon: " + numberOfRats);

        for (int i = 0; i < numberOfRats; i++)
        {
            // Select a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Debug.Log("Spawning rat at: " + spawnPoint.position);

            // Instantiate the rat at the selected spawn point
            GameObject ratInstance = Instantiate(ratPrefab, spawnPoint.position, spawnPoint.rotation);

            // Attach the SummonKilled method to the rat's death event
            RatController ratController = ratInstance.GetComponent<RatController>();
            if (ratController != null)
            {
                ratController.OnDeath += SummonKilled;
                Debug.Log("Attached SummonKilled to rat instance.");
            }

            // Add a slight delay between each spawn
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("Summoning attack completed");

        // Unfreeze the player controls
        FreezePlayer(false);

        // If no summons were created, call onComplete immediately
        if (summonsLeftToKill == 0)
        {
            died = true;
            onComplete?.Invoke();
        }
    }

    private void SummonKilled()
    {
        summonsLeftToKill--;
        Debug.Log("Summon killed, remaining: " + summonsLeftToKill);
        if (summonsLeftToKill <= 0)
        {
            Debug.Log("All summoned minions have been killed.");
            died = true;
            myAnim.SetBool("CoolDown", true);
        }
    }

    // Check if the boss is ready for the next attack
    public bool IsReadyForNextAttack()
    {
        // Check if CooldownBehaviour is in cooldown state
        return !myAnim.GetCurrentAnimatorStateInfo(0).IsName("CoolDown");
    }

    // Progress to the next attack stage
    private void ProgressToNextAttackStage()
    {
        currentAttackStage = (currentAttackStage + 1) % attackStages.Length;
        string nextAttack = attackStages[currentAttackStage];
        myAnim.SetTrigger(nextAttack);
    }

    public bool HasDied()
    {
        return died;
    }
}

