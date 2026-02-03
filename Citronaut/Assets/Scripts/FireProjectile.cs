using UnityEngine;
using UnityEngine.InputSystem;

public class FireProjectile : MonoBehaviour
{
    private GameObject projectilePrefab;
    private Transform firePoint;
    private bool canShoot = false;

    public float shootCooldown = 0.5f;
    private float lastShotTime;

    private PlayerInput playerInput;
    private InputAction fireAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            fireAction = playerInput.actions["Fire"];
        }
    }

    public void EnableShooting(GameObject projectile, Transform point)
    {
        projectilePrefab = projectile;
        firePoint = point;
        canShoot = true;

        // DEBUG: If 'point' is the Player object, this explains the ground spawning.
        Debug.Log($"<color=cyan>[Setup]</color> Shooting enabled. Point Name: {point.name} | Position: {point.position}");
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
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("Shoot called but Projectile Prefab or FirePoint is null!");
            return;
        }

        // BREADCRUMB: If the Y value in the console is 0, your firePoint is on the floor.
        Debug.Log($"<color=orange>[Shoot]</color> Spawning at Y: {firePoint.position.y} (Object: {firePoint.name})");

        // 1. Instantiate using the EXACT world position and rotation of the firePoint
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // 2. Use the firePoint's blue arrow (forward) for movement direction
        Vector3 direction = firePoint.forward;

        if (projectile.TryGetComponent<Projectile>(out Projectile projScript))
        {
            projScript.SetDirection(direction);
        }
    }
}
