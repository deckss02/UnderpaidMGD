using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWolf : StateMachineBehaviour
{
    private Boss1 boss1; // Reference to the Boss script

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

            boss1 = animator.GetComponent<Boss1>();

        // Start the summoning minions attack
        if (boss1 != null)
        {
            boss1.StartSummoningAttack(null); // No need to pass a callback
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
