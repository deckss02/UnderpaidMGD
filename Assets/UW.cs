using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UW : MonoBehaviour
{
    public BossHealthWolf bossHealthW; // Reference to the BossHealthTim script
    public GameObject ultimatePrefab; // Prefab for the ultimate visual effect
    public float ultimateDamage = 50f; // Damage dealt by the ultimate
    public Button ultimateButton; // Reference to the Button component

    private bool ultimateReady = false; // Flag to check if the ultimate is ready
    private GameObject ultimateInstance; // Reference to the instantiated ultimate visual effect
    void Start()
    {
        // Optionally, you can disable the ultimate button initially
        ultimateButton.interactable = false;

        // Add listener to the button's onClick event
        ultimateButton.onClick.AddListener(UseUltimate);
    }

    void Update()
    {
        // Check the boss's health and enable the ultimate button if needed
        if (bossHealthW.currentHealth <= 40f && !ultimateReady)
        {
            ultimateButton.interactable = true;
            ultimateReady = true;
        }
    }

    // Function to use the ultimate ability
    public void UseUltimate()
    {
        if (ultimateReady)
        {
            // Instantiate the ultimate visual effect at the boss's position
            ultimateInstance = Instantiate(ultimatePrefab, bossHealthW.transform.position, Quaternion.identity);

            // Initialize the UltimateEffect1 component if it exists
          // UltimateEffectWolf ultimateEffectW = ultimateInstance.GetComponent<UltimateEffectWolf>();
          // if (ultimateEffectW != null)
          // {
          //     ultimateEffectW.Initialize(bossHealthW, ultimateDamage);
          //     ultimateEffectW.ActivateUltimateEffect();
          // }

            // Reset the ultimate
            ultimateReady = false;
            ultimateButton.interactable = false; // Optionally disable the button
        }
    }

}
