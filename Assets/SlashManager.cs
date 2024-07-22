using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashManager : MonoBehaviour
{
    public RuntimeAnimatorController SwordSlash; // Animation controller for the sword slash
    public RuntimeAnimatorController ClaymoreSlash; // Animation controller for the claymore slash

    public Transform slashPosition; // Reference to the Transform of the slash position

    private Animator slashAnimator; // Reference to the Animator component on the slash position

    private string currentWeaponType; // Track the current weapon type

    private SwordUser swordUser; // Reference to the SwordUser component

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component from the slashPosition
        slashAnimator = slashPosition.GetComponent<Animator>();

        // Set the initial animator controller to ClaymoreSlash by default
        currentWeaponType = "Claymore";
        if (slashAnimator != null && ClaymoreSlash != null)
        {
            slashAnimator.runtimeAnimatorController = ClaymoreSlash;
        }
        else
        {
            Debug.LogError("Animator or ClaymoreSlash is not assigned.");
        }

        // Get the SwordUser component from the parent or other relevant GameObject
        swordUser = GetComponentInParent<SwordUser>();
        if (swordUser == null)
        {
            Debug.LogError("SwordUser component not found in parent.");
        }
    }

    // Method to handle attack based on the current weapon type
    public void OnAttackButtonClick()
    {
        Check(currentWeaponType); // Switch animator controller based on weapon type

        // Trigger the attack animation
        if (slashAnimator != null)
        {
            slashAnimator.SetTrigger("Attack"); // Ensure "Attack" is a trigger parameter in your Animator
        }
        else
        {
            Debug.LogError("Animator is not assigned.");
        }

        // Trigger the weapon-specific attack animation
        if (swordUser != null)
        {
            swordUser.TriggerWeaponAttack();
        }
        else
        {
            Debug.LogError("SwordUser is not assigned.");
        }

    }

    // Method to switch the animation controller based on weapon type
    public void Check(string weaponType)
    {
        if (slashAnimator == null)
        {
            Debug.LogError("Animator is not assigned.");
            return;
        }

        if (weaponType == "Sword")
        {
            Debug.Log("Switching to Sword animation.");
            if (SwordSlash != null)
            {
                slashAnimator.runtimeAnimatorController = SwordSlash;
            }
            else
            {
                Debug.LogError("SwordSlash Animator Controller is not assigned.");
            }
        }
        else if (weaponType == "Claymore")
        {
            Debug.Log("Switching to Claymore animation.");
            if (ClaymoreSlash != null)
            {
                slashAnimator.runtimeAnimatorController = ClaymoreSlash;
            }
            else
            {
                Debug.LogError("ClaymoreSlash Animator Controller is not assigned.");
            }
        }
        else
        {
            Debug.LogWarning("Unknown weapon type: " + weaponType);
        }
    }

    // Method to update the current weapon type (to be called by other scripts or UI)
    public void SetCurrentWeaponType(string weaponType)
    {
        currentWeaponType = weaponType;
        Check(currentWeaponType); // Immediately update the animator controller
    }
}
