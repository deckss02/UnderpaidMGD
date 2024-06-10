using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D enemyRigidbody;

    public EnemyCounter theEnemyCounter;
    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); //Get Access to the Rigidbody2D component of the Enemy
        theEnemyCounter = FindObjectOfType<EnemyCounter>();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);//Move Right
        }
        else
        {
            enemyRigidbody.velocity = new Vector2(-moveSpeed, 0f);//Move Left
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Turn
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)), transform.localScale.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            theEnemyCounter.EnemyKilled(); // Call EnemyKilled without arguments

        }
    }

}