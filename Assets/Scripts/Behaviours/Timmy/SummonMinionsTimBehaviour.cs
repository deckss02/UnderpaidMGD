using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMinionsTimBehaviour : StateMachineBehaviour
{
    private BossTim bossTim; // Reference to the Boss script

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the Boss component from the animator's GameObject
        if (bossTim == null)
            bossTim = animator.GetComponent<BossTim>();

        // Start the summoning minions attack
        if (bossTim != null)
        {
            bossTim.StartSummoningAttack(null); // No need to pass a callback
        }
        else
        {
            Debug.LogError("Boss component is not assigned.");
        }
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Update logic if needed
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the cooldown flag for the next time this state is entered
        animator.SetBool("Summon", false); // Reset summoning trigger
        animator.SetBool("CoolDown", true); // Reset summoning trigger
    }
}



