using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class MusicBoxCameraInitialize : MonoBehaviour {
	[SerializeField] Transform _otherPositionCamera;
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

	// Use this for initialization
	void Start () {
		_cameraInitTime = new Timer (3.0f);
		_targetFieldOfViewScript = GetComponent<TargetFieldOfView> ();
		_lookAtTargetScript = GetComponent<LookatTarget> ();
		_zoomOutTimer = new Timer (1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!_once && Input.GetKeyDown(KeyCode.Space)) {
			transform.parent = null;
			_originPosition = transform.position;
			_originRotation = transform.rotation;
			_cameraInitTime.Reset ();
			_once = true;
		}
		if (!_zoomOut) {
			if (_once) {
				if (!_cameraInitTime.IsOffCooldown) {
					_tempPos = Vector3.Slerp (_originPosition, _otherPositionCamera.position, _cameraInitTime.PercentTimePassed);
					_tempRot = Quaternion.Lerp (_originRotation, _otherPositionCamera.rotation, _cameraInitTime.PercentTimePassed);
					transform.SetPositionAndRotation (_tempPos, _tempRot);
				} else if (Input.GetKeyDown (KeyCode.S)) {
					_targetFieldOfViewScript.enabled = true;
					_lookAtTargetScript.enabled = true;
				}
			}
		} else {
			if (!_zoomOutTimer.IsOffCooldown) {
				Camera.main.fieldOfView = Mathf.Lerp (_tempfov, _tempGoalfov, _zoomOutTimer.PercentTimePassed);
				_tempRot = Quaternion.Lerp (_otherPositionCamera.rotation, _originRotation, _zoomOutTimer.PercentTimePassed);
				_tempPos = Vector3.Slerp (_otherPositionCamera.position, _originPosition, _zoomOutTimer.PercentTimePassed);
				transform.SetPositionAndRotation (_tempPos, _tempRot);
			}
		}
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
