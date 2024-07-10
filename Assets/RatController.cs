using UnityEngine;

public class RatController : MonoBehaviour
{
    public int health = 1;
    public delegate void RatDeath();
    public event RatDeath OnDeath;

    // Method to take damage
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to handle the rat's death
    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
