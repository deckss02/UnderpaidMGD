using UnityEngine;

public class CoolDownBehaviour : StateMachineBehaviour
{
    private Boss boss; // Reference to the Boss script
    private BossHealth bossHealth; // Reference to the BossHealth script
    private float cooldownTime = 5.0f; // Duration of the cooldown
    private float timer;
    private AttackStageManager attackStageManager; // Reference to the AttackStageManager

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Initialize references, reset the timer, and get AttackStageManager
        boss = animator.GetComponent<Boss>();
        bossHealth = animator.GetComponent<BossHealth>();
        timer = cooldownTime;
        attackStageManager = FindObjectOfType<AttackStageManager>(); // Find the AttackStageManager in the scene

        // Enable the weak point collider and disable invincibility
        if (bossHealth != null)
        {
            bossHealth.EnableWeakPoints(true);
            bossHealth.DisableInvincibility();
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
            // Get the next attack stage from AttackStageManager
            string nextAttackStage = attackStageManager.GetNextAttackStage();

            // Trigger transition to the selected attack stage
            switch (nextAttackStage)
            {
                case "SummonMinions":
                    SummonMinionsPicked(animator);
                    break;
                case "PawSlam":
                    PawSlamPicked(animator);
                    break;
                case "Claw":
                    ClawPicked(animator);
                    break;
                case "HairBallRoll":
                    HairBallRollPicked(animator);
                    break;
                default:
                    Debug.LogError("Invalid attack stage: " + nextAttackStage);
                    break;
            }

            // Reset the NextStage parameter to false before setting it to true later
            animator.SetBool("NextStage", false);

            // Set the NextStage boolean to indicate readiness to change state
            animator.SetBool("NextStage", true);

            // Reset the timer
            timer = cooldownTime;
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Perform actions on exiting Cooldown state, if any
        Debug.Log("Exited Cooldown state");

        // Disable the weak point collider and enable invincibility
        if (bossHealth != null)
        {
            bossHealth.EnableWeakPoints(false);
            bossHealth.EnableInvincibility();
        }

    }

    // Methods to activate specific attack stages
    private void SummonMinionsPicked(Animator animator)
    {
        animator.SetBool("Summon", true);
        // Reset other attack stage Bools if needed
        animator.SetBool("Slam", false);
        animator.SetBool("Claw", false);
        animator.SetBool("HairBall", false);
        animator.SetBool("CoolDown", false);
    }

    private void PawSlamPicked(Animator animator)
    {
        animator.SetBool("Slam", true);
        animator.SetTrigger("IsPaw");
        // Reset other attack stage Bools if needed
        animator.SetBool("Summon", false);
        animator.SetBool("Claw", false);
        animator.SetBool("HairBall", false);
        animator.SetBool("CoolDown", false);
    }

    private void ClawPicked(Animator animator)
    {
        animator.SetBool("Claw", true);
        // Reset other attack stage Bools if needed
        animator.SetBool("Summon", false);
        animator.SetBool("Slam", false);
        animator.SetBool("HairBall", false);
        animator.SetBool("CoolDown", false);
    }

    private void HairBallRollPicked(Animator animator)
    {
        animator.SetBool("HairBall", true);
        // Reset other attack stage Bools if needed
        animator.SetBool("Summon", false);
        animator.SetBool("Slam", false);
        animator.SetBool("Claw", false);
        animator.SetBool("CoolDown", false);
    }
}