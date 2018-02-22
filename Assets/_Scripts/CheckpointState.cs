using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointState : MonoBehaviour {

	TouchedTarget onTriggerEnterCallback = null;
    bool shrinkActivated = false;

	void OnTriggerEnter (Collider col) {
		if (onTriggerEnterCallback != null) {
			onTriggerEnterCallback (this);
			onTriggerEnterCallback = null;
		} 
	}

    private void Update()
    {
        if (shrinkActivated) {
            if (transform.localScale.magnitude > 0.1) {
                transform.localScale *= 0.1f;
            }
            else {
                UnenableSelf();
            }
        }
    }

    public void StartShrinking()
    {
        shrinkActivated = true;
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

    public void UnenableSelf()
    {
        enabled = false;
    }
}
