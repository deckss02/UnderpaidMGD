using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Animator cubeAnimator;
    private PlayerControllera playerController;

    void Start()
    {
        cubeAnimator = GetComponent<Animator>();  // Get the Animator component of the Cube
        playerController = FindObjectOfType<PlayerControllera>();  // Find the PlayerController script in the scene
    }

    void Update()
    {
        if (playerController != null)
        {
            bool isPlayerMoving = playerController.IsMoving;  // Check if the player is moving

            // Set the "Move" parameter in the Animator based on the player's movement
            cubeAnimator.SetBool("Move", isPlayerMoving);
        }
    }
}
