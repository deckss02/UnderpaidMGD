using UnityEngine;

public class CoolDownTimBehaviour : StateMachineBehaviour
{
    private BossTim bossTim; // Reference to the Boss script
    private BossHealthTim bossHealthTim; // Reference to the BossHealth script
    private float cooldownTime = 5.0f; // Duration of the cooldown
    private float timer;
    private int stagesCompleted = 0; // Counter to track completed stages

    private float resetDelay = 2.0f; // Delay before resetting state
    private float resetTime; // Time at which the state should be reset

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Initialize references, reset the timer
        bossTim = animator.GetComponent<BossTim>();
        bossHealthTim = animator.GetComponent<BossHealthTim>();
        timer = cooldownTime;
        resetTime = Time.time + resetDelay; // Set the reset time

        // Enable the weak point collider and disable invincibility
        if (bossHealthTim != null)
        {
            bossHealthTim.EnableWeakPoints(true);
            bossHealthTim.DisableInvincibility();
        }

        Debug.Log("Entered Cooldown state");
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Decrease the timer
        timer -= Time.deltaTime;

        // Check if the cooldown has finished
        if (timer <= 0)
        {
            // Get the next attack stage based on stagesCompleted
            string nextAttackStage = GetNextAttackStage();

            // Trigger transition to the selected attack stage
            switch (nextAttackStage)
            {
                case "SummonMinions":
                    SummonMinionsPicked(animator);
                    break;
                case "VineTeleport":
                    VineTeleportPicked(animator);
                    break;
                case "VineLane":
                    VineLanePicked(animator);
                    break;
                default:
                    Debug.LogError("Invalid attack stage: " + nextAttackStage);
                    break;
            }

            // Reset the timer
            timer = cooldownTime;
        }

        // Check if it's time to reset the cooldown state
        if (Time.time >= resetTime)
        {
            ResetState();
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Perform actions on exiting Cooldown state, if any
        Debug.Log("Exited Cooldown state");
    }

    // Function to reset state after a delay
    private void ResetState()
    {
        // Reset invincibility and weak points
        if (bossHealthTim != null)
        {             
            bossHealthTim.EnableWeakPoints(false);
            bossHealthTim.EnableInvincibility();
        }
    }

    // Get the next attack stage based on stagesCompleted
    private string GetNextAttackStage()
    {
        string[] attackStages = { "VineLane", "VineTeleport", "SummonMinions" };
        int index = stagesCompleted % attackStages.Length; // Loop through stages
        stagesCompleted++; // Increment the counter
        return attackStages[index];
    }

    // Methods to activate specific attack stages
    private void SummonMinionsPicked(Animator animator)
    {
        ResetAttackBools(animator);
        animator.SetBool("Summon", true);
        animator.SetBool("CoolDown", false);

    }

    private void VineTeleportPicked(Animator animator)
    {
        ResetAttackBools(animator);
        animator.SetBool("GroundVine", true);
        animator.SetBool("CoolDown", false);
    }

    private void VineLanePicked(Animator animator)
    {
        ResetAttackBools(animator);
        animator.SetBool("VineL", true);
        animator.SetBool("CoolDown", false);
    }

    // Reset all attack bools
    private void ResetAttackBools(Animator animator)
    {
        animator.SetBool("VineL", false);
        animator.SetBool("GroundVine", false);
        animator.SetBool("Summon", false);
    }
}
