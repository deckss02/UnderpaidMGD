using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set the isDead parameter to true
        animator.SetBool("isDead", true);

        Collider2D bossTimCollider = animator.GetComponent<Collider2D>();
        if (bossTimCollider != null)
        {
            bossTimCollider.enabled = false;
        }

        // Stop any ongoing attacks
        BossTim bossTim = animator.GetComponent<BossTim>();
        if (bossTim != null)
        {
            bossTim.attacking = false;
            bossTim.StopAllCoroutines();
        }

        // Ensure all actions are stopped
        BossHealthTim bossHealthTim = animator.GetComponent<BossHealthTim>();
        if (bossHealthTim != null)
        {
            bossHealthTim.StopAllActions();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if the current animation has finished playing
        if (stateInfo.normalizedTime >= 1.0f)
        {
            BossHealthTim bossHealthTim = animator.GetComponent<BossHealthTim>();
            if (bossHealthTim != null)
            {
                bossHealthTim.TriggerWinScreen();
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // You can add any cleanup logic here if needed
    }
}
