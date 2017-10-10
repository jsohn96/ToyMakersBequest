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

	// Use this for initialization
	void Start () {
		_cameraInitTime = new Timer (3.0f);

		_targetFieldOfViewScript = GetComponent<TargetFieldOfView> ();
		_lookAtTargetScript = GetComponent<LookatTarget> ();
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
	}
}
