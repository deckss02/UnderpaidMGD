using UnityEngine;

public class Seed : MonoBehaviour
{
    public GameObject groundVinesPrefab; // Prefab for the Ground Vines to spawn

    void Start()
    {
        // Drop ground vines at the seed's position
        Instantiate(groundVinesPrefab, transform.position, Quaternion.identity);

        // Destroy the seed after dropping ground vines
        Destroy(gameObject);
    }
}
