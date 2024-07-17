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
        animator.SetBool("GettingReady", true);
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Perform actions on exiting Idle state, if any
        Debug.Log("Exited Idle state");
        animator.SetBool("Idle", false);
    }
}
