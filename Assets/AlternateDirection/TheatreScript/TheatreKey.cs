using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreKey : MonoBehaviour {

	[SerializeField] TheatreCameraControl _theatreCameraControl;
	[SerializeField] TheatreLighting _theatreLighting;
	bool _keyPickedUp = false;


	void OnTouchDown(){
		if (!_keyPickedUp) {
			StartCoroutine(KeyPickedUp ());
		}
	}

	IEnumerator KeyPickedUp(){
		_theatreLighting.IntermissionLight ();
		_theatreCameraControl.ZoomOut ();
		gameObject.SetActive (false);
		yield return null;
	}
}
