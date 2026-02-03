using StarterAssets;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

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

        GameObject player = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);

        // Tell the controller to fully reset orientation
        ThirdPersonController controller =
            player.GetComponent<ThirdPersonController>();

        if (controller != null)
        {
            controller.ResetOrientation(spawnPoint.transform);
        }
        else
        {
            Debug.LogError("ThirdPersonController not found on player prefab!");
        }
    }
}