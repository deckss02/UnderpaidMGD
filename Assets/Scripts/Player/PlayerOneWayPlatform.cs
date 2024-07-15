using System.Collections;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    public GameObject currentOneWayPlatform;
   
    [SerializeField] private BoxCollider2D playerCollider;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }


    private void Update()
    {
      //if (Input.GetButtonDown("Down")|| Input.GetKeyDown(KeyCode.DownArrow))
      //{
      //    if (currentOneWayPlatform != null)
      //    {
      //        StartCoroutine(DisableCollision());
      //    }
      //}
    }

    public void JumpDown()
    {
        if (currentOneWayPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    public IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
