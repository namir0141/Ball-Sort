////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;

////public class GameController : MonoBehaviour
////{
////    public BottleController FirstBottle;
////    public BottleController SecondBottle;
////    public BottleController[] bottles;

////    private float bottleUp = 0.3f; // Select bottle
////    private float bottleDown = -0.3f; // Deselect bottle



////    void Update()
////    {
////        if (Input.GetMouseButtonDown(0)) // Perform raycast on left mouse button click (you can change this trigger)
////        {
////            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
////            RaycastHit hit;

////            if (Physics.Raycast(ray, out hit))
////            {
////                BottleController bottle = hit.collider.GetComponent<BottleController>();

////                if (bottle != null)
////                {
////                    if (FirstBottle == null)
////                    {
////                        FirstBottle = bottle;
////                        FirstBottle.transform.position += new Vector3(0, bottleUp, 0);
////                    }
////                    else
////                    {
////                        if (FirstBottle == bottle)
////                        {
////                            FirstBottle.transform.position -= new Vector3(0, bottleUp, 0);
////                            FirstBottle = null;
////                        }
////                        else
////                        {
////                            SecondBottle = bottle;
////                            FirstBottle.transform.position -= new Vector3(0, bottleUp, 0);
////                            FirstBottle = bottle;
////                            FirstBottle.transform.position += new Vector3(0, bottleUp, 0);
////                            SecondBottle = null;
////                        }
////                    }
////                }
////                else // Tap anywhere on the screen to deselect bottles
////                {
////                    if (FirstBottle != null)
////                    {
////                        FirstBottle.transform.position -= new Vector3(0, bottleUp, 0);
////                        FirstBottle = null;
////                    }
////                }
////            }
////        }

////        // Other game logic...
////    }
////}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GameController : MonoBehaviour
//{
//    public BottleController selectedBottle;
//    public string ballTag = "Ball"; // Tag of the ball object

//    private float bottleUp = 0.3f; // Select bottle
//    private float bottleDown = -0.3f; // Deselect bottle

//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            RaycastHit hit;

//            if (Physics.Raycast(ray, out hit))
//            {
//                BottleController bottle = hit.collider.GetComponent<BottleController>();

//                if (bottle != null)
//                {
//                    if (selectedBottle == null)
//                    {
//                        selectedBottle = bottle;
//                        selectedBottle.SelectBottle(); // Select the bottle
//                        selectedBottle.MoveUp(bottleUp); // Move the bottle up
//                    }
//                    else if (selectedBottle == bottle)
//                    {
//                        selectedBottle.MoveDown(); // Move the bottle down
//                        selectedBottle.DeselectBottle(); // Deselect the bottle
//                        selectedBottle = null;
//                    }
//                    else
//                    {
//                        // Transfer ball to the new tube (selectedBottle to bottle)
//                        TransferBall(selectedBottle, bottle);
//                        selectedBottle.MoveDown(); // Move the previous bottle down
//                        selectedBottle.DeselectBottle(); // Deselect the previous bottle
//                        selectedBottle = null;
//                    }
//                }
//            }
//        }

//        // Other game logic...
//    }

//    void TransferBall(BottleController sourceBottle, BottleController destinationBottle)
//    {
//        // Search for the ball in the source bottle's children
//        foreach (Transform child in sourceBottle.transform)
//        {
//            if (child.CompareTag(ballTag))
//            {
//                child.SetParent(destinationBottle.transform);
//                child.localPosition = Vector3.zero;
//                return;
//            }
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public BottleController selectedBottle;
    public string ballTag = "Ball"; // Tag of the ball object

    private float bottleUp = 0.3f; // Select bottle
    private float bottleDown = -0.3f; // Deselect bottle
    private float ballStackHeight = -0.3f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                BottleController bottle = hit.collider.GetComponent<BottleController>();

                if (bottle != null)
                {
                    if (selectedBottle == null)
                    {
                        selectedBottle = bottle;
                        selectedBottle.SelectBottle(); // Select the bottle
                        selectedBottle.MoveUp(bottleUp); // Move the bottle up
                    }
                    else if (selectedBottle == bottle)
                    {
                        selectedBottle.MoveDown(); // Move the bottle down
                        selectedBottle.DeselectBottle(); // Deselect the bottle
                        selectedBottle = null;
                    }
                    else
                    {
                        // Transfer top ball to the new tube (selectedBottle to bottle)
                        TransferTopBall(selectedBottle, bottle);
                        selectedBottle.MoveDown(); // Move the previous bottle down
                        selectedBottle.DeselectBottle(); // Deselect the previous bottle
                        selectedBottle = null;
                    }
                }
            }
        }

        // Other game logic...
    }

    void TransferTopBall(BottleController sourceBottle, BottleController destinationBottle)
    {
        Transform topBall = null;

        // Search for the top ball in the source bottle's children
        foreach (Transform child in sourceBottle.transform)
        {
            if (child.CompareTag(ballTag))
            {
                topBall = child;
                break;
            }
        }

        if (topBall != null)
        {
            topBall.SetParent(destinationBottle.transform);

            int ballCount = destinationBottle.GetBallCount(); // Get the count of balls in the destination bottle
            float stackHeight = ballCount * ballStackHeight; // Calculate stack height based on ball count

            topBall.localPosition = new Vector3(0, stackHeight, 0);
            destinationBottle.AddBall(topBall); // Add the ball to the destination bottle

            // Remove the ball from the source bottle
            sourceBottle.RemoveBall(topBall);
        }
    }
}