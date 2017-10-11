using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

	[SerializeField] Vector3 _goalPosition;
	Vector3 _originPosition;
	bool _beginZoom = false;
	Timer _zoomTimer;

	// Use this for initialization
	void Awake () {
		_zoomTimer = new Timer (3.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (_beginZoom) {
			transform.localPosition = Vector3.Lerp (_originPosition, _goalPosition, _zoomTimer.PercentTimePassed);
		}
	}

	public void BeginZoom(){
		_originPosition = transform.localPosition;
		_beginZoom = true;
		_zoomTimer.Reset ();
	}
}
