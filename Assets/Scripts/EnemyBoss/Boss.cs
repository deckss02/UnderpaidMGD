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
    public int minRats = 5;
    public int maxRats = 7;
    public PlayerController playerController; // Reference to the player's controller script

    // Attack stage names
    private string[] attackStages = { "PawAttack", "HairballAttack", "ClawAttack", "SummonMinions" };
    private int currentAttackStage = 0; // Current attack stage index

    private Animator myAnim;
    public bool attacking;

    // List to track summoned minions
    private List<GameObject> summonedMinions = new List<GameObject>();

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

        // Simulate the Claw attack duration
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Claw attack completed");

        // Call the callback to signal completion
        onComplete?.Invoke();
        attacking = false;
    }

    private IEnumerator SlamingAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Paw Slamming attack");

        // Simulate the Paw Slamming attack duration
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Paw Slamming attack completed");

        // Call the callback to signal completion
        onComplete?.Invoke();
        attacking = false;
    }

    private IEnumerator SummoningAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Summoning attack");

        // Freeze the player controls
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Randomize the number of rats to spawn
        int numberOfRats = Random.Range(minRats, maxRats);
        List<Transform> usedSpawnPoints = new List<Transform>();

        for (int i = 0; i < numberOfRats; i++)
        {
            Transform spawnPoint;
            do
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            } while (usedSpawnPoints.Contains(spawnPoint));

            usedSpawnPoints.Add(spawnPoint);

            // Instantiate the rat at the selected spawn point
            GameObject ratInstance = Instantiate(ratPrefab, spawnPoint.position, spawnPoint.rotation);
            summonedMinions.Add(ratInstance);

            // Add a slight delay between each spawn
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("Summoning attack completed");

        // Re-enable the player controls
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // Call the callback to signal completion
        onComplete?.Invoke();
        attacking = false;
    }

    // Check if all summoned minions are killed
    public bool AreAllMinionsDead()
    {
        // Remove any null entries from the list (destroyed minions)
        summonedMinions.RemoveAll(minion => minion == null);
        return summonedMinions.Count == 0;
    }

    // Transition the boss to the cooldown state
    public void GoToCooldown()
    {
        Debug.Log("All minions are killed. Boss is going to cooldown.");
        myAnim.SetBool("CoolDown", true);
        myAnim.SetBool("Summon", false);
        attacking = false;
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
}
