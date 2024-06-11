using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    private Boss boss;

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // Perform actions on entering Idle state, if any
        Debug.Log("Entered Idle state");
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Transition to Ready state if the boss is ready for the next attack
        if (boss != null && boss.IsReadyForNextAttack())
        {
            animator.SetBool("GettingReady", true);
        }

        // Reset logic
        if (animator.GetBool("Reset"))
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Reset", false);
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Perform actions on exiting Idle state, if any
        Debug.Log("Exited Idle state");
        animator.SetBool("Idle", false);
    }
}
