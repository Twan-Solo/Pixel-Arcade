using UnityEngine;

public class Blaster : MonoBehaviour
{
    public GameObject projectilePrefab;  // Assign your Projectile prefab in Inspector
    public Transform firePoint;          // Where projectiles spawn on the player

    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        // Check if the player touched the Blaster and hasn't collected it yet
        if (!collected && other.CompareTag("Player"))
        {
            collected = true;

            // Give the player the ability to shoot using FireProjectile
            FireProjectile fireScript = other.GetComponent<FireProjectile>();
            if (fireScript != null)
            {
                fireScript.EnableShooting(projectilePrefab, firePoint);
            }

            Destroy(gameObject); // Remove the Blaster from the scene
        }
    }
}
