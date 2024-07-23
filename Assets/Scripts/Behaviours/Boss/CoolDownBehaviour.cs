using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownBehaviour : StateMachineBehaviour
{
    private Boss boss; // Reference to the Boss script
    private BossHealth bossHealth; // Reference to the BossHealth script
    private float cooldownTime = 5.0f; // Duration of the cooldown
    private float timer;
    private int stagesCompleted = 0; // Counter to track completed stages

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Initialize references, reset the timer
        boss = animator.GetComponent<Boss>();
        bossHealth = animator.GetComponent<BossHealth>();
        timer = cooldownTime;

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
            // Get the next attack stage based on stagesCompleted
            string nextAttackStage = GetNextAttackStage();

            // Trigger transition to the selected attack stage
            switch (nextAttackStage)
            {
                case "SummonMinions":
                    SummonMinionsPicked(animator);
                    break;
                case "Paw8":
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

    // Get the next attack stage based on stagesCompleted
    private string GetNextAttackStage()
    {
        string[] attackStages = { "SummonMinions", "Paw8", "Claw", "HairBallRoll" };
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

    private void PawSlamPicked(Animator animator)
    {
        ResetAttackBools(animator);
        animator.SetBool("Paw8", true);
        animator.SetTrigger("IsPaw");
        animator.SetBool("CoolDown", false);
    }

    private void ClawPicked(Animator animator)
    {
        ResetAttackBools(animator);
        animator.SetBool("Claw", true);
        animator.SetBool("CoolDown", false);
    }

    private void HairBallRollPicked(Animator animator)
    {
        ResetAttackBools(animator);
        animator.SetBool("HairBall", true);
        animator.SetBool("CoolDown", false);
    }

    // Reset all attack bools
    private void ResetAttackBools(Animator animator)
    {
        animator.SetBool("Summon", false);
        animator.SetBool("Paw8", false);
        animator.SetBool("Claw", false);
        animator.SetBool("HairBall", false);
    }
}
