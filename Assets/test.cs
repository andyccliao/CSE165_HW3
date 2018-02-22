using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class test : MonoBehaviour {

    private Controller leap;

    // Use this for initialization
    void Start () {
        leap = new Controller();

    }

    // Update is called once per frame
    void Update () {
        Frame frame = leap.Frame();
        Hand lHand;
        if (frame.Hands.Count != 0) {
            lHand = frame.Hands[0];
            Debug.DrawRay(lHand.PalmPosition.ToVector3(), lHand.PalmNormal.ToVector3());
        }
        else {
            Debug.Log("No hands");
        }

    }
}
