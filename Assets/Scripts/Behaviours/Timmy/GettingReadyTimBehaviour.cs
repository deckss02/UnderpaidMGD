using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingReadyTimBehaviour : StateMachineBehaviour
{
    private BossTim bossTim; // Reference to the Boss script

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get reference to the Boss component
        bossTim = animator.GetComponent<BossTim>();

        // Perform actions on entering GettingReady state, if any
        Debug.Log("Entered GettingReady state");
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Transition to HairBall state if the boss is ready for the next attack
        if (bossTim != null && bossTim.IsReadyForNextAttack())
        {
            animator.SetBool("VineL", true);
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Perform actions on exiting GettingReady state, if any
        Debug.Log("Exited Ready state");
        animator.ResetTrigger("GR");
    }
}
