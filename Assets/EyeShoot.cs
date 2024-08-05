using System.Collections;
using UnityEngine;

public class EyeShoot : MonoBehaviour
{
    public GameObject pawPrefab; // Prefab of the paw to shoot
    public Transform player; // Reference to the player's transform
    public float shootDuration = 5f; // Duration to keep shooting
    public float shootInterval = 1f; // Interval between each shot
    public float pawSpeed = 10f; // Speed of the paw

    private bool isShooting = false; // Flag to control the shooting behavior

    private void OnEnable()
    {
        // Automatically start shooting when the script is enabled
        StartShooting();
    }

    private void OnDisable()
    {
        // Stop shooting when the script is disabled
        StopShooting();
    }

    // Start the shooting behavior
    public void StartShooting()
    {
        if (!isShooting)
        {
            isShooting = true;
            StartCoroutine(ShootPawRoutine());
        }
    }

    // Stop the shooting behavior
    public void StopShooting()
    {
        isShooting = false;
        StopAllCoroutines();
    }

    // Coroutine to handle the shooting behavior
    private IEnumerator ShootPawRoutine()
    {
        float startTime = Time.time;
        while (Time.time - startTime < shootDuration)
        {
            if (isShooting)
            {
                ShootPaw();
                yield return new WaitForSeconds(shootInterval);
            }
            else
            {
                yield return null; // Wait for the next frame
            }
        }

        isShooting = false; // Ensure the shooting stops after the duration
    }

    // Method to shoot a paw in the direction of the player
    private void ShootPaw()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        GameObject paw = Instantiate(pawPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = paw.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * pawSpeed;
        }
        else
        {
            Debug.LogWarning("The pawPrefab does not have a Rigidbody2D component.");
        }
    }
}
