using System.Collections;
using UnityEngine;

public class PawAI : MonoBehaviour
{
    public Transform target; // Target to move towards, usually the player
    public float speed = 200f; // Speed of movement
    public float nextWaypointDistance = 3f; // Distance threshold to consider "reaching" a waypoint
    public float stopDuration = 2f; // Time to stop before returning to the original position

    private Transform player; // Reference to the player's transform
    private Vector3 originalPosition; // Store the initial position to return to
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
        originalPosition = transform.position; // Store the original position
    }

    // Coroutine to start moving towards the player
    public IEnumerator MoveTowardsPlayer()
    {
        // Move towards the player
        while (Vector2.Distance(transform.position, player.position) > nextWaypointDistance)
        {
            // Move the paw towards the player position
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Stop for the specified duration
        yield return new WaitForSeconds(stopDuration);

        // Return to the original position
        while (Vector2.Distance(transform.position, originalPosition) > nextWaypointDistance)
        {
            // Move the paw back to the original position
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }
}
