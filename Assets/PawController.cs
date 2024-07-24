using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawController : MonoBehaviour
{
    public BossHealth bossHealth;

    // Start is called before the first frame update
    void Start()
    {
        bossHealth = FindAnyObjectByType<BossHealth>();

    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth.TakeDamage()) { }
    }
}
