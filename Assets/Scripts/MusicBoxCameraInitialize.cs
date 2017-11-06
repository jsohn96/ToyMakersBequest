using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using System;

public class MusicBoxCameraInitialize : MonoBehaviour {
	[SerializeField] Transform _startPositionCamera;
	[SerializeField] Transform _dancer;
	[SerializeField] MusicBoxManager _musicBoxManager;
	bool _once = false;
	Timer _cameraInitTime;
	Quaternion _originRotation;
	Vector3 _originPosition;

	Vector3 _tempPos;
	Quaternion _tempRot;

	TargetFieldOfView _targetFieldOfViewScript;
	LookatTarget _lookAtTargetScript;

	bool _zoomOut = false;
	float _tempfov;
	float _tempGoalfov;
	Timer _zoomOutTimer;

	ObjectRotator _objectRotator;
	PathNode _cachedPathNode = null;

	// Use this for initialization
	void Start () {
		_cameraInitTime = new Timer (3.0f);
		_targetFieldOfViewScript = GetComponent<TargetFieldOfView> ();
		_lookAtTargetScript = transform.parent.GetComponent<LookatTarget> ();
		_zoomOutTimer = new Timer (1.5f);

		_objectRotator = transform.parent.parent.GetComponent<ObjectRotator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!_once && Input.GetKeyDown(KeyCode.Space)) {
			//transform.parent = null;
			_objectRotator.enabled = false;
			_originPosition = transform.parent.position;
			_originRotation = transform.parent.rotation;
			_cameraInitTime.Reset ();
			_once = true;
		}

		if (!_zoomOut) {
			if (_once) {
				Debug.Log ("Cooldown: "+_cameraInitTime.IsOffCooldown);
				if (!_cameraInitTime.IsOffCooldown) {
					_tempPos = Vector3.Slerp (_originPosition, _startPositionCamera.position, _cameraInitTime.PercentTimePassed);
					_tempRot = Quaternion.Slerp (_originRotation, _startPositionCamera.rotation, _cameraInitTime.PercentTimePassed);
					_tempRot = MathHelpers.KeepRotationLevel (_tempRot);
					transform.parent.SetPositionAndRotation (_tempPos, _tempRot);
				} 
				if (Input.GetKeyDown (KeyCode.S)) {
					StartCoroutine (WaitForTimer (_cameraInitTime, EnableFollowScripts));
				}
			}
		} else {
			if (!_zoomOutTimer.IsOffCooldown) {
				Camera.main.fieldOfView = Mathf.Lerp (_tempfov, _tempGoalfov, _zoomOutTimer.PercentTimePassed);
				_tempRot = Quaternion.Slerp (_startPositionCamera.rotation, _originRotation, _zoomOutTimer.PercentTimePassed);
				_tempRot = MathHelpers.KeepRotationLevel (_tempRot);
				_tempPos = Vector3.Slerp (_startPositionCamera.position, _originPosition, _zoomOutTimer.PercentTimePassed);
				transform.parent.SetPositionAndRotation (_tempPos, _tempRot);
			}
		}

		//Have the camera focus on the circle that the dancer is on, instead of dancer
		// An attempt to reduce nausea
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


	void ChangeZoom(CamerafovAmountChange e){
		_targetFieldOfViewScript.enabled = false;
		_tempfov = Camera.main.fieldOfView;
		_tempGoalfov = e.FovAmount;
		_zoomOutTimer.Reset ();
		_zoomOut = true;
	}

	void OnEnable(){
		Events.G.AddListener<CamerafovAmountChange> (ChangeZoom);
	}

	void OnDisable(){
		Events.G.RemoveListener<CamerafovAmountChange> (ChangeZoom);
	}
}


