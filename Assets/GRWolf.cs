using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GRWolf : StateMachineBehaviour
{
    private Boss1 boss1; // Reference to the Boss script

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get reference to the Boss component
        boss1 = animator.GetComponent<Boss1>();

        // Perform actions on entering GettingReady state, if any
        Debug.Log("Entered GettingReady state");
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Transition to HairBall state if the boss is ready for the next attack
        if (boss1 != null && boss1.IsReadyForNextAttack())
        {
            animator.SetBool("Summon", true);
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Perform actions on exiting GettingReady state, if any
        Debug.Log("Exited Ready state");
    }
}
