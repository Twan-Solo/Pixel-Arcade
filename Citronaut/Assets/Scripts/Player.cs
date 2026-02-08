using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private static Player instance;
    private CharacterController controller;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            controller = GetComponent<CharacterController>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

        if (spawnPoint == null)
        {
            Debug.LogWarning("No PlayerSpawn found in this scene!");
            return;
        }

        // Disable controller before moving
        if (controller != null)
            controller.enabled = false;

        // Teleport to spawn
        transform.position = spawnPoint.transform.position;

        // IMPORTANT for 2D sprites: DO NOT rotate on Y
        // Keep rotation flat
        transform.rotation = Quaternion.identity;

        // Optional: reset velocity-like state
        Physics.SyncTransforms();

        // Re-enable controller
        if (controller != null)
            controller.enabled = true;
    }
}