using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreCoin : MonoBehaviour {
	bool _pickupable = false;

	public void BeginGlow(){
		GetComponent<shaderGlowCustom> ().enabled = true;
		_pickupable = true;
	}

	void OnTouchDown(Vector3 point){
		if (_pickupable) {
			gameObject.SetActive (false);
		}
	}
}
