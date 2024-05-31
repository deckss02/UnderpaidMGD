using UnityEngine;

public class TakeDamageBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var bossHealth = animator.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamageAnimationStart();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var bossHealth = animator.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamageAnimationEnd();
        }
    }
}
