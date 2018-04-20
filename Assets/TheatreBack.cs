using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreBack : MonoBehaviour {

	[SerializeField] TheatreCameraControl _theatreCameraControl;
	[SerializeField] TheatreRotation _theatreRotation;
	BoxCollider _thisBoxCollider;

	void Start(){
		_thisBoxCollider = GetComponent<BoxCollider> ();
	}


	void OnTouchDown(){
		//rotateTheater to face center
		// move camera to zoom in
		_thisBoxCollider.enabled = false;
	}
}
