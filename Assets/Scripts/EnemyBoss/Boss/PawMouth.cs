using System.Collections;
using UnityEngine;

public class PawMouth : PawAI
{
    public float stopDuration = 3f; // Duration to pause before spawning a claw
    public float circleDuration = 2f; // Duration to circle around the player before moving
    public float moveDuration = 5f; // Duration to move towards the player
    public float backOffDuration = 2f; // Duration to back off from the player

    protected override void Start()
    {
        base.Start(); // Call the base class Start method
        // Start the behavior coroutine
        StartCoroutine(PawBehaviorRoutine());
    }

    private IEnumerator PawBehaviorRoutine()
    {
        while (true)
        {
            if (isActive && !isReturning)
            {
                // Circle around the player for a specified duration
                yield return StartCoroutine(CircleAroundPlayer(circleDuration));

                // Move towards the player for a specified duration
                yield return StartCoroutine(MoveTowardsPlayerForDuration(moveDuration));

                // Pause for the specified stop duration
                yield return new WaitForSeconds(stopDuration);

                // Back off from the player for a specified duration
                yield return StartCoroutine(BackOffFromPlayer(backOffDuration));

                // Return to the original position
                ReturnToOriginalPosition();
            }
            else
            {
                yield return null; // Wait for the next frame
            }
        }
    }

    private IEnumerator CircleAroundPlayer(float duration)
    {
        float angle = 0f;
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            if (isActive && !isReturning)
            {
                float x = player.position.x + Mathf.Cos(angle) * circleRadius;
                float y = player.position.y + Mathf.Sin(angle) * circleRadius;
                Vector3 targetPosition = new Vector3(x, y, 0);

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

    private IEnumerator MoveTowardsPlayerForDuration(float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            if (isActive && !isReturning)
            {
                Vector3 targetPosition = player.position;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

                Vector3 direction = targetPosition - transform.position;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                yield return null; // Wait for the next frame
            }
            else
            {
                yield return null; // Wait for the next frame
            }
        }
    }

    private IEnumerator BackOffFromPlayer(float duration)
    {
        Vector3 originalDirection = (transform.position - player.position).normalized;
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            if (isActive && !isReturning)
            {
                Vector3 targetPosition = transform.position + originalDirection * movementSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

                Vector3 direction = targetPosition - transform.position;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                yield return null; // Wait for the next frame
            }
            else
            {
                yield return null; // Wait for the next frame
            }
        }
    }

    // Method to set the stop duration externally
    public void SetStopDuration(float duration)
    {
        stopDuration = duration;
    }
}
