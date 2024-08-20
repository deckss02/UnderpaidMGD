using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public GameObject WolfPrefab;
    public int numberOfWolves = 6;
    public PlayerControllera playerControllera; // Reference to the player's controller script
    private bool died = false;
    private string[] attackStages = { "SummonMinions" };
    private int currentAttackStage = 0; // Current attack stage index
    public Animator myAnim; // Animator component
    public bool attacking; // Flag to indicate if the boss is currently attacking
    public int activeWolvesCount;
    public Transform[] spawnPositionsArray1; // Array of spawn positions

    private void Start()
    {
        myAnim = GetComponent<Animator>(); // Get the Animator component
        attacking = false; // Initialize attacking flag to false
        playerControllera = FindObjectOfType<PlayerControllera>();
    }

    public void StartSummoningAttack(System.Action onComplete)
    {
        attacking = true; // Set attacking flag to true
        StartCoroutine(SummoningAttackCoroutine(onComplete)); // Start the Summoning attack coroutine
    }

    private IEnumerator SummoningAttackCoroutine(System.Action onComplete)
    {
        playerControllera.FreezePlayer(true);

        for (int i = 0; i < numberOfWolves; i++) // Loop to summon wolves at the spawn points
        {
            // Select a random spawn point from the spawn positions array
            Transform spawnPoint = spawnPositionsArray1[Random.Range(0, spawnPositionsArray1.Length)];
            float distance = Vector3.Distance(transform.position, spawnPoint.position);
            float speed = 5.0f; // Adjust speed as needed
            float duration = distance / speed;

            GameObject wolfInstance = Instantiate(WolfPrefab, spawnPoint.position, spawnPoint.rotation); // Instantiate the wolf at the selected spawn point
            activeWolvesCount++;
            yield return new WaitForSeconds(0.4f);
        }

        playerControllera.FreezePlayer(false);
        onComplete?.Invoke();
        attacking = false; // Reset attacking flag
    }

    public void KillWolf()
    {
        activeWolvesCount--;
        if (activeWolvesCount <= 0)
        {
            myAnim.SetBool("CoolDown", true); // Set the CoolDown animation state
            myAnim.SetBool("Summon", false); // Set the CoolDown animation state
            attacking = false;
        }
    }

    public bool IsReadyForNextAttack()
    {
        return !myAnim.GetCurrentAnimatorStateInfo(0).IsName("CoolDown"); // Check if CooldownBehaviour is in cooldown state
    }

    private void ProgressToNextAttackStage() // Progress to the next attack stage
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
