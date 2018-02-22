using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointState : MonoBehaviour {

	TouchedTarget onTriggerEnterCallback = null;

	void OnTriggerEnter (Collider col) {
		if (onTriggerEnterCallback != null) {
			onTriggerEnterCallback (this);
			onTriggerEnterCallback = null;
		} 
	}

	public void SetMaterial (Material mat) {
		Renderer renderer = GetComponent<Renderer>();
		if (renderer != null) {
			renderer.material = mat;
		} else {
			Debug.Log ("NO RENDERER IN " + name);
		}
	}

    public void SetOnTriggerEnterCallback(TouchedTarget cb)
    {
        onTriggerEnterCallback = cb;
    }
}
