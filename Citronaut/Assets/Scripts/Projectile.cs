using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 5f;

    private Vector3 moveDirection;
    // Set movement direction after spawning
    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
    void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
