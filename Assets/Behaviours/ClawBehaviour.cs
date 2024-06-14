using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawBehaviour : StateMachineBehaviour
{
    private Boss boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss == null)
            boss = animator.GetComponent<Boss>();

        boss.StartCoroutine(ClawAttack(animator));
    }

    private IEnumerator ClawAttack(Animator animator)
    {
        boss.attacking = true;
        // Add Claw attack logic here
        yield return new WaitForSeconds(1f);

        boss.ResetAllAttacks();
        Debug.Log("Claw attack executed");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Perform ongoing actions during Claw attack, if any
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.attacking = false;
        Debug.Log("Exited Claw attack state");
        animator.SetBool("Claw", false);
    }
}
