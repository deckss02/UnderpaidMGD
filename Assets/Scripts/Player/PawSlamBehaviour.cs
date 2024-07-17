using System.Collections;
using UnityEngine;

public class PawSlamBehaviour : StateMachineBehaviour
{
    private Boss boss;
    private PawAI pawAI;

    private bool hasAttacked = false; // Flag to track whether the pawslam attack has been executed

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the Boss component from the animator's GameObject
        if (boss == null)
            boss = animator.GetComponent<Boss>();

        // Get the PawAI component from the animator's GameObject
        if (pawAI == null)
            pawAI = animator.GetComponentInChildren<PawAI>();

        // Start the coroutine for the Paw Slam attack
        if (boss != null && pawAI != null)
        {
            boss.StartSlammingAttack(OnSlamComplete);
        }
        else
        {
            Debug.LogError("Boss or PawAI component is not assigned.");
        }
    }
    // Callback method to be called when the claw attack is complete
    private void OnSlamComplete()
    {
        hasAttacked = true; // Set the flag to true to trigger state transition
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if the slam attack has been executed and transition to cooldown state
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
        animator.SetBool("Slam", false);
        animator.ResetTrigger("IsPaw");
        Debug.Log("Exited Paw Slam attack state");
    }
}

