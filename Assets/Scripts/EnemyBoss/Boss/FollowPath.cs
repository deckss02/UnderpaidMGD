using System.Collections;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private Transform[] waypointsPattern1;
    [SerializeField] private Transform[] waypointsPattern2;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float switchInterval = 10f; // Time in seconds to switch waypoint patterns
    [SerializeField] private float fadeDuration = 1f; // Duration of the fade in/out effect
    [SerializeField] private bool useFading = true; // Flag to control fading

    private Transform[] currentWaypoints;
    private int waypointIndex = 0;
    private bool isActive = true; // Flag to control movement
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentWaypoints = waypointsPattern1; // Set the initial waypoint pattern
        transform.position = currentWaypoints[waypointIndex].transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        StartCoroutine(SwitchPatternAfterTime()); // Start the coroutine to switch patterns
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
        if (waypointIndex <= currentWaypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentWaypoints[waypointIndex].transform.position, moveSpeed * Time.deltaTime);

            if (transform.position == currentWaypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;

                // Reset to loop back to the first waypoint
                if (waypointIndex >= currentWaypoints.Length)
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

    // Method to switch to a different waypoint pattern
    public void SwitchWaypointPattern(int pattern)
    {
        if (pattern == 1)
        {
            currentWaypoints = waypointsPattern1;
        }
        else if (pattern == 2)
        {
            currentWaypoints = waypointsPattern2;
        }

        waypointIndex = 0; // Reset the waypoint index
        transform.position = currentWaypoints[waypointIndex].transform.position; // Set the position to the first waypoint of the new pattern
    }

    // Coroutine to switch patterns after a certain time
    private IEnumerator SwitchPatternAfterTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchInterval); // Wait for the specified interval

            if (useFading)
            {
                // Fade out, switch pattern, then fade in
                yield return StartCoroutine(FadeOut());
            }

            if (currentWaypoints == waypointsPattern1)
            {
                SwitchWaypointPattern(2);
            }
            else
            {
                SwitchWaypointPattern(1);
            }

            if (useFading)
            {
                yield return StartCoroutine(FadeIn());
            }
        }
    }

    // Coroutine to fade out the sprite
    private IEnumerator FadeOut()
    {
        float startAlpha = spriteRenderer.color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0, time / fadeDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
    }

    // Coroutine to fade in the sprite
    private IEnumerator FadeIn()
    {
        float startAlpha = spriteRenderer.color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 1, time / fadeDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
    }

    // Method to set the fading flag
    public void SetUseFading(bool useFading)
    {
        this.useFading = useFading;
    }
}
