using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour {
	bool _isZoomed = false;
	Timer _zoomTimer;
	float _currentFOV = 60.0f;
	[SerializeField] AnimationCurve _zoomCurve;

	Vector3 _originPos;
	Quaternion _originRot;

	Vector3 _goalPos;
	Quaternion _goalRot;

	Vector3 _tempPos;
	Quaternion _tempRot;

	bool _isDone = true;

	void Awake () {
		_zoomTimer = new Timer (1.5f);
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

		if (!_isDone) {
			if (_isZoomed) {
				_tempPos = Vector3.Lerp (_originPos, _goalPos, _zoomTimer.PercentTimePassed);
				_tempRot = Quaternion.Lerp (_originRot, _goalRot, _zoomTimer.PercentTimePassed);
				transform.SetPositionAndRotation (_tempPos, _tempRot);
				if (transform.position == _goalPos) {
					_isDone = true;
				}

			} else {
				_tempPos = Vector3.Lerp (_goalPos, _originPos, _zoomTimer.PercentTimePassed);
				_tempRot = Quaternion.Lerp (_goalRot, _originRot, _zoomTimer.PercentTimePassed);
				transform.SetPositionAndRotation (_tempPos, _tempRot);
				if (transform.position == _originPos) {
					_isDone = true;
				}
			}
		}

		if (Input.GetMouseButtonDown (1)) {
			ZoomOut ();
		}


//		if (_isZoomed) {
//			Camera.main.fieldOfView = Mathf.Lerp (_currentFOV, 20.0f, _zoomCurve.Evaluate(_zoomTimer.PercentTimePassed));
//		} else {
//			Camera.main.fieldOfView = Mathf.Lerp (_currentFOV, 60.0f, _zoomCurve.Evaluate(_zoomTimer.PercentTimePassed));
//		}
	}

	public void ZoomIn(Vector3 position, Vector3 rotation){
		if (_isDone) {
			_originPos = transform.position;
			_originRot = transform.rotation;
			_goalPos = position;
			_goalRot = Quaternion.Euler (rotation);
			_zoomTimer.Reset ();
			_isDone = false;
			_isZoomed = true;
		}
	}

	void ZoomOut(){
		if (_isZoomed) {
			_goalPos = transform.position;
			_goalRot = transform.rotation;
			_isZoomed = false;
			_isDone = false;
			_zoomTimer.Reset ();
		} 
	}
}
