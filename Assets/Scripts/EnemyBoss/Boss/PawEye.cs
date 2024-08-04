using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawEye : PawAI
{
    public float stopDuration = 10f; // Duration to pause before spawning a claw

    public GameObject paw;


    protected override void Start()
    {
        base.Start(); // Call the base class Start method
        // Any additional initialization for PawMouth
        StartCoroutine(MoveInCircle());
    }

    // Coroutine to move in a circle around the player
    private IEnumerator MoveInCircle()
    {
        float angle = 0f;
        while (true)
        {
            if (isActive && !isReturning)
            {
                // Custom movement logic for PawMouth
                float x = player.position.x + Mathf.Cos(angle) * circleRadius;
                float y = player.position.y + Mathf.Sin(angle) * circleRadius;
                Vector3 targetPosition = new Vector3(x, y, 0);
                paw.transform.rotation = Quaternion.identity;

                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

                Vector3 direction = targetPosition - transform.position;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                angle += movementSpeed * Time.deltaTime;
                if (angle >= 360f) angle -= 360f;

                yield return null; // Wait for the next frame
            }
            else
            {
                yield return null; // Wait for the next frame
            }
        }
    }
}
