using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordUser : MonoBehaviour
{
    public GameObject ClaymorePrefab; // Reference to the Claymore weapon prefab
    public GameObject SwordPrefab;    // Reference to the Sword weapon prefab

    public Transform ClaymorePosition; // Reference to the position where the weapon should be placed
    public Transform SwordPosition; // Reference to the position where the weapon should be placed

    private GameObject currentWeapon; // Reference to the current active weapon
    private GameObject claymoreInstance; // Instance of the Claymore weapon
    private GameObject swordInstance; // Instance of the Sword weapon

    public SlashManager slashManager; // Reference to the SlashManager

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate both weapons and set them as inactive
        claymoreInstance = Instantiate(ClaymorePrefab, ClaymorePosition.position, ClaymorePosition.rotation);
        claymoreInstance.transform.SetParent(ClaymorePosition);
        claymoreInstance.SetActive(false);

        swordInstance = Instantiate(SwordPrefab, SwordPosition.position, SwordPosition.rotation);
        swordInstance.transform.SetParent(SwordPosition);
        swordInstance.SetActive(false);

        // Equip the Claymore by default
        EquipWeapon(claymoreInstance);
        if (slashManager != null)
        {
            slashManager.SetCurrentWeaponType("Claymore");
        }
        else
        {
            Debug.LogError("SlashManager is not assigned.");
        }
    }

    public void SwitchToClaymore()
    {
        Debug.Log("SwitchToClaymore called");
        EquipWeapon(claymoreInstance);
        if (slashManager != null)
        {
            slashManager.SetCurrentWeaponType("Claymore");
        }
    }

    public void SwitchToSword()
    {
        Debug.Log("SwitchToSword called");
        EquipWeapon(swordInstance);
        if (slashManager != null)
        {
            slashManager.SetCurrentWeaponType("Sword");
        }
    }

    void EquipWeapon(GameObject weapon)
    {
        if (currentWeapon != null)
        {
            Debug.Log("Deactivating current weapon: " + currentWeapon.name);
            currentWeapon.SetActive(false); // Deactivate the current weapon
        }

        currentWeapon = weapon;
        Debug.Log("Activating new weapon: " + currentWeapon.name);
        currentWeapon.SetActive(true); // Activate the new weapon
    }

    public void TriggerWeaponAttack()
    {
        if (currentWeapon != null)
        {
            Animator weaponAnimator = currentWeapon.GetComponent<Animator>();
            if (weaponAnimator != null)
            {
                Debug.Log("Triggering attack animation on weapon: " + currentWeapon.name);
                weaponAnimator.SetTrigger("AttackDown");
            }
            else
            {
                Debug.LogError("Animator not found on the current weapon.");
            }
        }
    }
}
