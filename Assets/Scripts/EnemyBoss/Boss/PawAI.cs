using System.Collections;
using UnityEngine;

public class PawAI : MonoBehaviour
{
    public Transform target; // Target to move towards, usually the player
    public float circleRadius = 2.7f; // Radius for circular movement

    protected Transform player; // Reference to the player's transform
    protected Vector3 originalPosition; // Store the initial position to return to
    protected Rigidbody2D rb; // Reference to the Rigidbody2D component

    public float minRotationSpeed = 80.0f;
    public float maxRotationSpeed = 120.0f;
    public float minMovementSpeed = 1.75f;
    public float maxMovementSpeed = 2.25f;
    protected float rotationSpeed; // Degrees per second
    protected float movementSpeed; // Units per second

    protected bool isReturning = false; // Flag to check if the paw is returning to its original position
    protected bool isActive = false; // Flag to control activation of the paw

    protected virtual void Start()
    {
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        movementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Find the player by tag
        originalPosition = transform.position; // Store the original position

        // Start the movement coroutine
        StartCoroutine(MoveTowardsPlayer());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (player != null)
        {
            Gizmos.DrawWireSphere(player.position, circleRadius);
        }
    }

    // Coroutine to move towards the player
    protected virtual IEnumerator MoveTowardsPlayer()
    {
        while (true)
        {
            if (isActive && !isReturning)
            {
                // Move towards the player's position
                Vector3 targetPosition = player.position;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

                // Smoothly rotate towards the movement direction
                Vector3 direction = targetPosition - transform.position;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Continue moving
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
            StopCoroutine(MoveTowardsPlayer());
            StartCoroutine(ReturnToOriginalPositionCoroutine());
        }
    }

    // Coroutine to return to the original position
    protected IEnumerator ReturnToOriginalPositionCoroutine()
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

        // Resume moving towards the player if still active
        if (isActive)
        {
            StartCoroutine(MoveTowardsPlayer());
        }
    }

    // Method to activate the paw movement
    public void ActivatePaw()
    {
        isActive = true;
    }

    // Method to deactivate the paw movement
    public void DeactivatePaw()
    {
        isActive = false;
        ReturnToOriginalPosition();
    }
}
