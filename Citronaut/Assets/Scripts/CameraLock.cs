using UnityEngine;

public class LockMainCamera2D : MonoBehaviour
{
    void LateUpdate()
    {
        // Force camera to stay on one side
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}