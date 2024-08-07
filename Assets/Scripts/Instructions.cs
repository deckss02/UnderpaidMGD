using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    [SerializeField] private GameObject instructionPanel; // Reference to the instruction panel
    [SerializeField] private Button closeButton; // Reference to the close button

    void Start()
    {
        // Ensure the instruction panel is active at the start and pause the game
        ShowInstructions();

        // Add listener to the close button
        closeButton.onClick.AddListener(HideInstructions);
    }

    private void ShowInstructions()
    {
        instructionPanel.SetActive(true); // Show the instruction panel
        Time.timeScale = 0f; // Pause the game
    }

    private void HideInstructions()
    {
        instructionPanel.SetActive(false); // Hide the instruction panel
        Time.timeScale = 1f; // Resume the game
    }
}
