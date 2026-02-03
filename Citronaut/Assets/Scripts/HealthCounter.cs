using UnityEngine;
using TMPro;

public class HealthCounter : MonoBehaviour
{
    // Fix: Re-add the Instance so other scripts can find this UI component
    public static HealthCounter Instance;

    public TextMeshProUGUI HealthText;

    private void Awake()
    {
        // Setup the singleton for the UI
        if (Instance == null) Instance = this;
        Instance = this;

    }

    private void Start()
    {
        UpdateHealthDisplay();
    }

    // This allows you to call HealthCounter.Instance.UpdateHealthDisplay() from anywhere
    public void UpdateHealthDisplay()
    {
        if (PlayerData.Instance != null && HealthText != null)
        {
            HealthText.text = "Health: " + PlayerData.Instance.GetCurrentHealth();
        }
    }

    // If your code uses .AddHealth(amount), this will now update the REAL health in PlayerData
    public void AddHealth(int amount)
    {
        if (PlayerData.Instance != null)
        {
            if (amount > 0) PlayerData.Instance.Heal(amount);
            else PlayerData.Instance.TakeDamage(Mathf.Abs(amount));

            UpdateHealthDisplay();
        }
    }
}