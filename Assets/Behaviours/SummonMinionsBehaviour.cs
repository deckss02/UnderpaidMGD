using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMinionsBehaviour : StateMachineBehaviour
{
    private Boss boss;
    private bool hasSummoned;

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss == null)
        {
            boss = animator.GetComponent<Boss>();
        }

        if (boss != null)
        {
            boss.StartSummoningAttack(OnSummonComplete);
        }
        else
        {
            Debug.LogError("Boss component is not assigned.");
        }

        hasSummoned = false;
    }

    // Callback method to be called when the summon attack is complete
    private void OnSummonComplete()
    {
        hasSummoned = true;
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if all minions are dead after summoning is complete
        if (hasSummoned && boss.AreAllMinionsDead())
        {
            boss.GoToCooldown(); // Transition to cooldown
        }
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasSummoned = false;
    }
}

