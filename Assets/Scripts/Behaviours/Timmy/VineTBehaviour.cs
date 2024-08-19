using System.Collections;
using UnityEngine;

public class VineTBehaviour : StateMachineBehaviour
{
    private BossTim bossTim; // Reference to the Boss script

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the Boss component from the animator's GameObject
        if (bossTim == null)
            bossTim = animator.GetComponent<BossTim>();

        // Start the Paw Slam attack
        if (bossTim != null)
        {
            bossTim.StartVineTeleportAttack(OnTeleportComplete); // Pass a callback to handle completion
        }
        else
        {
            Debug.LogError("Boss component is not assigned.");
        }
    }

    // Callback method to be called when the slam attack is complete
    private void OnTeleportComplete()
    {
        // The animator will handle state transition using the "CoolDown" parameter
        bossTim.myAnim.SetBool("CoolDown", true);
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
        animator.SetBool("CoolDown", true);
        animator.SetBool("GroundVine", false);
        Debug.Log("Exited VineTeleport attack state");
    }
}
