using UnityEngine;

public class Claw : MonoBehaviour
{
    public float speed = 10.0f;

    // Method to set the direction and start moving the claw
    public void Shoot(Vector3 direction)
    {
        // Assuming you're using Rigidbody2D for movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on Claw.");
        }
    }
}

