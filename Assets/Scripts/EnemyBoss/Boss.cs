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

    // Attack stage names
    private string[] attackStages = { "PawAttack", "HairballAttack", "ClawAttack", "SummonMinions" };
    private int currentAttackStage = 0; // Current attack stage index

    private Animator myAnim;
    public bool attacking;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        attacking = false;
    }

    private void Update()
    {
        // Update logic if needed
    }

    public void StartHairBallRollAttack(System.Action onComplete)
    {
        attacking = true;
        StartCoroutine(HairBallRollAttackCoroutine(onComplete));
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
    }

    // Reset all attack states and set the boss to idle
    public void ResetAllAttacks()
    {
        attacking = false;
        myAnim.SetTrigger("Reset");
        // Reset any other necessary state here
    }

    // Check if the boss is ready for the next attack
    public bool IsReadyForNextAttack()
    {
        // Check if CooldownBehaviour is in cooldown state
        return !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Cooldown");
    }

    // Progress to the next attack stage
    private void ProgressToNextAttackStage()
    {
        currentAttackStage = (currentAttackStage + 1) % attackStages.Length;
        string nextAttack = attackStages[currentAttackStage];
        myAnim.SetTrigger(nextAttack);
    }

    // Handle minion spawning logic
    public void HandleMinionSpawning()
    {
        // Implement minion spawning logic here if needed
    }
}