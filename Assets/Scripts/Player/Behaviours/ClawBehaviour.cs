using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawBehaviour : StateMachineBehaviour
{
    private Boss boss;

    private bool hasAttacked = false; // Flag to track whether the summonminion attack has been executed

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss == null)
            boss = animator.GetComponent<Boss>();


        // Start the Claw attack with a callback to this behaviour
        if (boss != null)
        {
            boss.StartClawAttack(OnClawComplete);
        }
        else
        {
            Debug.LogError("Boss component is not assigned.");
        }
    }
    // Callback method to be called when the claw attack is complete
    private void OnClawComplete()
    {
        hasAttacked = true; // Set the flag to true to trigger state transition
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if the claw attack has been executed and transition to cooldown state
        if (hasAttacked)
        {
            animator.SetBool("CoolDown", true);
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the flag for the next time this state is entered
        hasAttacked = false;
        animator.SetBool("Claw", false);
    }
}
