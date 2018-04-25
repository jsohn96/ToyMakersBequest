using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreKey : MonoBehaviour {

	[SerializeField] TheatreCameraControl _theatreCameraControl;
	bool _keyPickedUp = false;


	void OnTouchDown(){
		if (!_keyPickedUp) {
			StartCoroutine(KeyPickedUp ());
		}
	}

	IEnumerator KeyPickedUp(){
		
		_theatreCameraControl.ZoomOut ();
		gameObject.SetActive (false);
		yield return null;
	}
}
