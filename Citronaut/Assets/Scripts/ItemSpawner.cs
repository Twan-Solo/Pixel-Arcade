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

        // 1. Spawn item temporarily above
        Vector3 tempSpawnPos = myCollider.bounds.center + Vector3.up * 5f;
        GameObject item = Instantiate(itemPrefab, tempSpawnPos, Quaternion.identity);

        // 2. Get the REAL collider from the spawned item
        Collider itemCollider = item.GetComponent<Collider>();

        if (itemCollider != null)
        {
            float itemHalfHeight = itemCollider.bounds.extents.y;

            // 3. Place item exactly on top
            Vector3 finalPosition = new Vector3(
                myCollider.bounds.center.x,
                myCollider.bounds.max.y + itemHalfHeight,
                myCollider.bounds.center.z
            );

            item.transform.position = finalPosition;
        }
        else
        {
            Debug.LogWarning("Spawned item has no collider!");
        }

        hasSpawned = true;
    }
}
