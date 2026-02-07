using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 5f, -7f);
    public float smoothSpeed = 5f;

    private Quaternion fixedRotation = Quaternion.Euler(0f, 0f, 0f);

    void LateUpdate()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p == null) return;
            player = p.transform;
        }

        // Only follow player on X axis
        Vector3 desiredPosition = transform.position;
        desiredPosition.x = player.position.x + offset.x;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.rotation = fixedRotation;
    }
}