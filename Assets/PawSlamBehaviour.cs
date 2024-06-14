using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawSlamBehaviour : StateMachineBehaviour
{
    private Boss boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss == null)
            boss = animator.GetComponent<Boss>();

        boss.StartCoroutine(PawSlamAttack(animator));
    }

    private IEnumerator PawSlamAttack(Animator animator)
    {
        boss.attacking = true;
        // Add Paw Slam attack logic here
        yield return new WaitForSeconds(1f);

        boss.ResetAllAttacks();
        Debug.Log("PawSlam attack executed");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Perform ongoing actions during Paw Slam attack, if any
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.attacking = false;
        Debug.Log("Exited Paw Slam attack state");
        animator.SetBool("Slam", false);
    }
}
