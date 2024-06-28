using System.Collections;
using UnityEngine;

public class PawAI : MonoBehaviour
{
    public Transform target; // Target to move around, usually the player
    public float stopDuration = 10f; // Time to stop before returning to the original position
    public float circleRadius = 3f; // Radius of the circular path

    private Transform player; // Reference to the player's transform
    private Vector3 originalPosition; // Store the initial position to return to
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    public float minRotationSpeed = 80.0f;
    public float maxRotationSpeed = 120.0f;
    public float minMovementSpeed = 1.75f;
    public float maxMovementSpeed = 2.25f;
    private float rotationSpeed; // Degrees per second
    private float movementSpeed; // Units per second

    private bool isReturning = false; // Flag to check if the paw is returning to its original position

    void Start()
    {
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        movementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Find the player by tag
        originalPosition = transform.position; // Store the original position

        // Start the movement coroutine
        StartCoroutine(MoveInCircle());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (player != null)
        {
            Gizmos.DrawWireSphere(player.position, circleRadius);
        }
    }

    // Coroutine to start moving in a circle around the player
    public IEnumerator MoveInCircle()
    {
        float angle = 0f;

        while (true)
        {
            if (!isReturning)
            {
                // Calculate the new position in a circle around the player
                float x = player.position.x + Mathf.Cos(angle) * circleRadius;
                float y = player.position.y + Mathf.Sin(angle) * circleRadius;
                Vector3 targetPosition = new Vector3(x, y, 0);

                // Move towards the target position
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

                // Smoothly rotate towards the movement direction
                Vector3 direction = targetPosition - transform.position;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Update the angle to continue moving in a circle
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

    // Method to start returning to the original position
    public void ReturnToOriginalPosition()
    {
        if (!isReturning)
        {
            isReturning = true;
            StopCoroutine(MoveInCircle());
            StartCoroutine(ReturnToOriginalPositionCoroutine());
        }
    }

    // Coroutine to return to the original position
    private IEnumerator ReturnToOriginalPositionCoroutine()
    {
        while (Vector2.Distance(transform.position, originalPosition) > 0.1f)
        {
            // Move the paw back to the original position
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, movementSpeed * Time.deltaTime);

            // Smoothly rotate towards the movement direction
            Vector3 direction = originalPosition - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            yield return null; // Wait for the next frame
        }
        isReturning = false; // Reset the flag once the paw reaches the original position

        // Resume moving in a circle
        StartCoroutine(MoveInCircle());
    }
}