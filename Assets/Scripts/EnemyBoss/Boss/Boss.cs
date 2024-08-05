using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public EyeShoot eyeShoot;
    public MouthMove mouthMove; // Reference to the MouthMove script
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
    public FollowPath followPath; // Reference to the FollowPath script

    private void Start()
    {
        myAnim = GetComponent<Animator>(); // Get the Animator component
        attacking = false; // Initialize attacking flag to false
        playerControllera = FindObjectOfType<PlayerControllera>();
        followPathComponentPawEye = pawEye.GetComponent<FollowPath>();
        followPathComponentPawMouth = pawMouth.GetComponent<FollowPath>();
        followPathComponentPawEye.SetActive(true);
        followPathComponentPawMouth.SetActive(true);
        eyeShoot.enabled = false;
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
        int minHairballs = 2;        // Random number of hairballs to spawn within a specified range
        int maxHairballs = 3;
        int numberOfHairballs = Random.Range(minHairballs, maxHairballs + 1);
        List<GameObject> pawEyeInstances = new List<GameObject>();
        List<GameObject> savePawInstances = new List<GameObject>();

        foreach (Transform firePoint in firePoints)     // Iterate through fire points
        {
            bool spawnHairball = Random.value < 0.5f; // Determine if this fire point should spawn a hairball

            if (spawnHairball && numberOfHairballs > 0)
            {
                GameObject pawEyeInstance = Instantiate(pawEyePrefab, firePoint.position, firePoint.rotation);
                pawEyeInstances.Add(pawEyeInstance);
                yield return new WaitForSeconds(0.5f); 
                Instantiate(hairballPrefab, firePoint.position, firePoint.rotation); // Spawn hairball at the selected fire point
                numberOfHairballs--;
            }
            else
            {
                GameObject savePawInstance = Instantiate(SavePaw, firePoint.position, firePoint.rotation);
                savePawInstances.Add(savePawInstance);
            }
        }
        foreach (GameObject pawEyeInstance in pawEyeInstances) // Schedule deactivation of pawEyes and savePaws after their own fade duration
        {
            StartCoroutine(FadeAway(pawEyeInstance));
        }
        foreach (GameObject savePawInstance in savePawInstances)
        {
            StartCoroutine(FadeAway(savePawInstance));
        }
        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
    }

    private IEnumerator FadeAway(GameObject instance)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(instance);
    }

    private IEnumerator SummoningAttackCoroutine(System.Action onComplete)
    {
        playerControllera.FreezePlayer(true);
        for (int i = 0; i < numberOfRats; i++) // Loop to summon rats at the spawn points
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length]; // Select a spawn point from the spawnPoints array
            float distance = Vector3.Distance(transform.position, spawnPoint.position);
            float speed = 5.0f; // Adjust speed as needed
            float duration = distance / speed;

            GameObject pawInstance = Instantiate(pawEyePrefab, transform.position, Quaternion.identity); // Instantiate the paw at the Boss's position and move towards the spawn point
            StartCoroutine(MovePawTowardsTarget(pawInstance, spawnPoint.position, duration));

            GameObject ratInstance = Instantiate(ratPrefab, spawnPoint.position, spawnPoint.rotation); // Instantiate the rat at the selected spawn point
            activeRatsCount++;
            yield return new WaitForSeconds(0.4f);
        }
        playerControllera.FreezePlayer(false);
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
        eyeShoot.enabled = true;
        followPath.SetUseFading(false);
        followPathComponentPawMouth.SetActive(false);
        Debug.Log("Starting Claw attack");
        mouthMove.StartMovingPaw();
        float followDuration = 5.0f; // Duration to wait before spawning claws
        int maxClawSpawns = 4; // Number of claws to spawn
        int clawSpawnCount = 0; // Counter for claws spawned

        while (clawSpawnCount < maxClawSpawns)
        {
            yield return new WaitForSeconds(followDuration);

            // Ensure the paw pauses before spawning the claw
            mouthMove.StopMovingPaw();
            Vector3 spawnPosition = mouthMove.transform.position;
            SpawnClaw(spawnPosition);
            clawSpawnCount++;
            Debug.Log("Claw spawned");
            followPathComponentPawMouth.SetActive(false);

            // Restart the paw movement after spawning the claw
            mouthMove.StartMovingPaw();
            yield return new WaitForSeconds(1.0f);
        }
        mouthMove.StopMovingPaw();
        followPathComponentPawMouth.SetActive(true);
        eyeShoot.enabled = false;
        myAnim.SetBool("Claw", false);
        myAnim.SetBool("CoolDown", true);
        followPath.SetUseFading(true);
        onComplete?.Invoke();
        attacking = false;
    }

    private void SpawnClaw(Vector3 spawnPosition)
    {
        Debug.Log("Spawning Claw at: " + spawnPosition);
        Instantiate(clawBulletPrefab, spawnPosition, Quaternion.identity);
    }


    private IEnumerator SlammingAttackCoroutine(System.Action onComplete)
    {
        eyeShoot.enabled = true;
        Debug.Log("Starting Paw Slamming attack");
        followPath.SetUseFading(false);
        followPathComponentPawEye.SetActive(false);
        followPathComponentPawMouth.SetActive(false);

        Transform[] spawnPoints1 = new Transform[] { spawnPoint1, spawnPoint2, spawnPoint3, spawnPoint4 };
        Transform[] spawnPoints2 = new Transform[] { spawnPt1, spawnPt2, spawnPt3, spawnPt4 };

        float totalAttackDuration = 6.0f; // Total duration for the attack
        int numberOfLoops = 2; // Number of times to loop through all spawn points
        float cooldownDuration = 2.0f; // Cooldown duration after completing all loops
        float clawLifetime = 0.3f; // Lifetime of each claw
        float pawSpeed = 2.0f; // Speed of the paw
        float moveDistance = 5.0f; // Distance the paw moves

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
            Transform[] currentSpawnPoints = (loop % 2 == 0) ? spawnPoints1 : spawnPoints2; // Switch between the two arrays of spawn points

            foreach (Transform spawnPoint in currentSpawnPoints)
            {
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
                pawMouth.transform.position = targetPosition;

                float timePerSpawnPoint = (totalAttackDuration - (moveDuration * (spawnPoints1.Length - 1))) / spawnPoints1.Length;
                float remainingTimeAtPoint = timePerSpawnPoint; // Calculate remaining time at this spawn point

                foreach (Vector3 direction in directions)             // Spawn claws in compass directions
                {
                    GameObject clawInstance = SpawnClaw(pawMouth.transform.position, direction);
                    if (clawInstance != null)
                    {
                        Destroy(clawInstance, clawLifetime); // Destroy the claw instance after a set time
                    }
                }
                yield return new WaitForSeconds(remainingTimeAtPoint);
            }
        }
        yield return new WaitForSeconds(cooldownDuration);
        pawMouth.DeactivatePaw();
        eyeShoot.enabled = false;
        followPathComponentPawEye.SetActive(true);
        followPathComponentPawMouth.SetActive(true);
        onComplete?.Invoke();
        followPath.SetUseFading(true);
        attacking = false; // Reset attacking flag
        myAnim.SetBool("Paw8", false);
        myAnim.SetBool("CoolDown", true);
    }

    private GameObject SpawnClaw(Vector3 position, Vector3 direction)
    {
        if (Claw != null) // Instantiate the claw prefab or perform any claw attack logic here
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
        return !myAnim.GetCurrentAnimatorStateInfo(0).IsName("CoolDown"); // Check if CooldownBehaviour is in cooldown state
    }

    private void ProgressToNextAttackStage()     // Progress to the next attack stage
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
