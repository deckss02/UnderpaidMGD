using System.Collections;
using UnityEngine;

public class BossTim : MonoBehaviour
{
    public GameObject VinePrefab;
    public Transform[] firePoints;
    public GameObject BirdPrefab; // Prefab for the bird enemy
    public Transform[] spawnPoints;
    public int numberOfBirds = 6; // Number of birds to spawn
    public PlayerControllera playerControllera; // Reference to the player's controller script
    private bool died = false;
    private string[] attackStages = { "VineLane", "VineTeleport", "ExplodingBird", "SummonMinions" };
    private int currentAttackStage = 0; // Current attack stage index
    public Animator myAnim; // Animator component
    public bool attacking; // Flag to indicate if the boss is currently attacking
    public int activeBirdsCount;

    public GameObject seedPrefab;        // The seed prefab to spawn
    public Transform[] seedSpawnPoints1; // First array of seed spawn points
    public Transform[] seedSpawnPoints2; // Second array of seed spawn points
    public Transform[] seedSpawnPoints3; // Third array of seed spawn points
    public float spawnInterval = 2f;     // Time between each seed spawn
    public int seedCount = 5;             // Number of seeds to spawn

    private void Start()
    {
        myAnim = GetComponent<Animator>(); // Get the Animator component
        attacking = false; // Initialize attacking flag to false
        playerControllera = FindObjectOfType<PlayerControllera>();
    }

    public void StartVineLaneAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(VineLaneAttackCoroutine(onComplete)); // Start the VineLane attack coroutine
    }

    public void StartVineTeleportAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(VineTeleportAttackCoroutine(onComplete)); // Start the VineTeleport attack coroutine
    }

    public void StartSummoningAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(SummoningAttackCoroutine(onComplete)); // Start the Summoning attack coroutine
    }

    private IEnumerator VineLaneAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting VineLane attack");
        // Implementation of VineLane attack
        yield return null;  // Ensure the coroutine returns a value

        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
    }

    private IEnumerator SummoningAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Summoning attack");
        playerControllera.FreezePlayer(true);

        for (int i = 0; i < numberOfBirds; i++) // Loop to summon birds at the spawn points
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length]; // Select a spawn point from the spawnPoints array
            float distance = Vector3.Distance(transform.position, spawnPoint.position);
            float speed = 5.0f; // Adjust speed as needed
            float duration = distance / speed;

            GameObject birdInstance = Instantiate(BirdPrefab, spawnPoint.position, spawnPoint.rotation);
            activeBirdsCount++;
            yield return new WaitForSeconds(0.4f);
        }
        yield return null;  // Ensure the coroutine returns a value

        playerControllera.FreezePlayer(false);
        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
    }

    public void KillBird()
    {
        activeBirdsCount--;
        if (activeBirdsCount <= 0)
        {
            myAnim.SetBool("CoolDown", true); // Set the CoolDown animation state
            myAnim.SetBool("Summon", false); // Set the CoolDown animation state
            attacking = false;
        }
    }

    private IEnumerator VineTeleportAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting VineTeleport attack");
        StartCoroutine(SpawnSeeds());

        yield return new WaitForSeconds(7.0f); // Example delay

        ResetVineTeleport();
        onComplete?.Invoke();
        attacking = false;
    }

    private IEnumerator SpawnSeeds()
    {
        // Group the seed spawn points into an array
        Transform[][] allPatterns = new Transform[][]
        {
            seedSpawnPoints1,
            seedSpawnPoints2,
            seedSpawnPoints3
        };

        // Pick a random pattern
        Transform[] selectedPattern = allPatterns[Random.Range(0, allPatterns.Length)];

        // Spawn a specific number of seeds at the selected pattern points
        for (int i = 0; i < seedCount; i++)
        {
            // Check if we have enough spawn points, if not, break
            if (i >= selectedPattern.Length)
                break;

            Transform spawnPoint = selectedPattern[i];
            Instantiate(seedPrefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void ResetVineTeleport()
    {
        // Reset animator parameters related to VineTeleport
        myAnim.SetBool("GroundVine", false);
        myAnim.SetBool("CoolDown", false);
    }

    public bool IsReadyForNextAttack()
    {
        return !myAnim.GetCurrentAnimatorStateInfo(0).IsName("CoolDown"); // Check if CooldownBehaviour is in cooldown state
    }

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
