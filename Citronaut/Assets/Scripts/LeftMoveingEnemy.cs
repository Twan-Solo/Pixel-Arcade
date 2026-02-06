using UnityEngine;

public class LeftMoveingEnemy : MonoBehaviour
{
    public float speed = 2f;

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
    }
}
