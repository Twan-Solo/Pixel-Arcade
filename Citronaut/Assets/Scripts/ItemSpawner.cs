using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    public GameObject itemPrefab;
    public bool spawnOnlyOnce = true;

    private bool hasSpawned = false;
    private Collider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();

        if (myCollider == null)
        {
            Debug.LogError("No Collider found on this object!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (spawnOnlyOnce && hasSpawned) return;
        if (itemPrefab == null || myCollider == null) return;

        // Top of THIS collider in world space
        Vector3 spawnPosition = new Vector3(
            myCollider.bounds.center.x,
            myCollider.bounds.max.y,
            myCollider.bounds.center.z
        );

        Instantiate(itemPrefab, spawnPosition, transform.rotation);
        hasSpawned = true;
    }
}
