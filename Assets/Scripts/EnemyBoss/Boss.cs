using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject hairballPrefab; // Prefab for the hairball projectile
    public Transform[] firePoints; // Points from which hairballs will be fired

    public GameObject ratPrefab; // Prefab for the rat enemy
    public Transform[] spawnPoints; // Points where rats will be spawned
    public int numberOfRats = 6; // Number of rats to spawn
    private List<GameObject> activeRats = new List<GameObject>();
    public PlayerController playerController; // Reference to the player's controller script

    private bool died = false; // Flag to track if all summons are dead

    // Array of attack stage names
    private string[] attackStages = { "PawAttack", "HairballAttack", "ClawAttack", "SummonMinions" };
    private int currentAttackStage = 0; // Current attack stage index

    private Animator myAnim; // Animator component
    public bool attacking; // Flag to indicate if the boss is currently attacking

    public PawEye pawEye; // Reference to the PawEye component
    public GameObject Claw;
    public PawMouth pawMouth; // Reference to the PawMouth component
    public GameObject pawEyePrefab; // Prefab for the pawEye GameObject
    public GameObject SavePaw; // Prefab for safe lane
    public float fadeDuration = 1.0f; // Duration for the pawEye to fade away
    private Vector3 pawStopPosition; // Variable to store the position where the paw stops

    private void Start()
    {
        myAnim = GetComponent<Animator>(); // Get the Animator component
        attacking = false; // Initialize attacking flag to false
    }

    private void Update()
    {
        // Handle updates (if needed)
    }

    // Method to start the HairBallRoll attack
    public void StartHairBallRollAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(HairBallRollAttackCoroutine(onComplete)); // Start the HairBallRoll attack coroutine
    }

    // Method to start the Claw attack
    public void StartClawAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(ClawAttackCoroutine(onComplete)); // Start the Claw attack coroutine
    }

    // Method to start the Slamming attack
    public void StartSlammingAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(SlammingAttackCoroutine(onComplete)); // Start the Slamming attack coroutine
    }

    // Method to start the Summoning attack
    public void StartSummoningAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(SummoningAttackCoroutine(onComplete)); // Start the Summoning attack coroutine
    }

    // Method to freeze or unfreeze the player
    private void FreezePlayer(bool freeze)
    {
        if (playerController != null)
        {
            playerController.canMove = !freeze; // Set the player's canMove property
        }
    }

    // Coroutine for the HairBallRoll attack
    private IEnumerator HairBallRollAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting HairBallRoll attack");
        pawEye.gameObject.SetActive(false);

        // Fixed number of hairballs to spawn
        int[] hairballOptions = { 3, 4 };
        int numberOfHairballs = hairballOptions[Random.Range(0, hairballOptions.Length)];
        List<GameObject> pawEyeInstances = new List<GameObject>();
        List<GameObject> savePawInstances = new List<GameObject>();

        // Iterate through fire points
        foreach (Transform firePoint in firePoints)
        {
            // Determine if this fire point should spawn a hairball
            bool spawnHairball = Random.value < 0.5f; // Example condition, adjust as needed

            if (spawnHairball)
            {
                // Instantiate PawEye prefab
                GameObject pawEyeInstance = Instantiate(pawEyePrefab, firePoint.position, firePoint.rotation);
                pawEyeInstances.Add(pawEyeInstance);

                // Spawn hairball at the selected fire point
                yield return new WaitForSeconds(0.3f);
                Instantiate(hairballPrefab, firePoint.position, firePoint.rotation);
            }
            else
            {
                // Instantiate SavePaw prefab at the fire point
                GameObject savePawInstance = Instantiate(SavePaw, firePoint.position, firePoint.rotation);
                savePawInstances.Add(savePawInstance);
            }
        }

        // Schedule deactivation of pawEyes and savePaws after the fade duration
        foreach (GameObject pawEyeInstance in pawEyeInstances)
        {
            StartCoroutine(FadeAwayPawEye(pawEyeInstance));
        }

        foreach (GameObject savePawInstance in savePawInstances)
        {
            StartCoroutine(FadeAwaySavePaw(savePawInstance));
        }

        // Reactivate PawEye at the end of the attack
        pawEye.gameObject.SetActive(true);

        Debug.Log("HairBallRoll attack completed");

        // Call the callback to signal completion
        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
    }


    // Coroutine to fade away the spawned PawEye
    private IEnumerator FadeAwayPawEye(GameObject pawEyeInstance)
    {
        yield return new WaitForSeconds(fadeDuration); // Wait for fade duration
        Destroy(pawEyeInstance); // Destroy the PawEye instance
    }

    // Coroutine to fade away the spawned SavePaw
    private IEnumerator FadeAwaySavePaw(GameObject savePawInstance)
    {
        yield return new WaitForSeconds(fadeDuration); // Wait for fade duration
        Destroy(savePawInstance); // Destroy the SavePaw instance
    }

    // Coroutine for the Claw attack
    private IEnumerator ClawAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Claw attack");

        // Variables for the attack logic
        float followDuration = 5.0f; // Duration to follow the player before spawning claws
        float clawSpawnInterval = 7.0f; // Interval between spawning claws
        int maxClawSpawns = 3; // Maximum number of claws to spawn before cooldown
        int clawSpawnCount = 3; // Counter for claws spawned
        float attackTimer = 10.0f; // Timer for tracking attack duration

        // Loop until attack conditions are met
        while (clawSpawnCount < maxClawSpawns)
        {
            // Move PawEye towards the player
            pawEye.ActivatePaw();
            yield return new WaitForSeconds(followDuration / 2); // Follow for half the duration
            pawStopPosition = pawEye.transform.position; // Store the position where PawEye stopped

            // Move PawMouth towards the player
            pawMouth.ActivatePaw();
            yield return new WaitForSeconds(followDuration / 2); // Follow for the remaining half duration
            pawStopPosition = pawMouth.transform.position; // Store the position where PawMouth stopped

            // Check if attack duration has reached the claw spawn interval
            attackTimer += followDuration;
            if (attackTimer >= clawSpawnInterval)
            {
                // Spawn a claw at the stored position
                SpawnClaw(pawStopPosition);

                // Reset the attack timer
                attackTimer = 0.0f;
                clawSpawnCount++;
            }
        }

        Debug.Log("Claw attack completed");

        // Deactivate PawEye and PawMouth
        pawEye.DeactivatePaw();
        pawMouth.DeactivatePaw();

        // Call the callback to signal completion
        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
    }

    // Method to spawn a claw at a specific position
    private void SpawnClaw(Vector3 spawnPosition)
    {
        Debug.Log("Spawning Claw at: " + spawnPosition);
        // Instantiate your claw prefab or perform any claw attack logic here
        Instantiate(Claw, spawnPosition, Quaternion.identity);
    }

    // Coroutine for the Slamming attack
    private IEnumerator SlammingAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Paw Slamming attack");
        pawMouth.ActivatePaw(); // Activate PawMouth
        pawEye.ActivatePaw(); // Activate PawEye

        // Simulate the Paw Slamming attack duration
        yield return new WaitForSeconds(20.0f);

        Debug.Log("Paw Slamming attack completed");

        pawEye.DeactivatePaw(); // Deactivate PawEye
        pawMouth.DeactivatePaw(); // Deactivate PawMouth
        // Call the callback to signal completion
        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
    }

    // Coroutine for the Summoning attack
    private IEnumerator SummoningAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Summoning attack");

        // Initialize the number of rats to be killed
        activeRats.Clear();

        // Loop to summon rats and paws at the same spawn points
        for (int i = 0; i < numberOfRats; i++)
        {
            // Select a spawn point from the spawnPoints array
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            Debug.Log("Spawning rat at: " + spawnPoint.position);

            // Instantiate a paw at the boss's position
            GameObject pawInstance = Instantiate(pawEyePrefab, transform.position, Quaternion.identity);

            // Calculate duration based on distance and speed
            float distance = Vector3.Distance(transform.position, spawnPoint.position);
            float speed = 5.0f; // Adjust speed as needed
            float duration = distance / speed;

            // Move the paw towards the spawn point
            yield return StartCoroutine(MovePawTowardsTarget(pawInstance, spawnPoint.position, duration));

            // Instantiate the rat at the selected spawn point
            GameObject ratInstance = Instantiate(ratPrefab, spawnPoint.position, spawnPoint.rotation);
            activeRats.Add(ratInstance);

            // Add a slight delay between each spawn
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("Summoning attack completed");

        // Start checking for remaining rats
        StartCoroutine(CheckRemainingRats(onComplete));
    }

    // Method to move the paw towards the target
    private IEnumerator MovePawTowardsTarget(GameObject paw, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = paw.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            paw.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        paw.transform.position = targetPosition;
    }

    // Coroutine to check for remaining rats
    private IEnumerator CheckRemainingRats(System.Action onComplete)
    {
        while (activeRats.Count > 0)
        {
            activeRats.RemoveAll(rat => rat == null); // Remove destroyed rats from the list
            yield return new WaitForSeconds(0.5f);
        }

        // All rats are destroyed
        Debug.Log("All summoned minions have been killed.");
        onComplete?.Invoke();
        attacking = false;// Invoke the onComplete callback
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
        currentAttackStage = (currentAttackStage + 1) % attackStages.Length; // Move to the next attack stage
        string nextAttack = attackStages[currentAttackStage];
        myAnim.SetTrigger(nextAttack); // Trigger the next attack animation
    }

    // Check if the boss has died
    public bool HasDied
    {
        get { return died; }
    }
}