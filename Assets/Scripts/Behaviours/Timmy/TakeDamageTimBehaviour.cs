using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageTimBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Damage", true); // Trigger take damage animation
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if the current animation has finished playing
        if (stateInfo.normalizedTime >= 2.0f)
        {
            animator.SetBool("CoolDown", true); // Transition to cooldown state
            animator.SetBool("Damage", false); // Reset take damage animation
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
