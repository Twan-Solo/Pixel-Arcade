using UnityEngine;

public class Health : MonoBehaviour
{
    public float lifetime = 5f;  // Health destroys itself after 5 seconds
    public int healthValue = 1;   // Score added when player collects
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HealthCounter.Instance != null)
                HealthCounter.Instance.AddHealth(healthValue);

            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
