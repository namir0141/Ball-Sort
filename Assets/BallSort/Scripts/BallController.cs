using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    private Coroutine moveCoroutine; // Use Coroutine directly instead of MoveCoroutine
    private bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 30f;
        rb.isKinematic = false;
    }


    public void MoveToTarget(Transform targetPosition)
    {
        if (isMoving)
        {
            Debug.Log("Another ball is already in motion.");
            return;
        }

        // Stop the previous coroutine if it exists
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // Start the new animation
        moveCoroutine = StartCoroutine(MoveBall(targetPosition.position, 0.1f));
    }

    private IEnumerator MoveBall(Vector3 targetPosition, float duration)
    {
        isMoving = true;

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        rb.useGravity = true;

        yield return new WaitForSeconds(0.1f);

        isMoving = false;
    }


}
