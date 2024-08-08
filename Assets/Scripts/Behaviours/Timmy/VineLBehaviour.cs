using UnityEngine;

public class VineLBehaviour : StateMachineBehaviour
{
    private BossTim bossTim; // Reference to the Boss script
    private bool hasAttacked = false; // Flag to track whether the hairball attack has been executed
    private float cooldownDelay = 2f; // Delay before setting CoolDown to true
    private float cooldownTimer = 0f; // Timer to track cooldown delay

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the Boss component from the animator's GameObject
        if (bossTim == null)
            bossTim = animator.GetComponent<BossTim>();

        // Start the HairBallRoll attack with a callback to this behaviour
        if (bossTim != null)
        {
            bossTim.StartVineLaneAttack(OnVineLaneComplete);
        }
        else
        {
            Debug.LogError("Boss component is not assigned.");
        }
    }

    // Callback method to be called when the hairball attack is complete
    private void OnVineLaneComplete()
    {
        hasAttacked = true; // Set the flag to true to trigger state transition
        cooldownTimer = cooldownDelay; // Start the cooldown timer
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasAttacked)
        {
            // Decrease the cooldown timer
            cooldownTimer -= Time.deltaTime;

            // Check if the cooldown delay has passed
            if (cooldownTimer <= 0)
            {
                animator.SetBool("CoolDown", true);
                hasAttacked = false; // Reset the flag for the next time this state is entered
            }
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the flag and timer for the next time this state is entered
        hasAttacked = false;
        cooldownTimer = 0f;
        animator.SetBool("VineL", false);
    }
}
