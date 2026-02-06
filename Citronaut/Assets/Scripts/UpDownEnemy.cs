using UnityEngine;

public class UpDownEnemy : MonoBehaviour
{
    public float moveHeight = 2f;
    public float moveSpeed = 2f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * moveSpeed) * moveHeight;

        transform.position = new Vector3(
            startPos.x,
            startPos.y + yOffset,
            startPos.z
        );
    }
}
