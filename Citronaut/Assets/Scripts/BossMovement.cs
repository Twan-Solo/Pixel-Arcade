using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BossMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;               // movement speed
    public float moveDistance = 5f;        // how far left/right from start
    public bool startLeft = true;          // initial direction

    [Header("Hop Settings")]
    public float maxHopHeight = 1f;        // max random hop height
    public float hopChancePerSecond = 0.1f;// chance per second to hop

    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingLeft;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        startPosition = rb.position;

        // Set initial direction
        movingLeft = startLeft;
        targetPosition = movingLeft ? startPosition + Vector3.left * moveDistance
                                   : startPosition + Vector3.right * moveDistance;
    }

    private void FixedUpdate()
    {
        // Move towards target
        Vector3 moveDir = (targetPosition - rb.position).normalized;
        rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);

        // Switch direction if reached target
        if (Vector3.Distance(rb.position, targetPosition) < 0.05f)
        {
            movingLeft = !movingLeft;
            targetPosition = movingLeft ? startPosition + Vector3.left * moveDistance
                                        : startPosition + Vector3.right * moveDistance;
        }

        // Random hop if grounded
        if (Random.value < hopChancePerSecond * Time.fixedDeltaTime && IsGrounded())
        {
            float hopHeight = Random.Range(0.3f, maxHopHeight);
            rb.AddForce(Vector3.up * Mathf.Sqrt(2f * 9.81f * hopHeight), ForceMode.VelocityChange);
        }
    }

    // Check if boss is on the ground
    private bool IsGrounded()
    {
        return Physics.Raycast(rb.position, Vector3.down, 0.6f);
    }
}
