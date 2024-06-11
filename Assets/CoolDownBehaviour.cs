using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBehaviour : StateMachineBehaviour
{
    private Boss boss; // Reference to the Boss script
    private float cooldownTime = 5.0f; // Duration of the cooldown
    private float timer;

    // Array to store the names of the attack stages
    private string[] attackStages = { "HairBallRoll", "SummonMinions", "Claw", "PawSlam" };

    private string previousAttackStage;

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Initialize the boss reference and reset the timer
        boss = animator.GetComponent<Boss>();
        timer = cooldownTime;

        // Perform actions on entering Cooldown state, if any
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
            string nextAttackStage = GetRandomAttackStage();

            // Trigger transition to the selected attack stage
            animator.SetTrigger(nextAttackStage);

            // Store the selected attack stage as the previous one
            previousAttackStage = nextAttackStage;
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Perform actions on exiting Cooldown state, if any
        Debug.Log("Exited Cooldown state");

        // Reset the Cooldown parameter to false
        animator.SetBool("Cooldown", false);
    }

    // Get a random attack stage that is different from the previous one
    private string GetRandomAttackStage()
    {
        string randomAttackStage;
        do
        {
            randomAttackStage = attackStages[Random.Range(0, attackStages.Length)];
        } while (randomAttackStage == previousAttackStage);
        return randomAttackStage;
    }
}