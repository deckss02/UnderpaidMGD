using UnityEngine;
using UnityEngine.UI;

public class Ultimate1 : MonoBehaviour
{
    public GameObject bossGameObject; // Reference to the Boss GameObject
    public GameObject ultimatePrefab; // Prefab for the ultimate visual effect
    public float ultimateDamage = 50f; // Damage dealt by the ultimate
    public Button ultimateButton; // Reference to the Button component

    private Animator bossAnimator; // Reference to the Boss's Animator component
    private GameObject ultimateInstance; // Reference to the instantiated ultimate visual effect

    void Start()
    {
        // Get the Animator component from the boss GameObject
        if (bossGameObject != null)
        {
            bossAnimator = bossGameObject.GetComponent<Animator>();
        }

        // Ensure the ultimate button is enabled at the start
        ultimateButton.interactable = true;

        // Add listener to the button's onClick event
        ultimateButton.onClick.AddListener(UseUltimate);
    }

    // Function to use the ultimate ability
    public void UseUltimate()
    {
        if (bossGameObject != null)
        {
            // Instantiate the ultimate visual effect at the boss's position
            ultimateInstance = Instantiate(ultimatePrefab, bossGameObject.transform.position, Quaternion.identity);

            // Initialize the UltimateEffect1 component if it exists
            UltimateEffect1 ultimateEffect = ultimateInstance.GetComponent<UltimateEffect1>();
            if (ultimateEffect != null)
            {
                BossHealthTim bossHealth = bossGameObject.GetComponent<BossHealthTim>();
                ultimateEffect.Initialize(bossHealth, ultimateDamage);
                ultimateEffect.ActivateUltimateEffect();
            }

            // Trigger the boss's death animation
            if (bossAnimator != null)
            {
                bossAnimator.SetBool("isDead", true);
            }

            // Optionally disable the button after use or keep it enabled for multiple uses
            ultimateButton.interactable = false;
        }
    }
}
