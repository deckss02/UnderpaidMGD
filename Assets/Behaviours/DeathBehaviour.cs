using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set the isDead parameter to true
        animator.SetBool("isDead", true);

        // Play the death animation or perform other actions
        // For example, disable the boss's collider or make it unresponsive
        Collider2D bossCollider = animator.GetComponent<Collider2D>();
        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }

        // Optionally, you can destroy the boss after the death animation finishes
        // Destroy(animator.gameObject, stateInfo.length);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // You can add any additional logic that should occur during the death animation
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // You can add any cleanup logic here if needed
    }
}