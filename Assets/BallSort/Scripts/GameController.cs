using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public BottleController selectedBottle;
    public string ballTag = "Ball"; // Tag of the ball object
    public int levelToUnlock;
    public BottleController[] bottles;
    [SerializeField] private int ballcount;


    private void Awake()
    {
        Input.multiTouchEnabled = false;
    }
    void Update()
    {
        handleInput();
        CheckLevelCompletion();
    }


    void handleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag(ballTag))
                {
                    // Handle the ball interaction
                    Debug.Log("Ball Hit: " + hit.collider.gameObject.name);
                    // Perform actions for the ball hit (if needed)
                }
                else
                {
                    BottleController bottle = hit.collider.GetComponent<BottleController>();
                    if (bottle != null && bottle.canMoveUp)
                    {
                        HandleBottleInteraction(bottle);
                        Debug.Log("Bottle Hit: " + bottle.gameObject.name);
                        // Perform actions for the bottle hit (if needed)
                    }
                }
            }
        }
        // Other game logic...
    }


    void HandleBottleInteraction(BottleController bottle)
    {
        if (bottle != null)
        {
            if (selectedBottle == null && !bottle.ballTransferInProgress)
            {
                selectedBottle = bottle;
                selectedBottle.SelectBottle(); // Select the bottle
                selectedBottle.MoveUp(); // Move the bottle up

            }
            else if (bottle.ballTransferInProgress)
            {
                selectedBottle = bottle;
                selectedBottle.SelectBottle2(); // Select the bottle
                selectedBottle.MoveUp();
            }
            else if (selectedBottle == bottle)
            {
                selectedBottle.MoveDown(); // Move the bottle down
                selectedBottle.DeselectBottle(); // Deselect the bottle
                selectedBottle = null;

            }
            else
            {
                selectedBottle.TransferTopBall(bottle);
                selectedBottle.MoveDown(); // Move the previous bottle down
                selectedBottle.DeselectBottle(); // Deselect the previous bottle

                selectedBottle = null;

                // Transfer top ball to the new tube (selectedBottle to bottle)

            }
        }

    }


    public bool CheckTubeWithThreeSameBalls()
    {
        foreach (BottleController bottle in bottles)
        {
            // Check if the bottle has child objects
            if (bottle.transform.childCount > 0)
            {
                // Get all child objects of the current bottle
                Transform[] children = bottle.GetComponentsInChildren<Transform>();

                // Filter only the child objects that have the tag "Ball"
                IEnumerable<Transform> balls = children.Where(child => child.CompareTag(ballTag));

                // Check if exactly three balls are present
                if (balls.Count() == ballcount)
                {
                    // Get distinct sprite names of the balls
                    var distinctSpriteNames = balls.Select(ball => ball.GetComponent<SpriteRenderer>().sprite.name).Distinct();

                    // If exactly one distinct sprite name is found among the three balls, return true
                    if (distinctSpriteNames.Count() == 1)
                    {
                        return true; // Return true if the conditions are met
                    }
                }
            }
        }

        return false; // Return false if no tube meets the conditions
    }
    void CheckLevelCompletion()
    {
        bool isOneTubeFilled = false;
        bool isExtraBallsLeft = false;

        foreach (BottleController bottle in bottles)
        {
            if (bottle.IsComplete())
            {
                isOneTubeFilled = true;
            }
            else
            {
                if (bottle.GetBallCount() > 0)
                {
                    isExtraBallsLeft = true;
                }

            }
        }

        if (isOneTubeFilled && !isExtraBallsLeft && CheckTubeWithThreeSameBalls())
        {
            UnlockNextLevel();
            SceneManager.LoadScene("Win");
            // Add logic here for completing the level (e.g., show completion UI, load next level, etc.)
        }
    }
    void UnlockNextLevel()
    {
        int currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");
        PlayerPrefs.SetInt("UnlockedLevel", levelToUnlock);
        if (levelToUnlock > currentUnlockedLevel)
        {
            int nextLevelToUnlock = currentUnlockedLevel + 1;

            PlayerPrefs.SetInt("UnlockedLevel", nextLevelToUnlock);
            PlayerPrefs.Save();
        }

    }

}

