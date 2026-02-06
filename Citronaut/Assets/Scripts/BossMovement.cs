using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 3f;     // how far left/right to move
    public float moveSpeed = 2f;        // movement speed
    public float actionCooldown = 1.5f; // how often it chooses a new action

    [Header("Hop Settings")]
    public float hopHeight = 1f;        // how high the hop goes
    public float hopDuration = 0.25f;   // time to reach top (so full up+down = 0.5s)

    private bool isActing = false;

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

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + direction * moveDistance;

        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isActing = false;
    }

    IEnumerator Hop()
    {
        isActing = true;

        Vector3 startPos = transform.position;
        Vector3 topPos = startPos + Vector3.up * hopHeight;

        // Move up
        float timer = 0f;
        while (timer < hopDuration)
        {
            transform.position = Vector3.Lerp(startPos, topPos, timer / hopDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure exact top position
        transform.position = topPos;

        // Move down
        timer = 0f;
        while (timer < hopDuration)
        {
            transform.position = Vector3.Lerp(topPos, startPos, timer / hopDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure exact start position
        transform.position = startPos;

        isActing = false;
    }
}


