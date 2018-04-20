using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreKey : MonoBehaviour {

	[SerializeField] TheatreCameraControl _theatreCameraControl;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.K)) {
			_theatreCameraControl.ZoomOut ();
		}
	}
}
