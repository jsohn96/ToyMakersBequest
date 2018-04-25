using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreCameraCollider : MonoBehaviour {
	[SerializeField] TheatreCameraControl _theatreCameraControl;

	void OnTouchDown(Vector3 point){
		_theatreCameraControl.InitializeScroll ();
	}

	void OnTouchUp(Vector3 point){
		_theatreCameraControl.EndScroll ();
	}
}
