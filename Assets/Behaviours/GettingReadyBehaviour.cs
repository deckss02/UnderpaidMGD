using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingReadyBehaviour : StateMachineBehaviour
{

    public float timer;
    public float minTime;
    public float maxTime;

    private Transform playerPos;
    public float speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the player's position by looking for the GameObject with the "Player" tag and getting its Transform component
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        // Set the timer to a random value between minTime and maxTime
        timer = Random.Range(minTime, maxTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if the timer has expired
        if (timer <= 0)
        {
            // Trigger a transition to the "Idle" state
            animator.SetTrigger("Idle");
        }
        else
        {
            // Decrease the timer by the time passed since the last frame
            timer -= Time.deltaTime;
        }

        // Calculate the target position, maintaining the current y-position of the animator's transform
        Vector2 target = new Vector2(playerPos.position.x, animator.transform.position.y);
        // Move the animator's transform towards the target position at the specified speed
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, target, speed * Time.deltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
