using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject hairballPrefab; // Prefab for the hairball projectile
    public Transform[] firePoints; // Points from which hairballs will be fired

    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform spawnPoint4;
    public GameObject clawBulletPrefab; // New prefab for the claw bullet

    public GameObject ratPrefab; // Prefab for the rat enemy
    public Transform[] spawnPoints; // Points where rats will be spawned
    public int numberOfRats = 6; // Number of rats to spawn
    public PlayerControllera playerController; // Reference to the player's controller script
    private bool died = false;

    // Array of attack stage names
    private string[] attackStages = { "PawAttack", "HairballAttack", "ClawAttack", "SummonMinions" };
    private int currentAttackStage = 0; // Current attack stage index

    public Animator myAnim; // Animator component
    public bool attacking; // Flag to indicate if the boss is currently attacking

    public PawEye pawEye; // Reference to the PawEye component
    public GameObject Claw;
    public PawMouth pawMouth; // Reference to the PawMouth component
    public GameObject pawEyePrefab; // Prefab for the pawEye GameObject
    public GameObject SavePaw; // Prefab for safe lane
    public float fadeDuration = 1.0f; // Duration for the pawEye to fade away
    private Vector3 pawStopPosition; // Variable to store the position where the paw stops

    public int activeRatsCount;

    private void Start()
    {
        myAnim = GetComponent<Animator>(); // Get the Animator component
        attacking = false; // Initialize attacking flag to false
        playerController = GetComponent<PlayerControllera>();
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

    private IEnumerator HairBallRollAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting HairBallRoll attack");
        pawEye.gameObject.SetActive(false);

        // Random number of hairballs to spawn within a specified range
        int minHairballs = 2;
        int maxHairballs = 3;
        int numberOfHairballs = Random.Range(minHairballs, maxHairballs + 1);
        List<GameObject> pawEyeInstances = new List<GameObject>();
        List<GameObject> savePawInstances = new List<GameObject>();

        // Iterate through fire points
        foreach (Transform firePoint in firePoints)
        {
            // Determine if this fire point should spawn a hairball
            bool spawnHairball = Random.value < 0.5f; // Example condition, adjust as needed

            if (spawnHairball && numberOfHairballs > 0)
            {
                // Instantiate PawEye prefab
                GameObject pawEyeInstance = Instantiate(pawEyePrefab, firePoint.position, firePoint.rotation);
                pawEyeInstances.Add(pawEyeInstance);

                // Wait for a few seconds before spawning the hairball
                yield return new WaitForSeconds(0.5f); // Adjust as needed

                // Spawn hairball at the selected fire point
                Instantiate(hairballPrefab, firePoint.position, firePoint.rotation);
                numberOfHairballs--;
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

    private IEnumerator ClawAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Claw attack");

        // Variables for the attack logic
        float followDuration = 5.0f; // Duration to wait before spawning claws
        int maxClawSpawns = 4; // Number of claws to spawn
        int clawSpawnCount = 0; // Counter for claws spawned

        // Set initial position
        Vector3 currentPosition = pawMouth.transform.position;

        // Loop until attack conditions are met
        while (clawSpawnCount < maxClawSpawns)
        {
            // Set stop duration for PawMouth
            pawMouth.SetStopDuration(followDuration);

            // Activate PawMouth
            Debug.Log("PawMouth activated");
            pawMouth.ActivatePaw();

            // Spawn a claw at the current position
            SpawnClaw(currentPosition);
            clawSpawnCount++;

            // Wait for the specified duration at the current position
            yield return new WaitForSeconds(followDuration);

            // Update currentPosition to PawMouth's new position after waiting
            currentPosition = pawMouth.transform.position;

            // Deactivate PawMouth
            pawMouth.DeactivatePaw();
        }

        Debug.Log("Claw attack completed");

        // Ensure PawMouth is deactivated
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

    private IEnumerator SlammingAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Paw Slamming attack");

        // Define the spawn points
        Transform[] spawnPoints = new Transform[] { spawnPoint1, spawnPoint2, spawnPoint3, spawnPoint4 };
        float totalAttackDuration = 20.0f; // Total duration for the attack
        float moveDuration = 3.0f; // Time to move to each spawn point
        int numberOfLoops = 4; // Number of times to loop through all spawn points
        float cooldownDuration = 5.0f; // Cooldown duration after completing all loops

        // Calculate the time to stay at each spawn point
        float timePerSpawnPoint = (totalAttackDuration - (moveDuration * (spawnPoints.Length - 1))) / spawnPoints.Length;

        // Define compass directions
        Vector3[] directions = new Vector3[]
        {
        Vector3.up, // North
        Vector3.down, // South
        Vector3.right, // East
        Vector3.left, // West
        new Vector3(1, 1, 0).normalized, // North-East
        new Vector3(-1, 1, 0).normalized, // North-West
        new Vector3(1, -1, 0).normalized, // South-East
        new Vector3(-1, -1, 0).normalized // South-West
        };

        // Activate PawMouth and PawEye
        pawMouth.ActivatePaw();
        pawEye.ActivatePaw();

        for (int loop = 0; loop < numberOfLoops; loop++)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                // Move to spawn point
                float startTime = Time.time;
                while (Vector3.Distance(pawMouth.transform.position, spawnPoint.position) > 0.1f)
                {
                    pawMouth.transform.position = Vector3.MoveTowards(pawMouth.transform.position, spawnPoint.position, Time.deltaTime * moveDuration);
                    yield return null;
                }

                // Ensure PawMouth is at the spawn point
                pawMouth.transform.position = spawnPoint.position;

                // Calculate remaining time at this spawn point
                float elapsed = Time.time - startTime;
                float remainingTimeAtPoint = Mathf.Max(timePerSpawnPoint - elapsed, 0);

                // Spawn claws in compass directions
                foreach (Vector3 direction in directions)
                {
                    SpawnClaw(pawMouth.transform.position, direction);
                }

                // Pause at the spawn point
                yield return new WaitForSeconds(remainingTimeAtPoint);
            }
        }

        // Cooldown period after completing all loops
        yield return new WaitForSeconds(cooldownDuration);

        // Deactivate PawEye and PawMouth
        pawEye.DeactivatePaw();
        pawMouth.DeactivatePaw();

        Debug.Log("Paw Slamming attack completed");

        // Call the callback to signal completion
        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
        myAnim.SetBool("Paw8", false);
        myAnim.SetBool("CoolDown", true);
    }





    // Method to spawn a claw projectile
    private void SpawnClaw(Vector3 position, Vector3 direction)
    {
        Debug.Log("Spawning Claw at: " + position);

        // Instantiate the claw prefab or perform any claw attack logic here
        if (Claw != null)
        {
            GameObject clawInstance = Instantiate(Claw, position, Quaternion.identity);
            Claw clawScript = clawInstance.GetComponent<Claw>();

            if (clawScript != null)
            {
                clawScript.Shoot(direction); // Ensure this method exists in your Claw script
            }
            else
            {
                Debug.LogError("Claw script not found on Claw prefab.");
                Destroy(clawInstance);
            }
        }
        else
        {
            Debug.LogError("Claw prefab not assigned.");
        }
    }




    private IEnumerator SummoningAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Summoning attack");

        // Freeze the player controls
        FreezePlayer(true);

        // Loop to summon rats at the spawn points
        for (int i = 0; i < numberOfRats; i++)
        {
            // Select a spawn point from the spawnPoints array
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            Debug.Log("Spawning rat at: " + spawnPoint.position);

            // Calculate duration based on distance and speed
            float distance = Vector3.Distance(transform.position, spawnPoint.position);
            float speed = 5.0f; // Adjust speed as needed
            float duration = distance / speed;

            // Instantiate the paw at the Boss's position and move towards the spawn point
            GameObject pawInstance = Instantiate(pawEyePrefab, transform.position, Quaternion.identity);
            StartCoroutine(MovePawTowardsTarget(pawInstance, spawnPoint.position, duration));

            // Instantiate the rat at the selected spawn point
            GameObject ratInstance = Instantiate(ratPrefab, spawnPoint.position, spawnPoint.rotation);
            activeRatsCount++;
            Debug.Log("Active rats count after spawning: " + activeRatsCount);

            yield return new WaitForSeconds(0.4f);
        }

        Debug.Log("Summoning attack completed");

        // Unfreeze the player controls
        FreezePlayer(false);

        // Call the callback to signal completion
        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
    }

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

    // Call the new fade method after reaching the target
    StartCoroutine(FadeAwayPaw(paw));
}


    private IEnumerator FadeAwayPaw(GameObject pawInstance)
    {
        // Fade out effect logic can be added here if needed (e.g., scale down, change opacity)
        yield return new WaitForSeconds(fadeDuration); // Wait for fade duration
        Destroy(pawInstance); // Destroy the paw instance
    }


    public void KillRat()
    {
        activeRatsCount--;
        if (activeRatsCount <= 0)
        {
            myAnim.SetBool("CoolDown", true); // Set the CoolDown animation state
            myAnim.SetBool("Summon", false); // Set the CoolDown animation state
            attacking = false;
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
        currentAttackStage = (currentAttackStage + 1) % attackStages.Length; // Move to the next attack stage
        string nextAttack = attackStages[currentAttackStage];
        myAnim.SetTrigger(nextAttack); // Trigger the next attack animation
    }

    public void OnBossDefeated()
    {
        if (!died)
        {
            died = true;
            myAnim.SetBool("isDead", true);
            // Handle the logic for when the boss is defeated
            Debug.Log("Boss defeated");
        }
    }
}
