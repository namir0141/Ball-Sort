using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb; // Rigidbody component of the ball
    private bool isMoving = false; // Flag to indicate if the ball is moving

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ReleaseBall(Vector3 direction, float force)
    {
        isMoving = true;
        rb.isKinematic = false; // Enable physics for the ball
        rb.AddForce(direction * force, ForceMode.Impulse); // Add force to release the ball
    }

    public bool IsMoving()
    {
        return isMoving;
    }


}
