using UnityEngine;

public class ParasiteRat : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // Speed at which the enemy moves
    public GameObject[] wayPoints;

    int nextWaypoint = 1;
    float distToPoint;

    [SerializeField] private int damageAmount = 1; // Amount of damage to apply
    [SerializeField] private int maxHealth = 1; // Maximum health for the enemy
    private int currentHealth; // Current health of the enemy
    private Rigidbody2D enemyRigidbody; // Reference to the Rigidbody2D component
    public TimEC timEC;

    private SimpleFlash simpleFlash;
    private Animator myAnim;

    private bool isKilled = false; // Flag to check if the enemy is already counted as killed

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        myAnim = GetComponent<Animator>(); // Get and store a reference to the Animator component
        currentHealth = maxHealth;
        timEC = FindObjectOfType<TimEC>();
        simpleFlash = GetComponent<SimpleFlash>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        distToPoint = Vector2.Distance(transform.position, wayPoints[nextWaypoint].transform.position);

        transform.position = Vector2.MoveTowards(transform.position, wayPoints[nextWaypoint].transform.position, moveSpeed * Time.deltaTime);

        if (distToPoint < 0.3f)
        {
            TakeTurn();
        }
    }

    void TakeTurn()
    {
        Vector3 currRot = transform.eulerAngles;
        currRot.z += wayPoints[nextWaypoint].transform.eulerAngles.z;
        transform.eulerAngles = currRot;
        ChooseNextWaypoint();
    }

    void ChooseNextWaypoint()
    {
        nextWaypoint++;

        if (nextWaypoint == wayPoints.Length)
        {
            nextWaypoint = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collider is tagged as a Bullet and apply damage
        if (collision.collider.CompareTag("Bullet"))
        {
            TakeDamage(damageAmount); // Apply damage to the enemy
            Destroy(collision.collider.gameObject); // Destroy the bullet after hitting the enemy
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!isKilled)
        {
            isKilled = true;
            simpleFlash.Flash(); // Start the flash effect
            timEC.EnemyKilled();
            Destroy(gameObject, 0.3f);
        }
    }
}