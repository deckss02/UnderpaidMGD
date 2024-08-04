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
    public Transform spawnPt1;
    public Transform spawnPt2;
    public Transform spawnPt3;
    public Transform spawnPt4;
    public GameObject clawBulletPrefab; // New prefab for the claw bullet
    public GameObject ratPrefab; // Prefab for the rat enemy
    public Transform[] spawnPoints; // Points where rats will be spawned
    public int numberOfRats = 6; // Number of rats to spawn
    public PlayerControllera playerControllera; // Reference to the player's controller script
    private bool died = false;
    private string[] attackStages = { "PawAttack", "HairballAttack", "ClawAttack", "SummonMinions" };
    private int currentAttackStage = 0; // Current attack stage index
    public Animator myAnim; // Animator component
    public bool attacking; // Flag to indicate if the boss is currently attacking
    public FollowPath followPathComponentPawEye;
    public FollowPath followPathComponentPawMouth;
    public PawEye pawEye; // Reference to the PawEye component
    public GameObject Claw;
    public PawMouth pawMouth; // Reference to the PawMouth component
    public GameObject pawEyePrefab; // Prefab for the pawEye GameObject
    public GameObject SavePaw; // Prefab for safe lane
    private Vector3 pawStopPosition; // Variable to store the position where the paw stops
    public int activeRatsCount;

    private void Start()
    {
        myAnim = GetComponent<Animator>(); // Get the Animator component
        attacking = false; // Initialize attacking flag to false
        playerControllera = FindObjectOfType<PlayerControllera>();
        followPathComponentPawEye = pawEye.GetComponent<FollowPath>();
        followPathComponentPawMouth = pawMouth.GetComponent<FollowPath>();
        followPathComponentPawEye.SetActive(true);
        followPathComponentPawMouth.SetActive(true);
    }

    public void StartHairBallRollAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(HairBallRollAttackCoroutine(onComplete)); // Start the HairBallRoll attack coroutine
    }
    public void StartClawAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(ClawAttackCoroutine(onComplete)); // Start the Claw attack coroutine
    }
    public void StartSlammingAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(SlammingAttackCoroutine(onComplete)); // Start the Slamming attack coroutine
    }
    public void StartSummoningAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(SummoningAttackCoroutine(onComplete)); // Start the Summoning attack coroutine
    }

    private IEnumerator HairBallRollAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting HairBallRoll attack");

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
                GameObject pawEyeInstance = Instantiate(pawEyePrefab, firePoint.position, firePoint.rotation);
                pawEyeInstances.Add(pawEyeInstance);

                yield return new WaitForSeconds(0.5f); // Adjust as needed

                // Spawn hairball at the selected fire point
                Instantiate(hairballPrefab, firePoint.position, firePoint.rotation);
                numberOfHairballs--;
            }
            else
            {
                GameObject savePawInstance = Instantiate(SavePaw, firePoint.position, firePoint.rotation);
                savePawInstances.Add(savePawInstance);
            }
        }

        // Schedule deactivation of pawEyes and savePaws after their own fade duration
        foreach (GameObject pawEyeInstance in pawEyeInstances)
        {
            StartCoroutine(FadeAway(pawEyeInstance));
        }

        foreach (GameObject savePawInstance in savePawInstances)
        {
            StartCoroutine(FadeAway(savePawInstance));
        }

        Debug.Log("HairBallRoll attack completed");
        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
    }

    private IEnumerator FadeAway(GameObject instance)
    {
        // Wait for a default duration or adjust as needed
        yield return new WaitForSeconds(2.0f);

        // Destroy the instance after the wait
        Destroy(instance);
    }

    private IEnumerator SummoningAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Summoning attack");

        // Freeze the player controls
        playerControllera.FreezePlayer(true);

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
        playerControllera.FreezePlayer(false);

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
            paw.transform.rotation = Quaternion.identity;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        paw.transform.position = targetPosition;
        paw.transform.rotation = Quaternion.identity;

        // Call the fade method after reaching the target
        StartCoroutine(FadeAway(paw));
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

    private IEnumerator ClawAttackCoroutine(System.Action onComplete)
    {
        // Deactivate any path-following components
        followPathComponentPawEye.SetActive(false);
        followPathComponentPawMouth.SetActive(false);

        Debug.Log("Starting Claw attack");
        pawMouth.ActivatePaw(); // Start the paw movement

        float followDuration = 10.0f; // Duration to wait before spawning claws
        int maxClawSpawns = 4; // Number of claws to spawn
        int clawSpawnCount = 0; // Counter for claws spawned

        while (clawSpawnCount < maxClawSpawns)
        {
            pawMouth.SetStopDuration(followDuration);
            Debug.Log("PawMouth activated");

            // Wait for the paw to follow the player and pause
            yield return new WaitForSeconds(followDuration);

            // Spawn a claw at the current position (paw's position)
            Vector3 spawnPosition = pawMouth.transform.position;
            SpawnClaw(spawnPosition);
            clawSpawnCount++;

            // Deactivate the paw after spawning a claw
            pawMouth.DeactivatePaw();
            Debug.Log("PawMouth deactivated");

            // Small wait time to prevent overlap issues
            yield return new WaitForSeconds(1.0f);
        }

        Debug.Log("Claw attack completed");

        // Reactivate path-following components
        followPathComponentPawEye.SetActive(true);
        followPathComponentPawMouth.SetActive(true);

        // Reset animation states
        myAnim.SetBool("Claw", false);
        myAnim.SetBool("CoolDown", true);

        // Complete the attack
        onComplete?.Invoke();
        attacking = false;
    }

    // Method to spawn a claw at a specific position
    private void SpawnClaw(Vector3 spawnPosition)
    {
        Debug.Log("Spawning Claw at: " + spawnPosition);
        Instantiate(clawBulletPrefab, spawnPosition, Quaternion.identity);
    }


    private IEnumerator SlammingAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Paw Slamming attack");
        followPathComponentPawEye.SetActive(false);
        followPathComponentPawMouth.SetActive(false);

        // Define the two arrays of spawn points
        Transform[] spawnPoints1 = new Transform[] { spawnPoint1, spawnPoint2, spawnPoint3, spawnPoint4 };
        Transform[] spawnPoints2 = new Transform[] { spawnPt1, spawnPt2, spawnPt3, spawnPt4 };

        float totalAttackDuration = 1.0f; // Total duration for the attack
        int numberOfLoops = 2; // Number of times to loop through all spawn points
        float cooldownDuration = 2.0f; // Cooldown duration after completing all loops
        float clawLifetime = 8.0f; // Lifetime of each claw
        float pawSpeed = 2.0f; // Speed of the paw
        float moveDistance = 5.0f; // Distance the paw moves

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

        pawMouth.ActivatePaw();

        for (int loop = 0; loop < numberOfLoops; loop++)
        {
            // Switch between the two arrays of spawn points
            Transform[] currentSpawnPoints = (loop % 2 == 0) ? spawnPoints1 : spawnPoints2;

            foreach (Transform spawnPoint in currentSpawnPoints)
            {
                // Calculate the duration based on distance and speed
                float moveDuration = moveDistance / pawSpeed;
                float elapsedTime = 0;
                Vector3 startPosition = pawMouth.transform.position;
                Vector3 targetPosition = Vector3.MoveTowards(startPosition, spawnPoint.position, moveDistance);

                while (elapsedTime < moveDuration)
                {
                    pawMouth.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // Ensure PawMouth is at the target position
                pawMouth.transform.position = targetPosition;

                // Calculate remaining time at this spawn point
                float timePerSpawnPoint = (totalAttackDuration - (moveDuration * (spawnPoints1.Length - 1))) / spawnPoints1.Length;
                float remainingTimeAtPoint = timePerSpawnPoint;

                // Spawn claws in compass directions
                foreach (Vector3 direction in directions)
                {
                    GameObject clawInstance = SpawnClaw(pawMouth.transform.position, direction);
                    if (clawInstance != null)
                    {
                        Destroy(clawInstance, clawLifetime); // Destroy the claw instance after a set time
                    }
                }

                // Pause at the spawn point
                yield return new WaitForSeconds(remainingTimeAtPoint);
            }
        }

        // Cooldown period after completing all loops
        yield return new WaitForSeconds(cooldownDuration);

        pawMouth.DeactivatePaw();

        Debug.Log("Paw Slamming attack completed");

        followPathComponentPawEye.SetActive(true);
        followPathComponentPawMouth.SetActive(true);
        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
        myAnim.SetBool("Paw8", false);
        myAnim.SetBool("CoolDown", true);
    }

    private GameObject SpawnClaw(Vector3 position, Vector3 direction)
    {
        // Instantiate the claw prefab or perform any claw attack logic here
        if (Claw != null)
        {
            GameObject clawInstance = Instantiate(Claw, position, Quaternion.identity);
            Claw clawScript = clawInstance.GetComponent<Claw>();

            if (clawScript != null)
            {
                clawScript.Shoot(direction); // Ensure this method exists in your Claw script
                Debug.Log("Claw shot in direction: " + direction);
                return clawInstance;
            }
        }
        return null;
    }

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
            Debug.Log("Boss defeated");
        }
    }
}
