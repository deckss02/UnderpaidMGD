using UnityEngine;

public class ShootingFlower : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; // Amount of damage to apply
    [SerializeField] private int maxHealth = 1; // Maximum health for the enemy
    private int currentHealth; // Current health of the enemy
    private Rigidbody2D enemyRigidbody; // Reference to the Rigidbody2D component
    public TimEC timEC;


    private Transform player;
    public GameObject AcidBullet;
    public GameObject BulletPos;
    public float fireRate = 1.0f;
    private float nextFireTime;

    private SimpleFlash simpleFlash;
    private bool isKilled = false;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        timEC = FindObjectOfType<TimEC>();
        simpleFlash = GetComponent<SimpleFlash>();
    }

    void Update()
    {
        if (Time.time > nextFireTime)
        {
            Instantiate(AcidBullet, BulletPos.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collider is tagged as a Bullet and apply damage
        if (collision.collider.CompareTag("Bullet"))
        {
            Debug.Log($"Bullet hit enemy. Applying {damageAmount} damage.");
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
