using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ultimate : MonoBehaviour
{
    public BossHealth bossHealth; // Reference to the BossHealth script
    public GameObject ultimatePrefab; // Prefab for the ultimate visual effect
    public float ultimateDamage = 50f; // Damage dealt by the ultimate

    private bool ultimateReady = false; // Flag to check if the ultimate is ready
    private Transform buttonTransform; // Transform of the Ultimate button
    private Button ultimateButton; // Reference to the Button component

    void Start()
    {
        // Cache the button's transform and Button component
        buttonTransform = transform;
        ultimateButton = GetComponent<Button>();

        // Optionally, you can disable the ultimate button initially
        ultimateButton.interactable = false;
    }

    void Update()
    {
        // Check the boss's health
        if (bossHealth.currentHealth <= 40f && !ultimateReady)
        {
            // Enable the ultimate button
            ultimateButton.interactable = true;
            ultimateReady = true;
        }
    }

    // Function to use the ultimate ability
    public void UseUltimate()
    {
        Debug.Log("Ultimate Playing!!!!");
        if (ultimateReady)
        {
            // Instantiate the ultimate visual effect at the button's position
            Instantiate(ultimatePrefab, buttonTransform.position, Quaternion.identity);

            // Deal damage to the boss
            bossHealth.TriggerUltimateDamage(ultimateDamage);

            // Reset the ultimate
            ultimateReady = false;

            // Disable the ultimate button permanently
            ultimateButton.interactable = false;
            ultimateButton.gameObject.SetActive(false); // Optionally disable the entire button GameObject
        }
    }
}

