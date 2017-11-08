using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using System;

public enum MusicBoxCameraMode {
	StaticFollowMode,
	WayPointMode,
	TraversalMode,
	Null
}

public class MusicBoxCameraManager : MonoBehaviour {

	MusicBoxCameraMode _musicBoxCameraMode = MusicBoxCameraMode.Null;

	[SerializeField] Transform _startPositionCamera, _endPositionCamera;
	[SerializeField] Transform _dancer;
	[SerializeField] MusicBoxManager _musicBoxManager;



	bool _cameraIsMoving = false;
	Timer _cameraMoveTimer;
	Quaternion _originRotation;
	Vector3 _originPosition;
	Quaternion _goalRotation;
	Vector3 _goalPosition;
	float _originFOV;
	float _goalFOV;
	bool _includeFOVShift = false;

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

	// Update is called once per frame
	void Update () {
		if (_cameraIsMoving) {
			if (!_cameraMoveTimer.IsOffCooldown) {
				_tempPos = Vector3.Slerp (_originPosition, _goalPosition, _cameraMoveTimer.PercentTimePassed);
				_tempRot = Quaternion.Slerp (_originRotation, _goalRotation, _cameraMoveTimer.PercentTimePassed);
				_tempRot = MathHelpers.KeepRotationLevel (_tempRot);
				transform.parent.SetPositionAndRotation (_tempPos, _tempRot);
				if (_includeFOVShift) {
					Camera.main.fieldOfView = Mathf.Lerp (_originFOV, _goalFOV, _cameraMoveTimer.PercentTimePassed);
				}
			} else {
				_cameraIsMoving = false;
				_includeFOVShift = false;
			}
		}
		// Checks to see if camera should focus on circle or dancer
		FocusCameraOnCircle ();

	}

	public void MoveToWayPoint(Transform wayPointTransform, float duration, float fov = 0.0f, bool isTraversal = false){
		if (isTraversal) {
			SwitchCameraMode (MusicBoxCameraMode.TraversalMode);
		} else {
			SwitchCameraMode (MusicBoxCameraMode.WayPointMode);
		}
		_originPosition = transform.parent.position;
		_originRotation = transform.parent.rotation;
		_goalPosition = wayPointTransform.position;
		_goalRotation = wayPointTransform.rotation;

		_originFOV = Camera.main.fieldOfView;
		if (fov != 0.0f) {
			_goalFOV = fov;
			_includeFOVShift = true;
		}
		_cameraMoveTimer.CooldownTime = duration;
		_cameraMoveTimer.Reset ();
		_cameraIsMoving = true;
	}

	// Expected to be called from Camera Timeline, except for waypoint Mode
	// Call MoveToWayPoint() Instead for waypoint
	public void SwitchCameraMode(MusicBoxCameraMode newCameraMode){
		if (_musicBoxCameraMode != newCameraMode) {
			_musicBoxCameraMode = newCameraMode;
			switch (_musicBoxCameraMode) {
			case MusicBoxCameraMode.StaticFollowMode:
				_objectRotator.enabled = false;
				StartCoroutine (WaitForTimer (_cameraMoveTimer, EnableFollowScripts));
				break;
			case MusicBoxCameraMode.WayPointMode:
				_objectRotator.enabled = false;
				_targetFieldOfViewScript.enabled = false;
				_lookAtTargetScript.enabled = false;
				break;
			case MusicBoxCameraMode.TraversalMode:
				_targetFieldOfViewScript.enabled = false;
				_lookAtTargetScript.enabled = false;
				_objectRotator.enabled = true;
				break;
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


