using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 5f, -7f);
    public float smoothSpeed = 5f;

    // Fixed rotation for 2.5D (change 90 to -90 if needed)
    private Quaternion fixedRotation = Quaternion.Euler(0f,0f, 0f);

    private void LateUpdate()
    {
        // Find player if needed
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p == null) return;
            player = p.transform;
        }

        // Follow position only
        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        //Lock rotation — NEVER LookAt
        transform.rotation = fixedRotation;
    }
}