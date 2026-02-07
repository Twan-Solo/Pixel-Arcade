using UnityEngine;
using UnityEngine.InputSystem;

public class FireProjectile : MonoBehaviour
{
    private GameObject projectilePrefab;
    private Transform firePoint;
    private bool canShoot = false;

    [Header("Shooting Settings")]
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

        Debug.Log($"<color=cyan>[Setup]</color> Shooting enabled at: {point.position}");
    }

    void Update()
    {
        if (!canShoot || fireAction == null) return;

        if (fireAction.triggered && Time.time >= lastShotTime + shootCooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("Projectile Prefab or FirePoint is missing!");
            return;
        }

        // Spawn projectile at fire point
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        // X-AXIS ONLY DIRECTION (LEFT / RIGHT)
        Vector3 direction = firePoint.right;
        direction.y = 0f;
        direction.z = 0f;
        direction.Normalize();

        if (projectile.TryGetComponent<Projectile>(out Projectile proj))
        {
            proj.SetDirection(direction);
        }
        else
        {
            Debug.LogWarning("Projectile prefab is missing a Projectile script!");
        }
    }
}