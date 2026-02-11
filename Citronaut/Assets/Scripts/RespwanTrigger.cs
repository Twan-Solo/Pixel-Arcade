using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnTrigger : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 25; // how much health to remove each time

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (PlayerData.Instance == null)
            return;

        // Apply damage FIRST
        PlayerData.Instance.TakeDamage(damageAmount);

        int health = PlayerData.Instance.GetCurrentHealth();

        Debug.Log("Player health after damage: " + health);

        if (health > 0)
        {
            RespawnPlayer(other.gameObject);
        }
        else
        {
            SceneManager.LoadScene(PlayerData.Instance.mainMenuSceneName);
        }
    }

    private void RespawnPlayer(GameObject player)
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

        if (spawnPoint == null)
        {
            Debug.LogError("PlayerSpawn not found!");
            return;
        }

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.transform.position = spawnPoint.transform.position;

        if (controller != null)
            controller.enabled = true;

        Debug.Log("Player Respawned");
    }
}