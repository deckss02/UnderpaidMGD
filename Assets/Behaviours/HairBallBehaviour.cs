using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairBallBehaviour : StateMachineBehaviour
{
    private BossHealth bossHealth; // Reference to the BossHealth script
    private bool hasAttacked = false; // Flag to track whether the hairball attack has been executed
    private float elapsedTime = 0f; // Elapsed time since entering the state
    private float timeUntilSummon = 3f; // Time until transitioning to Summon state if no damage is taken

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
        else
        {
            animator.SetBool("HairBall", false);
            animator.SetBool("Summon", true);
        }
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Increment elapsed time
        elapsedTime += Time.deltaTime;

        // Check if the hairball attack has been executed and transition to cooldown state
        if (hasAttacked && stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("Cooldown", true);
        }

        // Check if it's time to transition to the Summon state
        if (elapsedTime >= timeUntilSummon)
        {
            animator.SetBool("HairBall", false);
            animator.SetBool("Summon", true);
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the flag and elapsed time for the next time this state is entered
        hasAttacked = false;
        elapsedTime = 0f;
        animator.SetBool("HairBall", false);
    }
}