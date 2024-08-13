using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBullet : MonoBehaviour
{
    private Transform player;
    public float speed;
    private bool hasCollided = false;

    public GameObject idleObject; // Reference to the idle GameObject
    public GameObject explodeObjectPrefab; // Reference to the explosion prefab

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        idleObject.SetActive(true); // Ensure the idle GameObject is active at the start
    }

    void Update()
    {
        if (!hasCollided)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasCollided = true;
            TriggerExplosion();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            hasCollided = true;
            TriggerExplosion();
        }
    }

    private void TriggerExplosion()
    {
        // Instantiate the explosion effect at the current position of the AcidBullet
        if (explodeObjectPrefab != null)
        {
            Instantiate(explodeObjectPrefab, transform.position, Quaternion.identity);
        }

        // Deactivate the idle object if needed
        idleObject.SetActive(false);

        // Destroy the AcidBullet game object after a short delay
        Destroy(gameObject, 1.2f); // Adjust the delay as needed
    }
}
