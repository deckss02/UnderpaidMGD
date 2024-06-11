using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Other variables and references...

    // Attack stage names
    private string[] attackStages = { "PawAttack", "HairballAttack", "ClawAttack", "SummonMinions" };
    private int currentAttackStage = 0; // Current attack stage index

    private Animator myAnim;
    public bool attacking;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        attacking = true;
    }

    private void Update()
    {
        // Update logic if needed
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