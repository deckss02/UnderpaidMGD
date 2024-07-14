using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMinionsBehaviour : StateMachineBehaviour
{
    private Boss boss; // Reference to the Boss script

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the Boss component from the animator's GameObject
        if (boss == null)
            boss = animator.GetComponent<Boss>();

        // Start the summoning minions attack with a callback to this behaviour
        if (boss != null)
        {
            boss.StartSummoningAttack(OnSummonComplete);
        }
        else
        {
            Debug.LogError("Boss component is not assigned.");
        }
    }

    // Callback method to be called when the summon attack is complete
    private void OnSummonComplete()
    {
        // Nothing needed here; the Boss class will handle transitioning to cooldown when all minions are dead
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if all summoned minions are dead and transition to cooldown state
        if (boss != null && boss.HasDied)
        {
            animator.SetBool("CoolDown", true); // Transition to cooldown state
            Debug.Log("All minions dead, transitioning to CoolDown state");
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the cooldown flag for the next time this state is entered
        animator.SetBool("Summon", false); // Reset summoning trigger
    }
}


