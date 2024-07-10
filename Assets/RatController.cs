using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : MonoBehaviour
{
    public event System.Action OnDeath;

    public void Die()
    {
        // Trigger death logic
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
