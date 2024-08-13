using UnityEngine;
using UnityEngine.UI;

public class Ultimate1 : MonoBehaviour
{
    public BossHealthTim bossHealth; // Reference to the BossHealthTim script
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
        if (bossHealth.currentHealth <= 40f && !ultimateReady)
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
            ultimateInstance = Instantiate(ultimatePrefab, bossHealth.transform.position, Quaternion.identity);

            // Initialize the UltimateEffect1 component if it exists
            UltimateEffect1 ultimateEffect = ultimateInstance.GetComponent<UltimateEffect1>();
            if (ultimateEffect != null)
            {
                ultimateEffect.Initialize(bossHealth, ultimateDamage);
                ultimateEffect.ActivateUltimateEffect();
            }

            // Reset the ultimate
            ultimateReady = false;
            ultimateButton.interactable = false; // Optionally disable the button
        }
    }
}
