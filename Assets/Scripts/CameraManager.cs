using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	[SerializeField] Camera _boxCamera;
	[SerializeField] Camera _inHoleCamera;

	// Use this for initialization
	void Awake () {
		_boxCamera.enabled = true;
		_inHoleCamera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CameraSwitch(InsidePeepHoleEvent e){
		if (e.IsInsidePeepHole) {
			_boxCamera.enabled = false;
			_inHoleCamera.enabled = true;
		} else {
			_boxCamera.enabled = true;
			_inHoleCamera.enabled = false;
		}
	}

	void OnEnable(){
		Events.G.AddListener<InsidePeepHoleEvent> (CameraSwitch);
	}
	void OnDisable(){
		Events.G.RemoveListener<InsidePeepHoleEvent> (CameraSwitch);
	}
}
