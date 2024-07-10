using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairBall : MonoBehaviour
{
    public float minSpeed = 3f; // Minimum speed of the hairball
    public float maxSpeed = 7f; // Maximum speed of the hairball
    public float maxDistance = 10f; // Maximum distance the hairball can travel
    public float lifetime = 3f; // Lifetime of the hairball

    private float speed; // Speed of the hairball
    private float distanceTravelled = 0f;
    private Vector3 startPosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        // Randomize the speed
        speed = Random.Range(minSpeed, maxSpeed);

        // Start the coroutine to destroy the hairball after its lifetime ends
        StartCoroutine(DestroyHairBallAfterLifetime());
    }

    void Update()
    {
        // Move the hairball forward
        Vector2 movement = new Vector2(speed * Time.deltaTime, 0f);
        rb.MovePosition(rb.position + movement);

        // Calculate the distance travelled
        distanceTravelled = Vector3.Distance(startPosition, transform.position);

        // Rotate the hairball 360 degrees
        transform.Rotate(0, 0, 360 * Time.deltaTime);

        // Destroy the hairball if it has traveled the maximum distance
        if (distanceTravelled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyHairBallAfterLifetime()
    {
        // Wait for the specified lifetime
        yield return new WaitForSeconds(lifetime);

        // Destroy the hairball
        Destroy(gameObject);
    }
}

