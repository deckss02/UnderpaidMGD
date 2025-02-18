using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 moveDirection = new Vector2(1f, 0.25f);
    [SerializeField] GameObject rightCheck, roofCheck, groundCheck;
    [SerializeField] Vector2 rightCheckSize, roofCheckSize, groundCheckSize;
    [SerializeField] LayerMask FlyingBound; // Removed groundLayer

    [SerializeField] bool goingUp = true;

    private bool touchedGround, touchedRoof, touchedRight;
    private Rigidbody2D EnemyRB;

    void Start()
    {
        EnemyRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HitLogic();
    }

    void FixedUpdate()
    {
        EnemyRB.velocity = moveDirection * moveSpeed;
    }

    void HitLogic()
    {
        touchedRight = HitDetector(rightCheck, rightCheckSize, FlyingBound); // Removed groundLayer reference
        touchedRoof = HitDetector(roofCheck, roofCheckSize, FlyingBound); // Removed groundLayer reference
        touchedGround = HitDetector(groundCheck, groundCheckSize, FlyingBound); // Removed groundLayer reference

        if (touchedRight)
        {
            Flip();
        }
        if (touchedRoof && goingUp)
        {
            ChangeYDirection();
        }
        if (touchedGround && !goingUp)
        {
            ChangeYDirection();
        }
    }

    bool HitDetector(GameObject gameObject, Vector2 size, LayerMask layer)
    {
        return Physics2D.OverlapBox(gameObject.transform.position, size, 0f, layer);
    }

    void ChangeYDirection()
    {
        moveDirection.y = -moveDirection.y;
        goingUp = !goingUp;
    }

    void Flip()
    {
        transform.Rotate(new Vector2(0, 180));
        moveDirection.x = -moveDirection.x;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheck.transform.position, groundCheckSize);
        Gizmos.DrawWireCube(roofCheck.transform.position, roofCheckSize);
        Gizmos.DrawWireCube(rightCheck.transform.position, rightCheckSize);
    }
}
