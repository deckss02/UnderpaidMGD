using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set the isDead parameter to true
        animator.SetBool("isDead", true);

        Collider2D bossCollider = animator.GetComponent<Collider2D>();
        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }

        // Stop any ongoing attacks
        Boss boss = animator.GetComponent<Boss>();
        if (boss != null)
        {
            boss.attacking = false;
            boss.StopAllCoroutines();
        }

        // Ensure all actions are stopped
        BossHealth bossHealth = animator.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.StopAllActions();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if the current animation has finished playing
        if (stateInfo.normalizedTime >= 1.0f)
        {
            BossHealth bossHealth = animator.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TriggerWinScreen();
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // You can add any cleanup logic here if needed
    }
}
