using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    private Boss boss;

    void Start()
    {
        boss = FindObjectOfType<Boss>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "WeakPoint")
        {
            boss.GetComponent<BossHealth>().TakeDamage(20);
        }
    }
}
