using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    // Public Serialized Fields
    [Header("Public Variables")]
    [SerializeField] public string ballTag = "Ball";
    [SerializeField] public List<Transform> balls = new List<Transform>();
    [SerializeField] public bool canMoveUp = true;
    [SerializeField] public bool moveUpInProgress = false;
    [SerializeField] public bool moveDownInProgress = false;
    [SerializeField] public bool ballTransferInProgress = false;

    // Private Serialized Fields
    [Header("Private Variables")]
    [SerializeField] private int maxBallLimit = 3;
    [SerializeField] private Vector3 selectedOffset = new Vector3(0, 0.3f, 0);
    [SerializeField] private float ballStackHeight = 0.3f;
    [SerializeField] private float initialBallHeight = -0.3f; // Initial height for the first ball
    [SerializeField] private bool isSelected = false;
    [SerializeField] private Collider bottleCollider;
    [SerializeField] private bool interactionEnabled = true;


    private void Start()
    {
        CollectInitialBalls();
        bottleCollider = GetComponent<Collider>();
    }

    public void EnableRaycast()
    {
        if (bottleCollider != null)
        {
            bottleCollider.enabled = true;
        }
    }

    public void DisableRaycast()
    {
        if (bottleCollider != null)
        {
            bottleCollider.enabled = false;
        }
    }

    private void CollectInitialBalls()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(ballTag))
            {
                balls.Add(child);
            }
        }
    }
    public bool IsSelected
    {
        get { return isSelected; }
    }
    public void SelectBottle()
    {
        isSelected = true;
        Transform topBall = GetTopBall();
        if (topBall != null)
        {
            Rigidbody topBallRigidbody = topBall.GetComponent<Rigidbody>();
            if (topBallRigidbody != null)
            {
                topBallRigidbody.useGravity = false;
                topBallRigidbody.isKinematic = false;
            }
        }
    }

    public void SelectBottle2()
    {
        isSelected = true;
        Transform topBall = GetSecondTopBall();
        if (topBall != null)
        {
            Rigidbody topBallRigidbody = topBall.GetComponent<Rigidbody>();
            if (topBallRigidbody != null)
            {
                topBallRigidbody.useGravity = false;
                topBallRigidbody.isKinematic = false;
            }
        }
    }

    public void DeselectBottle()
    {

        isSelected = false;

        Transform topBall = GetTopBall();
        if (topBall != null)
        {

            Rigidbody topBallRigidbody = topBall.GetComponent<Rigidbody>();
            if (topBallRigidbody != null)
            {
                topBallRigidbody.useGravity = true;
                topBallRigidbody.velocity = Vector3.zero;
                topBallRigidbody.angularVelocity = Vector3.zero;
            }
        }
    }


    public void MoveUp()
    {
        if (isSelected)
        {
            if (!ballTransferInProgress && GetBallCount() > 0)
            {
                MoveTopBallUp();
            }
            else if (ballTransferInProgress && GetBallCount() > 1)
            {
                MoveUpSecondBall();
            }
        }
    }
    public void MoveTopBallUp()
    {
        if (isSelected)
        {
            canMoveUp = false;
            Transform topBall = GetTopBall();
            if (topBall != null && !topBall.GetComponent<Rigidbody>().useGravity)
            {
                moveUpInProgress = true;
                Transform targetObject = transform.Find("Target");
                if (targetObject != null)
                {
                    topBall.DOMove(targetObject.position, 0.2f).OnComplete(() =>
                    {
                        topBall.GetComponent<Rigidbody>().useGravity = false;
                        topBall.GetComponent<Rigidbody>().isKinematic = true;
                        moveUpInProgress = false;
                        canMoveUp = true;
                    }).SetEase(Ease.Linear);
                }
                else
                {
                    moveUpInProgress = false;
                    canMoveUp = true;
                }
            }
        }
    }

    public void MoveUpSecondBall()
    {
        if (isSelected)
        {
            canMoveUp = false;

            Transform secondTopBall = GetSecondTopBall();

            if (secondTopBall != null && !secondTopBall.GetComponent<Rigidbody>().useGravity)
            {
                moveUpInProgress = true;

                Transform targetObject = transform.Find("Target");

                if (targetObject != null)
                {
                    secondTopBall.DOMove(targetObject.position, 0.2f).OnComplete(() =>
                    {
                        secondTopBall.GetComponent<Rigidbody>().useGravity = false;
                        secondTopBall.GetComponent<Rigidbody>().isKinematic = true;
                        moveUpInProgress = false;
                        canMoveUp = true;
                    }).SetEase(Ease.Linear);
                }
                else
                {
                    moveUpInProgress = false;
                    canMoveUp = true;
                }
            }
        }
    }


    public void MoveDown(bool moveSecondTop = false)
    {
        if (isSelected) // Check if the bottle is selected and has more than one ball
        {
            Transform ballToMoveDown = moveSecondTop ? GetSecondTopBall() : GetTopBall();

            if (ballToMoveDown != null)
            {
                Rigidbody rb = ballToMoveDown.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Check if the Rigidbody is kinematic
                    if (rb.isKinematic)
                    {
                        // If kinematic, set it to non-kinematic before moving down
                        rb.isKinematic = false;
                    }

                    Vector3 newPosition = ballToMoveDown.position - selectedOffset;
                    ballToMoveDown.position = newPosition;
                    moveDownInProgress = true;

                    // After successfully moving down, allow MoveUp() to be called again
                    canMoveUp = true;
                }
            }
        }
    }


    public int GetBallCount()
    {
        return balls.Count;
    }

    public void AddBall(Transform ball)
    {
        balls.Add(ball);

        // Calculate stack height for the newly added ball
        float stackHeight = initialBallHeight + (balls.Count - 1) * ballStackHeight;

        // Set the position of the added ball based on the stack height
        ball.SetParent(transform);
        ball.localPosition = new Vector3(0, stackHeight, 0);
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            ballRigidbody.isKinematic = true; // Set the ball's isKinematic to true when added to the bottle
        }
    }

    public void RemoveBall(Transform ball)
    {
        balls.Remove(ball);
    }

    public bool IsComplete()
    {

        return GetBallCount() >= 3;
    }

    public Transform GetTopBall()
    {
        if (balls.Count > 0)
        {
            return balls[balls.Count - 1]; // Return the topmost ball
        }
        else
        {
            return null;
        }
    }

    public Transform GetSecondTopBall()
    {
        if (balls.Count > 1)
        {
            return balls[balls.Count - 2]; // Return the second topmost ball if available
        }
        else
        {
            return null;
        }
    }


    public void TransferTopBall(BottleController destinationBottle)
    {
        if (!ballTransferInProgress && interactionEnabled && isSelected)
        {
            Transform topBall = GetTopBall();

            if (topBall != null && destinationBottle.canMoveUp && destinationBottle.GetBallCount() < destinationBottle.maxBallLimit)
            {
                SpriteRenderer currentBallRenderer = topBall.GetComponent<SpriteRenderer>();
                SpriteRenderer destinationTopBallRenderer = destinationBottle.GetTopBall()?.GetComponent<SpriteRenderer>();

                if (currentBallRenderer != null && destinationTopBallRenderer == null || SpriteNameEqual(currentBallRenderer.sprite, destinationTopBallRenderer.sprite))
                {
                    Rigidbody topBallRigidbody = topBall.GetComponent<Rigidbody>();

                    if (topBallRigidbody != null)
                    {
                        topBallRigidbody.isKinematic = false;

                        BallController ballMovement = topBall.GetComponent<BallController>();

                        if (ballMovement != null)
                        {
                            ballTransferInProgress = true;
                            ballMovement.MoveToTarget(destinationBottle.transform.Find("Target"));
                            StartCoroutine(TransferOwnershipAfterDelay(topBall, destinationBottle));
                            destinationBottle.DisableRaycast();
                        }
                    }
                }
            }
        }
    }


    private bool CanTransferBall(BottleController destinationBottle, SpriteRenderer currentBallRenderer)
    {
        if (destinationBottle.GetBallCount() < destinationBottle.maxBallLimit)
        {
            SpriteRenderer destinationTopBallRenderer = destinationBottle.GetTopBall()?.GetComponent<SpriteRenderer>();

            if (destinationTopBallRenderer == null)
            {
                return true;
            }
            else
            {
                return SpriteNameEqual(currentBallRenderer.sprite, destinationTopBallRenderer.sprite);
            }
        }

        return false;
    }


    private IEnumerator TransferOwnershipAfterDelay(Transform ball, BottleController destinationBottle)
    {

        yield return new WaitForSeconds(0.5f); // Adjust the delay time as needed

        ball.SetParent(destinationBottle.transform);
        destinationBottle.EnableRaycast();
        destinationBottle.AddBall(ball);
        RemoveBall(ball);


        // Ensure isKinematic is true for all balls in the destination bottle after transferring

        interactionEnabled = true;
        ballTransferInProgress = false;
    }


    public bool SpriteNameEqual(Sprite spriteA, Sprite spriteB)
    {
        // Compare the names of the sprites
        return spriteA.name == spriteB.name;
    }

    public bool AreBallsSameSpriteName()
    {
        if (balls.Count <= 1)
        {
            return true;
        }

        string firstBallSpriteName = balls[0].GetComponent<SpriteRenderer>().sprite.name;

        foreach (Transform ball in balls)
        {
            string currentBallSpriteName = ball.GetComponent<SpriteRenderer>().sprite.name;

            if (firstBallSpriteName != currentBallSpriteName)
            {
                return false;
            }
        }

        return true;
    }


}




