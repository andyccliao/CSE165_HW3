using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class PlayerScript : MonoBehaviour {

    private int leftHandId;
    private int rightHandId;
    private bool movementActive, stoppingActive, rotationActive;

    public Rigidbody parentForMovement;
    public int force = 5;
    public int stoppingForce = 5;
    public float forwardAngle;
    public float torque = 0.01f;
    private Vector3 rHandPos = Vector3.negativeInfinity; // used for rotation UX
    private Quaternion parentRot = Quaternion.identity;

    bool gameStart = false;
    bool countdown = false;
    public float totalTime = 0f;
    public float countdownTime = 3f;
    public Vector3 savedPos = Vector3.zero;

    public GameObject rCanvas;
    public Text rText;
    public RaceCourse rc;

    public GameObject handArrow;
    private bool showArrow = false;
    public bool endGame = false;
    public float shuntStrength = 1;

    public GameObject milk;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PlayerCollide: " + other.tag);
        if (gameStart) {
            if (other.CompareTag("Ground")) {
                countdownTime = 3f;
                gameStart = false;
                countdown = true;
                parentForMovement.position = savedPos;
                parentForMovement.velocity = Vector3.zero;
            }
            else if (other.CompareTag("Checkpoint")) {
                savedPos = other.transform.position;
            }

        }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        /* NOTE: Accessing Hands is an Orion thing, Hands already changes to Unity coordinates.
         * Using controller and frame does not; it is from the previous sdk */
        Hand lHand = Hands.Left;    //frame.Hand(leftHandId);
        Hand rHand = Hands.Right;   //frame.Hand(rightHandId);

        if (endGame || rc.noCourse) {
            if (rCanvas.activeSelf && rHand != null) {
                rCanvas.transform.position = rHand.PalmPosition.ToVector3() + rHand.Direction.ToVector3();
                rCanvas.transform.rotation = Quaternion.LookRotation(rHand.Direction.ToVector3(), -rHand.PalmNormal.ToVector3());
            }
            rText.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            rText.text = "T: " + totalTime.ToString("F3") + "\n" + (rc.checkpointNum).ToString() + "/" + rc.checkpoints.Count.ToString();

            if (showArrow && rHand != null) {
                handArrow.SetActive(false);
                milk.SetActive(true);
                milk.transform.position = rHand.PalmPosition.ToVector3() + 0.1f * rHand.PalmNormal.ToVector3();
            }
            else {
                milk.SetActive(false);
            }
            return;
        }

        /* GAME NOT YET STARTED; COUNTDOWN */
        if (!gameStart) {
            if (!countdown) {
                rText.text = "Left Hand Thumbs Up to Start";

            }
            else {
                if (countdownTime > 0) {
                    rText.text = Mathf.Ceil(countdownTime).ToString();
                    countdownTime -= Time.deltaTime;
                }
                else {
                    gameStart = true;
                    rText.text = totalTime.ToString("F3");
                    savedPos = transform.position;
                }
            }
        }
        else {
            /* GAME START */
            // Increase time
            totalTime += Time.deltaTime;
            rText.text = "T: " + totalTime.ToString("F3") + "\n" + (rc.checkpointNum).ToString() + "/" + rc.checkpoints.Count.ToString();


            if (lHand != null && movementActive) {
                //RotatedBy changes the angle of "forward", for ergonomics
                parentForMovement.AddForce(force * lHand.Direction.ToVector3().RotatedBy(Quaternion.Euler(forwardAngle, forwardAngle, 0)));
            }
            else if (stoppingActive && lHand != null && lHand.GetFistStrength() > 0.8) {
                parentForMovement.velocity = Vector3.zero;
                //parentForMovement.AddRelativeForce(stoppingForce * -parentForMovement.velocity, ForceMode.Acceleration);
            }

            if (rHand != null && rotationActive) {
                // For fine tuning          ; Next time try using 2:1 ratio pinch to turn
                parentForMovement.AddForce(force * rHand.Direction.ToVector3().RotatedBy(Quaternion.Euler(forwardAngle, -forwardAngle, 0)));
            }

            
        }

        /* Manage arrow in hand */
        if (showArrow && rHand != null) {
            handArrow.SetActive(true);
            handArrow.transform.position = rHand.PalmPosition.ToVector3() + 0.1f * rHand.PalmNormal.ToVector3();

            Vector3 nextCkpt = rc.GetNextCheckpointPosition();
            if (!nextCkpt.ContainsNaN()) {
                handArrow.transform.LookAt(nextCkpt);
            }
        }
        else {
            handArrow.SetActive(false);
        }

        /* Manage text on back of hand */
        if (rCanvas.activeSelf && rHand != null) {
            rCanvas.transform.position = rHand.PalmPosition.ToVector3();
            rCanvas.transform.rotation = Quaternion.LookRotation(rHand.Direction.ToVector3(), -rHand.PalmNormal.ToVector3());
        }
    }

    public void StartGame()
    {
        // Countdown
        Debug.Log("START GAME!");
        countdown = true;
    }
    public void StartForceInHandDirection()
    {
        Debug.Log("StartForce");
        movementActive = true;
    }
    public void EndForceInHandDirection()
    {
        Debug.Log("EndForce");
        movementActive = false;
    }
    public void StartRotationInHandDirection()
    {
        Debug.Log("StartRotation");
        rotationActive = true;
    }
    public void EndRotationInHandDirection()
    {
        Debug.Log("EndRotation");
        rotationActive = false;
    }
    public void StartStoppingForce()
    {
        Debug.Log("STOPPING FORCE");
        stoppingActive = true;
    }
    public void EndStoppingForce()
    {
        Debug.Log("No STOPPING FORCE");
        stoppingActive = false;
    }
    public void StartShowArrow()
    {
        Debug.Log("Show Arrow");
        showArrow = true;
    }
    public void EndShowArrow()
    {
        Debug.Log("No Show Arrow");
        showArrow = false;
    }
    public void ShuntLeft()
    {
        Debug.Log("Shunt Left");
        if (gameStart) {
            parentForMovement.AddRelativeForce(new Vector3(shuntStrength, 0, 0), ForceMode.Impulse);
        }
    }
    public void ShuntRight()
    {
        Debug.Log("Shunt Right");
        if (gameStart) {
            parentForMovement.AddRelativeForce(new Vector3(-shuntStrength, 0, 0), ForceMode.Impulse);
        }
    }

    public void EndGame()
    {
        endGame = true;
        parentForMovement.velocity = Vector3.up;
    }


    public void PrintDebug()
    {
        Debug.Log("RUNNUGNUGN");
    }
}   //PlayerScript
