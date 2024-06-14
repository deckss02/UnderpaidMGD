using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairBallBehaviour : StateMachineBehaviour
{
    private BossHealth bossHealth; // Reference to the BossHealth script
    private bool hasAttacked = false; // Flag to track whether the hairball attack has been executed

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the BossHealth component from the animator's GameObject
        if (bossHealth == null)
            bossHealth = animator.GetComponent<BossHealth>();

        // If the boss has taken damage, transition to TakeDamage state
        if (bossHealth != null && bossHealth.currentHealth < bossHealth.maxHealth)
        {
            animator.SetBool("Damage", true);
        }

        // Start the HairBallRoll attack coroutine if no damage was taken
        if (bossHealth != null && bossHealth.currentHealth >= bossHealth.maxHealth)
            bossHealth.StartHairBallRollAttack();
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if the hairball attack has been executed and transition to cooldown state
        if (hasAttacked && stateInfo.normalizedTime >= 3.0f)
        {
            animator.SetBool("Cooldown", true);
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the flag for the next time this state is entered
        hasAttacked = false;
        animator.SetBool("HairBall", false);
    }
}