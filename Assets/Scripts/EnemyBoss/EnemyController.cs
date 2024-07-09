using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // Speed at which the enemy moves

    public Transform pointA; // Transform of point A
    public Transform pointB; // Transform of point B
    private Transform currentPoint; // Current target point
    public float specificXPointA = -5.35f; // Specific X coordinate for point A
    public float specificXPointB = 0.62f; // Specific X coordinate for point B
    public Color gizmoColor = Color.blue; // Color of the gizmos

    private Rigidbody2D enemyRigidbody; // Reference to the Rigidbody2D component
    public EnemyCounter theEnemyCounter; // Reference to the EnemyCounter script
    public EnemyHealth enemyHealth; // Reference to the EnemyHealth script

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        theEnemyCounter = FindObjectOfType<EnemyCounter>(); // Find the EnemyCounter component in the scene
        enemyHealth = GetComponent<EnemyHealth>(); // Get the EnemyHealth component attached to the enemy

        if (pointA == null || pointB == null)
        {
            Debug.LogError("pointA or pointB is not assigned in the Inspector!");
        }

        currentPoint = pointB; // Start with pointB as initial point

        if (enemyRigidbody == null)
        {
            Debug.LogError("Rigidbody2D component is not found on the enemy GameObject!");
        }
    }


    void Update()
    {
        MoveBetweenPoints();
    }

    private void MoveBetweenPoints()
    {
        if (currentPoint == null || enemyRigidbody == null)
        {
            Debug.LogError("currentPoint or enemyRigidbody is null. Make sure they are properly assigned!");
            return;
        }

        Vector2 moveDirection = (currentPoint.position - transform.position).normalized;
        enemyRigidbody.velocity = moveDirection * moveSpeed;

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
        {
            if (currentPoint == pointA)
            {
                currentPoint = pointB;
            }
            else
            {
                currentPoint = pointA;
            }
            Flip(); // Flip direction
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(pointA.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.position, 0.5f);
        Gizmos.DrawLine(pointA.position, pointB.position);
    }

    // Called when the enemy takes damage
    public void TakeDamage(int amount)
    {
        // Reduce health using the EnemyHealth component
        enemyHealth.TakeDamage(amount);

        // Check if the enemy is destroyed (health <= 0) and handle accordingly
        if (enemyHealth.currentHealth <= 0)
        {
            Die(); // Destroy the enemy
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // Destroy the enemy game object
    private void Die()
    {
        Destroy(gameObject); // Destroy the enemy game object
    }
}
