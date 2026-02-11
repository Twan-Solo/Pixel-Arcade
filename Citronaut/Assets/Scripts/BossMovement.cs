using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BossMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 3f;     // how far left/right to move
    public float moveSpeed = 2f;        // movement speed
    public float actionCooldown = 1.5f; // how often it chooses a new action

    [Header("Hop Settings")]
    public float hopHeight = 1f;        // how high the hop goes
    public float hopDuration = 0.25f;   // time to reach top (full up+down = 0.5s)

    private Rigidbody rb;
    private bool isActing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Rigidbody setup
        rb.constraints = RigidbodyConstraints.FreezeRotation;  // prevents tipping
        rb.interpolation = RigidbodyInterpolation.Interpolate; // smooth movement
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // prevent tunneling
    }

    private void Start()
    {
        InvokeRepeating(nameof(ChooseAction), 1f, actionCooldown);
    }

    void ChooseAction()
    {
        if (isActing) return;

        int action = Random.Range(0, 3); // 0 = left, 1 = right, 2 = hop

        switch (action)
        {
            case 0:
                StartCoroutine(Move(Vector3.left));
                break;
            case 1:
                StartCoroutine(Move(Vector3.right));
                break;
            case 2:
                StartCoroutine(Hop());
                break;
        }
    }

    IEnumerator Move(Vector3 direction)
    {
        isActing = true;

        Vector3 startPos = rb.position;
        Vector3 targetPos = startPos + direction * moveDistance;
        Vector3 moveDir = (targetPos - startPos).normalized;

        while (Vector3.Distance(rb.position, targetPos) > 0.05f)
        {
            // Check for wall ahead using Raycast
            if (!Physics.Raycast(rb.position, moveDir, 0.6f))
            {
                rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                break; // hit a wall, stop moving
            }

            yield return new WaitForFixedUpdate();
        }

        isActing = false;
    }

    IEnumerator Hop()
    {
        isActing = true;

        Vector3 startPos = rb.position;
        Vector3 topPos = startPos + Vector3.up * hopHeight;

        // Move up
        float timer = 0f;
        while (timer < hopDuration)
        {
            Vector3 newPos = Vector3.Lerp(startPos, topPos, timer / hopDuration);
            rb.MovePosition(newPos);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(topPos);

        // Move down
        timer = 0f;
        while (timer < hopDuration)
        {
            Vector3 newPos = Vector3.Lerp(topPos, startPos, timer / hopDuration);
            rb.MovePosition(newPos);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(startPos);

        isActing = false;
    }
}

