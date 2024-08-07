using System.Collections;
using UnityEngine;

public class FollowPath1 : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 2f;

    private int waypointIndex = 0;
    private bool isActive = true; // Flag to control movement
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    private void Update()
    {
        if (isActive)
        {
            Move();
        }
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, moveSpeed * Time.deltaTime);

            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;

                // Reset to loop back to the first waypoint
                if (waypointIndex >= waypoints.Length)
                {
                    waypointIndex = 0;
                }
            }
        }
    }

    // Method to activate/deactivate the movement
    public void SetActive(bool active)
    {
        isActive = active;
    }
}
