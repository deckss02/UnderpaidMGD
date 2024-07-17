using System.Collections;
using UnityEngine;

public class HairBallBehaviour : StateMachineBehaviour
{
    private Boss boss; // Reference to the Boss script
    private bool hasAttacked = false; // Flag to track whether the hairball attack has been executed

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the Boss component from the animator's GameObject
        if (boss == null)
            boss = animator.GetComponent<Boss>();

        // Start the HairBallRoll attack with a callback to this behaviour
        if (boss != null)
        {
            boss.StartHairBallRollAttack(OnHairBallAttackComplete);
        }
        else
        {
            Debug.LogError("Boss component is not assigned.");
        }
    }

    // Callback method to be called when the hairball attack is complete
    private void OnHairBallAttackComplete()
    {
        hasAttacked = true; // Set the flag to true to trigger state transition
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if the hairball attack has been executed and transition to cooldown state
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
        animator.SetBool("HairBall", false);
    }
}
