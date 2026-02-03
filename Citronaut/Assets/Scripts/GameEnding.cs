using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Required for Coroutines

public class GameEnding : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private float transitionDelay = 0.5f; // Adjust this delay as needed

    private bool m_IsEnding = false;

    void OnTriggerEnter(Collider other)
    {
        // Check if the player entered and that we aren't already ending
        if (other.CompareTag("Player") && !m_IsEnding)
        {
            m_IsEnding = true;
            StartCoroutine(DestroyAndLoadMenu());
        }
    }

    private IEnumerator DestroyAndLoadMenu()
    {
        // 1. Instantly hide the player so they 'disappear' for the user
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.gameObject.SetActive(false);

            // 2. Start the destruction process
            Destroy(PlayerData.Instance.gameObject);
            PlayerData.Instance = null; // Clear static reference immediately
        }

        // 3. Wait for the specified delay to ensure cleanup is finished
        yield return new WaitForSeconds(transitionDelay);

        // 4. Load the main menu
        SceneManager.LoadScene(mainMenuSceneName);
    }
}




