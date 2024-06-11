using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMinionsBehaviour : StateMachineBehaviour
{

    private Boss boss; // Reference to the Boss script

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the Boss component from the animator's GameObject
        if (boss == null)
            boss = animator.GetComponent<Boss>();

        // Start the Summon Minions coroutine
        boss.StartCoroutine(SummonMinions(animator));
    }

    // Coroutine to handle summoning minions
    private IEnumerator SummonMinions(Animator animator)
    {
        boss.attacking = true;
        // Call the minion spawning logic
        boss.HandleMinionSpawning();
        yield return new WaitForSeconds(1f); // Example duration

        // After summoning minions, reset the attack state
        boss.ResetAllAttacks();
        Debug.Log("Minions summoned");
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Perform ongoing actions during Summon Minions, if any
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset triggers or perform cleanup on exiting the state
        boss.attacking = false;
        Debug.Log("Exited Summon Minions state");
    }
}
