using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    private bool isSelected = false;
    private Vector3 selectedOffset = new Vector3(0, 0.3f, 0);
    public string ballTag = "Ball";
    private List<Transform> balls = new List<Transform>();

    public bool IsSelected
    {
        get { return isSelected; }
    }


    public void SelectBottle()
    {
        isSelected = true;
        Debug.Log("Bottle selected"); // Log when the bottle is selected
        // Apply any visual changes when selected
    }

    // Function to handle deselection
    public void DeselectBottle()
    {
        isSelected = false;
        Debug.Log("Bottle deselected"); // Log when the bottle is deselected
        // Apply any visual changes when deselected
    }

    // Function to move the bottle up
    public void MoveUp(float amount)
    {
        Debug.Log("Moving bottle up"); // Log when moving the bottle up
        transform.position += new Vector3(0, amount, 0);
    }

    // Function to move the bottle down
    public void MoveDown()
    {
        Debug.Log("Moving bottle down"); // Log when moving the bottle down
        transform.position -= selectedOffset; // Remove offset to move down
    }
    public int GetBallCount()
    {
        return balls.Count;
    }

    public void AddBall(Transform ball)
    {
        balls.Add(ball);
    }

    public void RemoveBall(Transform ball)
    {
        balls.Remove(ball);
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


}
