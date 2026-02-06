using UnityEngine;

public class LeftMoveingEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed of the enemy")]
    public float speed = 2f;

    [Tooltip("Distance the enemy moves left and right from its starting position")]
    public float moveDistance = 5f;

    [Tooltip("If true, enemy starts moving left first; if false, starts moving right first")]
    public bool startLeft = true;

    [Header("Flip Settings")]
    [Tooltip("Assign the sprite or model to flip horizontally")]
    public Transform visual; // assign your sprite or model here

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingLeft;

    private void Start()
    {
        startPosition = transform.position;

        // Set initial direction based on inspector
        movingLeft = startLeft;
        targetPosition = movingLeft ? startPosition + Vector3.left * moveDistance
                                   : startPosition + Vector3.right * moveDistance;

        // Flip the visual to face initial direction
        UpdateFlip();
    }

    private void Update()
    {
        // Move towards target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if reached target
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            if (movingLeft)
            {
                targetPosition = startPosition;
                movingLeft = false;
            }
            else
            {
                targetPosition = startPosition + (startLeft ? Vector3.right * moveDistance : Vector3.left * moveDistance);
                movingLeft = true;
            }

            // Flip visual when changing direction
            UpdateFlip();
        }
    }

    private void UpdateFlip()
    {
        if (visual == null) return;

        // Flip by scaling X
        Vector3 scale = visual.localScale;
        scale.x = movingLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        visual.localScale = scale;
    }
}
