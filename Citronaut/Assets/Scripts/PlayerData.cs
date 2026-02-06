using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public int score;
    public GameObject heldObjectPrefab;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Death Settings")]
    public string mainMenuSceneName = "MainMenu"; // scene to return to on death
    public bool destroyPlayerOnDeath = true;      // remove PlayerData on death

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentHealth = maxHealth;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ------------------------------
    // Health Management
    // ------------------------------

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        // Update UI
        if (HealthCounter.Instance != null)
            HealthCounter.Instance.UpdateHealthDisplay();

        // Check death
        if (currentHealth <= 0)
            HandleDeath();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (HealthCounter.Instance != null)
            HealthCounter.Instance.UpdateHealthDisplay();
    }

    // ------------------------------
    // Death Handling
    // ------------------------------

    private void HandleDeath()
    {
        Debug.Log("Player has died!");

        if (destroyPlayerOnDeath)
            Destroy(gameObject);

        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
    }

    // ------------------------------
    // Scene Management
    // ------------------------------

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Destroy PlayerData if we return to Main Menu to reset
        if (scene.name == "MainMenu" && destroyPlayerOnDeath)
        {
            Destroy(gameObject);
        }
    }
}
