using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 5f;

    private Vector3 moveDirection;

    public void SetDirection(Vector3 direction)
    {
        // Ensure we have a direction, otherwise default to forward
        moveDirection = direction == Vector3.zero ? transform.forward : direction.normalized;
    }

    void Start()
    {
        // Clean up the projectile after a few seconds
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile forward in world space
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    // Use OnTriggerEnter to work with your ModularDamageDealer
    private void OnTriggerEnter(Collider other)
    {
        // We let the ModularDamageDealer handle hurting the enemy.
        // We only need to check if we hit something that ISN'T the player.
        if (!other.CompareTag("Player"))
        {
            // If the enemy script is set to 'destroyAttackerOnHit', 
            // it will handle destroying this projectile. 
            // Otherwise, we destroy it here on impact with walls/ground.
            Destroy(gameObject);
        }
    }
}
