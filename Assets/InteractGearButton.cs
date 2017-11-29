using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGearButton : MonoBehaviour {

	bool _isOn = false;

	void OnEnable(){
		_isOn = true;
	}

	void OnDisable() {
		_isOn = false;
	}
	
	void FixedUpdate () {
		if (_isOn) {
			//transform.Rotate (Vector3.forward, -0.5f);
		}
	}
}
