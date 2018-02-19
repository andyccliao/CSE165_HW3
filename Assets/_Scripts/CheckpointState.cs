using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointState : MonoBehaviour {

	TouchedTarget callback = null;

	void OnTriggerEnter (Collider col) {
		if (callback != null) {
			callback ();
			callback = null;

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
}
