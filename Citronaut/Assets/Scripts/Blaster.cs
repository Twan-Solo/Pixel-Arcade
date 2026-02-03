using UnityEngine;

public class Blaster : MonoBehaviour
{
    public GameObject projectilePrefab;
    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        // 1. Only trigger if it's the player and we haven't picked it up yet
        if (!collected && other.CompareTag("Player"))
        {
            FireProjectile fireScript = other.GetComponent<FireProjectile>();

            if (fireScript != null)
            {
                // FIX: Match your hierarchy name "FirePoint" exactly!
                Transform playerMuzzle = other.transform.Find("FirePoint");

                if (playerMuzzle != null)
                {
                    collected = true;
                    Debug.Log("<color=green>[Success]</color> Found FirePoint! Projectiles will spawn at: " + playerMuzzle.position);

                    // Hand off the prefab and the PLAYER'S persistent FirePoint
                    fireScript.EnableShooting(projectilePrefab, playerMuzzle);

                    // Remove the pickup from the world
                    Destroy(gameObject);
                }
                else
                {
                    // If this shows up, double-check that FirePoint isn't inside a 'Model' or 'Graphics' child
                    Debug.LogError("<color=red>[Error]</color> Could not find 'FirePoint' as a direct child of " + other.name);
                }
            }
        }
    }
}