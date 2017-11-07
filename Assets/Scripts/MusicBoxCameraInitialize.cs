using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using System;

public enum MusicBoxCameraMode {
	StaticFollowMode,
	Static,
	Null
}

public class MusicBoxCameraInitialize : MonoBehaviour {

	MusicBoxCameraMode _musicBoxCameraMode = MusicBoxCameraMode.Null;

	[SerializeField] Transform _startPositionCamera;
	[SerializeField] Transform _dancer;
	[SerializeField] MusicBoxManager _musicBoxManager;

	bool _initialized = false;

	bool _cameraIsMoving = false;
	Timer _cameraMoveTimer;
	Quaternion _originRotation;
	Vector3 _originPosition;
	Quaternion _goalRotation;
	Vector3 _goalPosition;

	Vector3 _tempPos;
	Quaternion _tempRot;

	TargetFieldOfView _targetFieldOfViewScript;
	LookatTarget _lookAtTargetScript;

	bool _zooming = false;
	float _tempfov;
	float _tempGoalfov;
	Timer _zoomTimer;

	ObjectRotator _objectRotator;
	PathNode _cachedPathNode = null;

	// Use this for initialization
	void Start () {
		_cameraMoveTimer = new Timer (3.0f);
		_targetFieldOfViewScript = GetComponent<TargetFieldOfView> ();
		_lookAtTargetScript = transform.parent.GetComponent<LookatTarget> ();
		_zoomTimer = new Timer (1.5f);

		_objectRotator = transform.parent.parent.GetComponent<ObjectRotator> ();
	}

	//Brings the camera to the start position for the level
	void InitializeMusicBoxLevel(){
		_initialized = true;
		_objectRotator.enabled = false;
		_originPosition = transform.parent.position;
		_originRotation = transform.parent.rotation;
		_goalPosition = _startPositionCamera.position;
		_goalRotation = _startPositionCamera.rotation;
		_cameraMoveTimer.Reset ();
		_cameraIsMoving = true;
	}

	// Update is called once per frame
	void Update () {
		if (!_initialized && Input.GetKeyDown(KeyCode.Space)) {
			//Bring the camera to the start position for the level
			InitializeMusicBoxLevel ();
		}

		if (_cameraIsMoving) {
			if (!_cameraMoveTimer.IsOffCooldown) {
				_tempPos = Vector3.Slerp (_originPosition, _goalPosition, _cameraMoveTimer.PercentTimePassed);
				_tempRot = Quaternion.Slerp (_originRotation, _goalRotation, _cameraMoveTimer.PercentTimePassed);
				_tempRot = MathHelpers.KeepRotationLevel (_tempRot);
				transform.parent.SetPositionAndRotation (_tempPos, _tempRot);
			} else {
				_cameraIsMoving = false;
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				StartCoroutine (WaitForTimer (_cameraMoveTimer, EnableFollowScripts));
			}
		}

		// Checks to see if camera should focus on circle or dancer
		FocusCameraOnCircle ();

	}


	void SwitchCameraMode(MusicBoxCameraMode newCameraMode){
		if (_musicBoxCameraMode != newCameraMode) {
			switch (_musicBoxCameraMode) {
			default:
				break;
			}
		}
	}

	void EnableFollowScripts(){
		_targetFieldOfViewScript.enabled = true;
		_lookAtTargetScript.enabled = true;
	}

	// passes timer and function as parameter to call until the timer is done
	IEnumerator WaitForTimer(Timer _timer, Action action){
		while (!_timer.IsOffCooldown) {
			yield return null;
		}
		action ();
		yield return null;
	}

	//Have the camera focus on the circle that the dancer is on, instead of dancer
	// An attempt to reduce nausea
	void FocusCameraOnCircle() {
		if (_cachedPathNode != _musicBoxManager.GetActivePathNetwork ()._curNode) {
			_cachedPathNode = _musicBoxManager.GetActivePathNetwork ()._curNode;

			if (_musicBoxManager.GetActivePathNetwork ()._curNode.GetControlColor() != ButtonColor.None) {
				if (_targetFieldOfViewScript.enabled) {
					_targetFieldOfViewScript.SetTargetOverride (_cachedPathNode.transform);
				}
				_lookAtTargetScript.SetTargetOverride (_cachedPathNode.transform);
			} else {
				_targetFieldOfViewScript.SetTargetOverride (_dancer);
				_lookAtTargetScript.SetTargetOverride (_dancer);
			}
		}
	}


	void ChangeZoom(CamerafovAmountChange e){
		_targetFieldOfViewScript.enabled = false;
		_tempfov = Camera.main.fieldOfView;
		_tempGoalfov = e.FovAmount;
		_zoomTimer.Reset ();
		_zooming = true;
	}

	void OnEnable(){
		Events.G.AddListener<CamerafovAmountChange> (ChangeZoom);
	}

	void OnDisable(){
		Events.G.RemoveListener<CamerafovAmountChange> (ChangeZoom);
	}
}


