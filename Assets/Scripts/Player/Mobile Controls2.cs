using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControls2 : MonoBehaviour
{
    private Vector2 startTouchPosition, endTouchPosition;

    public bool Attack;
    public float movespeed;


    private Touch touch;

    private IEnumerator goCoroutine;
    private bool CoroutineAllowed;


    // Start is called before the first frame update
    void Start()
    {
        CoroutineAllowed = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
        }

        if (touch.phase == TouchPhase.Began)
        {
            startTouchPosition = touch.position;
        }

        if (Input.touchCount > 0 && touch.phase == TouchPhase.Ended && CoroutineAllowed)
        {
            Attack = false; // When the Player stands still, he will not attack colliding enemies.
            endTouchPosition = touch.position;

            if ((endTouchPosition.y > startTouchPosition.y) && (Mathf.Abs(touch.deltaPosition.y) > Mathf.Abs(touch.deltaPosition.x)))
            {
                Attack = true; // When he moves in any direction, he starts attacking.
                goCoroutine = go(new Vector3(0f, 0.25f, 0f));
                StartCoroutine(goCoroutine);
            }
            //Move Up
            else if ((endTouchPosition.y <  startTouchPosition.y) && (Mathf.Abs(touch.deltaPosition.y) > Mathf.Abs(touch.deltaPosition.x)))
            {
                Attack = true;
                goCoroutine = go(new Vector3(0f, -0.25f, 0f));
                StartCoroutine(goCoroutine);
            }
            //Move Down
            else if ((endTouchPosition.x < startTouchPosition.x) && (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y)))
            {
                Attack = true;
                goCoroutine = go(new Vector3(-0.25f, 0f, 0f));
                StartCoroutine(goCoroutine);
            }
            // Move Left
            else if ((endTouchPosition.x > startTouchPosition.x) && (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y)))
            {
                Attack = true;
                goCoroutine = go(new Vector3(0.25f, 0f, 0f));
                StartCoroutine(goCoroutine);
            }
            // Move Right
        }
    }


    private IEnumerator go(Vector3 direction) 
    {
        CoroutineAllowed = false;

        for (int i = 0; i < 2; i++) 
        { 
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, movespeed * Time.deltaTime);
            transform.Translate(direction);
            yield return new WaitForSeconds(0.01f);
        }

        transform.Translate(direction);

        CoroutineAllowed = true;

    }
}
