using UnityEngine;

public class PickupSpawn : MonoBehaviour
{
    public float lifetime = 5f;     // Auto-destroy after time
    public int scoreValue = 1;      // Score added when player collects
    public bool destroyOnEnemy = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScoreCounter.Instance != null)
                ScoreCounter.Instance.AddScore(scoreValue);

            Destroy(gameObject);
        }

        if (destroyOnEnemy && other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
