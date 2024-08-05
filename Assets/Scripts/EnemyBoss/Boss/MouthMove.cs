using System.Collections;
using UnityEngine;

public class MouthMove : MonoBehaviour
{
    public float circleDuration = 6f; // Duration to circle around the player before moving
    public float moveDuration = 0.9f; // Duration to move towards the player
    public float backOffDistance = 4f; // Distance to back off from the player
    public Transform player; // Reference to the player's transform
    public float circleRadius = 2.7f; // Radius for circular movement
    public float movementSpeed = 1.5f; // Speed of movement

    private bool isActive = false; // Flag to control activation of the paw
    private Quaternion fixedRotation; // Variable to store the fixed rotation

    private void Start()
    {
        // Set the fixed rotation to the current rotation of the object at the start
        fixedRotation = transform.rotation;
    }

    public void StartMovingPaw()
    {
        isActive = true;
        StartCoroutine(PawBehaviorRoutine());
    }

    public void StopMovingPaw()
    {
        isActive = false;
        StopAllCoroutines();
        // Ensure the position and rotation are fixed when stopping
        transform.position = transform.position;
        transform.rotation = fixedRotation;
    }

    private IEnumerator PawBehaviorRoutine()
    {
        while (isActive)
        {
            yield return StartCoroutine(CircleAroundPlayer(circleDuration));
            yield return StartCoroutine(MoveTowardsPlayerForDuration(moveDuration));
            yield return new WaitForSeconds(1f); // Pause for the specified stop duration
            yield return StartCoroutine(BackOffFromPlayer(backOffDistance));
        }
    }

    private IEnumerator CircleAroundPlayer(float duration)
    {
        float angle = 0f;
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            if (isActive)
            {
                float x = player.position.x + Mathf.Cos(angle) * circleRadius;
                float y = player.position.y + Mathf.Sin(angle) * circleRadius;
                Vector3 targetPosition = new Vector3(x, y, 0);

                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                transform.rotation = fixedRotation; // Lock the rotation

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
            if (isActive)
            {
                Vector3 targetPosition = player.position;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                transform.rotation = fixedRotation; // Lock the rotation

                yield return null; // Wait for the next frame
            }
            else
            {
                yield return null; // Wait for the next frame
            }
        }
    }

    private IEnumerator BackOffFromPlayer(float distance)
    {
        Vector3 originalDirection = (transform.position - player.position).normalized;
        float distanceTraveled = 0f;
        while (distanceTraveled < distance)
        {
            if (isActive)
            {
                Vector3 targetPosition = transform.position + originalDirection * movementSpeed * Time.deltaTime;
                distanceTraveled += Vector3.Distance(transform.position, targetPosition);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                transform.rotation = fixedRotation; // Lock the rotation

                yield return null; // Wait for the next frame
            }
            else
            {
                yield return null; // Wait for the next frame
            }
        }
    }
}
