using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class PlayerScript : MonoBehaviour {

    private Controller leap;
    private int leftHandId;
    private int rightHandId;

    private void Awake()
    {
        leap = new Controller();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        Frame frame = leap.Frame();
        Hand lHand = frame.Hand(leftHandId);
        Hand rHand = frame.Hand(rightHandId);
        //Debug.Log(leftHandId + " | " + rightHandId);

        //Assumes only two hands
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

        if (lHand != null && rHand != null) {
            Vector3 leapLPalm = lHand.PalmPosition.ToVector3();
            Debug.DrawLine(leapLPalm, leapLPalm + 10 * lHand.PalmNormal.ToVector3(), Color.white);
        }
    }

    
}   //PlayerScript
