using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform LeftPoint; // Reference to the left point position
    public Transform RightPoint; //Reference to the right point position

    public float moveSpeed;//How fast the enemy can move 

    private Rigidbody2D enemyRigidbody; //Reference to the Rigidbody2D component of the Enemy

    public bool movingRight;//Check if the enemy should move left or right

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
        if (movingRight && (transform.position.x > RightPoint.position.x)) // if the enemy is moving right and it has gone past the right point, it should start to move left
        {
            movingRight = false;
            Flip();
        }

        if (!movingRight && (transform.position.x < LeftPoint.position.x)) // if the enemy is moving left and it has gone past the left point, it should start to move right
        {
            movingRight = true;
            Flip();
        }
        if (movingRight)
        {
            enemyRigidbody.velocity = new Vector2(moveSpeed, enemyRigidbody.velocity.y);
        }
        else
        {
            enemyRigidbody.velocity = new Vector2(-moveSpeed, enemyRigidbody.velocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            theEnemyCounter.EnemyKilled(); // Call EnemyKilled without arguments

        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

}