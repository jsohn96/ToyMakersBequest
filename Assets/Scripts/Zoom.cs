using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour {
	bool _isZoomed = false;
	Timer _zoomTimer;
	float _currentFOV = 60.0f;
	[SerializeField] AnimationCurve _zoomCurve;

	void Awake () {
		_zoomTimer = new Timer (0.6f);
	}
	
	void Update () {
		if(Input.GetButtonDown("Zoom")){
			_zoomTimer.Reset ();
			_currentFOV = Camera.main.fieldOfView;
			if(_isZoomed){
				_isZoomed = false;
			} else {
				_isZoomed = true;
			}
		}


		if (_isZoomed) {
			Camera.main.fieldOfView = Mathf.Lerp (_currentFOV, 20.0f, _zoomCurve.Evaluate(_zoomTimer.PercentTimePassed));
		} else {
			Camera.main.fieldOfView = Mathf.Lerp (_currentFOV, 60.0f, _zoomCurve.Evaluate(_zoomTimer.PercentTimePassed));
		}
	}
}
