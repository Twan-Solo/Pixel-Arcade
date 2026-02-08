using StarterAssets;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    [Tooltip("Should the player face right on spawn?")]
    public bool facingRight = true;

    private void Start()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

        if (spawnPoint == null)
        {
            Debug.LogError("PlayerSpawn tag not found in scene!");
            return;
        }

        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned!");
            return;
        }

        // Spawn player
        GameObject player = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);

        // Reset orientation and flip sprite
        ThirdPersonController controller = player.GetComponent<ThirdPersonController>();

        if (controller != null)
        {
            controller.ResetOrientation(spawnPoint.transform.position, facingRight);
        }
        else
        {
            Debug.LogError("ThirdPersonController not found on player prefab!");
        }
    }
}