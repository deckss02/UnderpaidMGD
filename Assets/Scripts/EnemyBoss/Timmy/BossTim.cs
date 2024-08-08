using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTim : MonoBehaviour
{
    public GameObject hairballPrefab; // Prefab for the hairball projectile
    public Transform[] firePoints; // Points from which hairballs will be fired
    public GameObject BirdPrefab; // Prefab for the rat enemy
    public Transform[] spawnPoints; // Points where rats will be spawned
    public int numberOfBirbs = 6; // Number of rats to spawn
    public PlayerControllera playerControllera; // Reference to the player's controller script
    private bool died = false;
    private string[] attackStages = { "VineLane", "VineTeleport", "ExplodingBird", "SummonMinions" };
    private int currentAttackStage = 0; // Current attack stage index
    public Animator myAnim; // Animator component
    public bool attacking; // Flag to indicate if the boss is currently attacking
    public int activeRatsCount;

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
    public void StartBirdAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(ExplodingBirdAttackCoroutine(onComplete)); // Start the ExplodingBird attack coroutine
    }
    public void StartSummoningAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(SummoningAttackCoroutine(onComplete)); // Start the Summoning attack coroutine
    }
   
    private IEnumerator VineLaneAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting VineLane attack");
        //int minHairballs = 2;        // Random number of hairballs to spawn within a specified range
        //int maxHairballs = 3;
        //int numberOfHairballs = Random.Range(minHairballs, maxHairballs + 1);
        //List<GameObject> pawEyeInstances = new List<GameObject>();
        //List<GameObject> savePawInstances = new List<GameObject>();
        //
        //foreach (Transform firePoint in firePoints)     // Iterate through fire points
        //{
        //    bool spawnHairball = Random.value < 0.5f; // Determine if this fire point should spawn a hairball
        //
        //    if (spawnHairball && numberOfHairballs > 0)
        //    {
        //        GameObject pawEyeInstance = Instantiate(pawEyePrefab, firePoint.position, firePoint.rotation);
        //        pawEyeInstances.Add(pawEyeInstance);
        //        yield return new WaitForSeconds(0.5f);
        //        Instantiate(hairballPrefab, firePoint.position, firePoint.rotation); // Spawn hairball at the selected fire point
        //        numberOfHairballs--;
        //    }
        //    else
        //    {
        //        GameObject savePawInstance = Instantiate(SavePaw, firePoint.position, firePoint.rotation);
        //        savePawInstances.Add(savePawInstance);
        //    }
        //}
        //foreach (GameObject pawEyeInstance in pawEyeInstances) // Schedule deactivation of pawEyes and savePaws after their own fade duration
        //{
        //    StartCoroutine(FadeAway(pawEyeInstance));
        //}
        //foreach (GameObject savePawInstance in savePawInstances)
        //{
        //    StartCoroutine(FadeAway(savePawInstance));
        //}
        yield return null;  // Ensure the coroutine returns a value

        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
    }
    //
    //private IEnumerator FadeAway(GameObject instance)
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    Destroy(instance);
    //}
    //
    private IEnumerator SummoningAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Summoning attack");
        playerControllera.FreezePlayer(true);
        //for (int i = 0; i < numberOfRats; i++) // Loop to summon rats at the spawn points
        //{
        //    Transform spawnPoint = spawnPoints[i % spawnPoints.Length]; // Select a spawn point from the spawnPoints array
        //    float distance = Vector3.Distance(transform.position, spawnPoint.position);
        //    float speed = 5.0f; // Adjust speed as needed
        //    float duration = distance / speed;
        //
        //    GameObject pawInstance = Instantiate(pawEyePrefab, transform.position, Quaternion.identity); // Instantiate the paw at the Boss's position and move towards the spawn point
        //    StartCoroutine(MovePawTowardsTarget(pawInstance, spawnPoint.position, duration));
        //
        //    GameObject ratInstance = Instantiate(ratPrefab, spawnPoint.position, spawnPoint.rotation); // Instantiate the rat at the selected spawn point
        //    activeRatsCount++;
        //    yield return new WaitForSeconds(0.4f);
        //}
        yield return null;  // Ensure the coroutine returns a value

        playerControllera.FreezePlayer(false);
        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
    }
    public void KillRat()
    {
         //activeRatsCount--;
         //if (activeRatsCount <= 0)
         //{
         //    myAnim.SetBool("CoolDown", true); // Set the CoolDown animation state
         //    myAnim.SetBool("Summon", false); // Set the CoolDown animation state
         //    attacking = false;
         //}
    }

    private IEnumerator VineTeleportAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting VineTeleport attack");

        yield return null;  // Ensure the coroutine returns a value

        myAnim.SetBool("VineT", false);
        myAnim.SetBool("CoolDown", true);
        onComplete?.Invoke();
        attacking = false;
    }

    private IEnumerator ExplodingBirdAttackCoroutine(System.Action onComplete)
    {
        Debug.Log("Starting Bird attack");

        yield return null;  // Ensure the coroutine returns a value

        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
        myAnim.SetBool("Bird", false);
        myAnim.SetBool("CoolDown", true);
    }

    //private void SpawnBird(Vector3 spawnPosition)
    //{
    //    Debug.Log("Spawning Bird at: " + spawnPosition);
    //    Instantiate(BirdPrefab, spawnPosition, Quaternion.identity);
    //}

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