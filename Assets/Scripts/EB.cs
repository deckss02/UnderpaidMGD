using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EB : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D myRigidbody;


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if(IsFacingRight())
        {
            myRigidbody.velocity = new Vector2(moveSpeed, 0f);//Move Right
        }
        else
        {
            myRigidbody.velocity = new Vector2(-moveSpeed, 0f);//Move Left
        }
    }
    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        //Turn
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), transform.localScale.y);
    }
}
