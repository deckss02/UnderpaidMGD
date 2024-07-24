using System.Collections;
using UnityEngine;

public class PawSlamBehaviour : StateMachineBehaviour
{
    private Boss boss; // Reference to the Boss script

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the Boss component from the animator's GameObject
        if (boss == null)
            boss = animator.GetComponent<Boss>();

        // Start the Paw Slam attack
        if (boss != null)
        {
            boss.StartSlammingAttack(OnSlamComplete); // Pass a callback to handle completion
        }
        else
        {
            Debug.LogError("Boss component is not assigned.");
        }
    }

    // Callback method to be called when the slam attack is complete
    private void OnSlamComplete()
    {
        // The animator will handle state transition using the "CoolDown" parameter
        boss.myAnim.SetBool("CoolDown", true);
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // No additional logic needed for each frame
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset flags for the next time this state is entered
        animator.SetBool("Cooldown", true);
        animator.SetBool("Paw8", false);
        animator.ResetTrigger("IsPaw");
        Debug.Log("Exited Paw Slam attack state");
    }
}
