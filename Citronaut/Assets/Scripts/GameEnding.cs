using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameEnding : MonoBehaviour
{
    [SerializeField] private string creditsSceneName = "Credits";
    [SerializeField] private float transitionDelay = 0.5f;

    private bool m_IsEnding = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !m_IsEnding)
        {
            m_IsEnding = true;
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        // Hide & destroy player
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.gameObject.SetActive(false);
            Destroy(PlayerData.Instance.gameObject);
            PlayerData.Instance = null;
        }

        yield return new WaitForSeconds(transitionDelay);

        // Load Credits scene instead of Main Menu
        SceneManager.LoadScene(creditsSceneName);
    }
}




