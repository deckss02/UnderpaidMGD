using System.Collections;
using UnityEngine;

public class HealBehaviour : StateMachineBehaviour
{
    public GameObject healingEffectPrefab; // Reference to the healing effect prefab
    private GameObject healingEffectInstance; // Instance of the healing effect prefab

    // Called when the state starts evaluating
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Instantiate the healing effect prefab at the player's position
        if (healingEffectPrefab != null)
        {
            Transform playerTransform = animator.transform;
            healingEffectInstance = Instantiate(healingEffectPrefab, playerTransform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Healing Effect Prefab is not assigned.");
        }
    }

    // Called on each Update frame while the state is being evaluated
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Any update logic can go here if needed
    }

    // Called when the state stops being evaluated
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Hide the healing effect instance when exiting the state
        if (healingEffectInstance != null)
        {
            // Deactivate the healing effect instance
            healingEffectInstance.SetActive(false);
            // Destroy the healing effect instance after a brief delay
            Destroy(healingEffectInstance, 0.5f);
        }
        animator.SetBool("IsHealing", false); // Reset the IsHealing bool to ensure the animation does not loop
    }
}