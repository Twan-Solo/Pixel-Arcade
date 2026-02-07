using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Sync local score with the persistent PlayerData score
        if (PlayerData.Instance != null)
        {
            score = PlayerData.Instance.score;
        }
        UpdateScoreText();
    }
    public void AddScore(int amount)
    {
        score += amount;

        // Update the PlayerData script globally
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.score = score;
        }

        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        scoreText.text = " " + score;
    }
}
