using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    public List<GameObject> currentOneWayPlatforms = new List<GameObject>(); // List to track all platforms the player is interacting with

    [SerializeField] private BoxCollider2D playerCollider;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Uncomment this if you want to use a button press for jumping down
        // if (Input.GetButtonDown("Down") || Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     if (currentOneWayPlatforms.Count > 0)
        //     {
        //         StartCoroutine(DisableCollision());
        //     }
        // }
    }

    public void JumpDown()
    {
        if (currentOneWayPlatforms.Count > 0)
        {
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatforms.Add(collision.gameObject); // Add the platform to the list
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatforms.Remove(collision.gameObject); // Remove the platform from the list
        }
    }

    private IEnumerator DisableCollision()
    {
        // Disable collision for all platforms in the list
        foreach (var platform in currentOneWayPlatforms)
        {
            BoxCollider2D platformCollider = platform.GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(playerCollider, platformCollider);
        }

        yield return new WaitForSeconds(1f);

        // Re-enable collision for all platforms in the list
        foreach (var platform in currentOneWayPlatforms)
        {
            BoxCollider2D platformCollider = platform.GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }
    }
}
