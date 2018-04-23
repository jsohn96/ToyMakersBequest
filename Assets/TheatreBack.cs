using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreBack : MonoBehaviour {

	[SerializeField] TheatreCameraControl _theatreCameraControl;
	[SerializeField] TheatreRotation _theatreRotation;
	BoxCollider _thisBoxCollider;

	[SerializeField] Transform _backCover;

	void Start(){
		_thisBoxCollider = GetComponent<BoxCollider> ();
		Vector3 tempPos = _backCover.localPosition;
		tempPos.x = 0f;
		_backCover.localPosition = tempPos;
	}


	void OnTouchDown(){
		//rotateTheater to face center
		// move camera to zoom in
		_backCover.gameObject.SetActive(false);
		_thisBoxCollider.enabled = false;

		_theatreRotation.StartBackRotation ();
	}
}
