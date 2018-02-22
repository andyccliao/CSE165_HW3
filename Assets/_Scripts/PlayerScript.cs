using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class PlayerScript : MonoBehaviour {

    private int leftHandId;
    private int rightHandId;
    private bool movementActive, stoppingActive;

    public Rigidbody parentForMovement;
    public int force = 5;
    public int stoppingForce = 5;
    public float forwardAngle;
    public float torque = 0.01f;

    private void Awake()
    {
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
        //Debug.Log(leftHandId + " | " + rightHandId);

        //Assumes only two hands
        /*
        if (lHand == null || rHand == null) {
            if (frame.Hands.Count >= 1) {
                Hand hand1 = frame.Hands[0];
                if (hand1.IsLeft) {
                    lHand = hand1;
                    leftHandId = hand1.Id;
                }
                else if (hand1.IsRight) {
                    rHand = hand1;
                    rightHandId = hand1.Id;
                }
            }
            if (frame.Hands.Count >= 2) {
                Hand hand2 = frame.Hands[1];
                if (hand2.IsLeft) {
                    lHand = hand2;
                    leftHandId = hand2.Id;
                }
                else if (hand2.IsRight) {
                    rHand = hand2;
                    rightHandId = hand2.Id;
                }
            }
        }
        */

        if (lHand != null && movementActive && lHand.GetFistStrength() > 0.85f) {
            parentForMovement.AddForce(force * lHand.Direction.ToVector3().RotatedBy(Quaternion.Euler(0, forwardAngle, 0)));
        } else if (stoppingActive) {
            parentForMovement.AddRelativeForce(stoppingForce * -parentForMovement.velocity);
        }
    }

    public void StartForceInHandDirection()
    {
        movementActive = true;
    }
    public void EndForceInHandDirection()
    {
        movementActive = false;
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
    public void PrintDebug()
    {
        Debug.Log("RUNNUGNUGN");
    }
}   //PlayerScript
