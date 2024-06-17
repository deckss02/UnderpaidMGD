using System.Collections;
using UnityEngine;

public class PawSlamBehaviour : StateMachineBehaviour
{
    private Boss boss;
    private PawAI pawAI;

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
            boss.StartCoroutine(PawSlamAttack(animator));
        }
        else
        {
            Debug.LogError("Boss or PawAI component is not assigned.");
        }
    }

    // Coroutine for the Paw Slam attack
    private IEnumerator PawSlamAttack(Animator animator)
    {
        boss.attacking = true;

        // Move towards the player
        yield return boss.StartCoroutine(pawAI.MoveTowardsPlayer());

        // Perform the Paw Slam attack (add attack logic here)
        Debug.Log("Paw Slam attack started");
        yield return new WaitForSeconds(1f);
        Debug.Log("Paw Slam attack executed");

        // Reset all attack-related parameters
        boss.ResetAllAttacks();
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Perform ongoing actions during Paw Slam attack, if any
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.attacking = false;
        Debug.Log("Exited Paw Slam attack state");
        animator.SetBool("Slam", false);
        animator.ResetTrigger("IsPaw");
    }
}

