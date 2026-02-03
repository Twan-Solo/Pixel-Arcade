using UnityEngine;

public class ModularDamageDealer : MonoBehaviour
{
    [Header("Hazard Settings (Hurt Player)")]
    public bool dealsDamageToPlayer = true;
    public int damageToPlayer = 10;

    [Header("Destructible Settings (Hurt This Object)")]
    public bool isDestructible = true;
    public int hitsToDestroy = 3;
    public string vulnerableToTag = "PlayerAttack";
    public bool destroyAttackerOnHit = true;

    private int currentHits = 0;

    private void OnTriggerEnter(Collider other)
    {
        // DEBUG: Fires if the physics engine sees a 3D touch
        Debug.Log($"<color=cyan>[Physics]</color> 3D Collision detected with: {other.gameObject.name}");
        HandleInteraction(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // DEBUG: Fires if the physics engine sees a 2D touch
        Debug.Log($"<color=cyan>[Physics]</color> 2D Collision detected with: {other.gameObject.name}");
        HandleInteraction(other.gameObject);
    }

    private void HandleInteraction(GameObject otherObj)
    {
        // 1. Check if we should hurt the Player
        if (dealsDamageToPlayer && otherObj.CompareTag("Player"))
        {
            Debug.Log("<color=green>[Success]</color> Player tag identified! Sending damage...");

            if (PlayerData.Instance != null)
            {
                PlayerData.Instance.TakeDamage(damageToPlayer);
            }
            else
            {
                Debug.LogError("<color=red>[Error]</color> PlayerData.Instance is NULL! Ensure the PlayerData script is on an object in your scene.");
            }
        }
        else if (dealsDamageToPlayer)
        {
            // DEBUG: This helps if you hit something but the tag is wrong
            Debug.Log($"<color=yellow>[Notice]</color> Hit {otherObj.name}, but it is NOT tagged as 'Player'. Current tag: {otherObj.tag}");
        }

        // 2. Check if the specific item type hit THIS object
        if (isDestructible && otherObj.CompareTag(vulnerableToTag))
        {
            Debug.Log($"<color=magenta>[Combat]</color> This object was hit by {vulnerableToTag}!");
            TakeHit();

            if (destroyAttackerOnHit)
            {
                Destroy(otherObj);
            }
        }
    }

    private void TakeHit()
    {
        currentHits++;
        if (currentHits >= hitsToDestroy)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"<color=red>[Destroyed]</color> {gameObject.name} has been destroyed!");
        Destroy(gameObject);
    }
}
