using UnityEngine;
using UnityEngine.InputSystem;

public class FireProjectile : MonoBehaviour
{
    private GameObject projectilePrefab;
    private Transform firePoint;
    private bool canShoot = false;

    public float shootCooldown = 0.5f;
    private float lastShotTime;

    // InputAction reference
    private PlayerInput playerInput;
    private InputAction fireAction;


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            fireAction = playerInput.actions["Fire"]; // Make sure you have a "Fire" action in your Input Actions
        }
    }

    public void EnableShooting(GameObject projectile, Transform point)
    {
        projectilePrefab = projectile;
        firePoint = point;
        canShoot = true;
    }

    void Update()
    {
        if (!canShoot || fireAction == null) return;

        if (fireAction.triggered && Time.time > lastShotTime + shootCooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = transform.forward;
        projectile.GetComponent<Projectile>().SetDirection(direction);
    }
}
