using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyController : MonoBehaviour
{
    
    public GameObject bulletToRight1;
    public GameObject bulletToLeft1; //Game Object will be instantiated when hit the fire button
    private Vector2 bulletPos; //Coordinates where the bullet should be instantiated
    public float fireRate;
    private float nextFire;
    private bool facingRight = true;
    private Rigidbody2D rb;
    public Transform LeftPoint; // Reference to the left point position
    public Transform RightPoint; //Reference to the right point position
    private float timer;
    private Rigidbody2D enemyRigidbody; //Reference to the Rigidbody2D component of the Enemy
    public bool movingRight;//Check if the enemy should move left or right

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); //Get Access to the Rigidbody2D component of the Enemy
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;  
        nextFire = timer + fireRate;
        if(timer > 2)
        {
            timer = 0;
            Fire();
        }
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.tag.Equals ("Bullet"))
        {
            Destroy (col.gameObject);
            Destroy (gameObject);
        }
    }
    void Fire()
    {
       bulletPos = transform.position; //Set the position of the bullet to be player position
       if (facingRight)
       {
           bulletPos += new Vector2(+1f, 0f);
           Instantiate(bulletToRight1, bulletPos, Quaternion.identity);
       }
    }
}
