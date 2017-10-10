using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxCameraInitialize : MonoBehaviour {
	[SerializeField] Transform _otherPositionCamera;
	bool _once = false;
	Timer _cameraInitTime;
	Quaternion _originRotation;
	Vector3 _originPosition;

	Vector3 _tempPos;
	Quaternion _tempRot;
	// Use this for initialization
	void Start () {
		_cameraInitTime = new Timer (3.0f);
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

		if (_once && !_cameraInitTime.IsOffCooldown) {
			_tempPos = Vector3.Slerp (_originPosition, _otherPositionCamera.position, _cameraInitTime.PercentTimePassed);
			_tempRot = Quaternion.Lerp (_originRotation, _otherPositionCamera.rotation, _cameraInitTime.PercentTimePassed);

			transform.SetPositionAndRotation (_tempPos, _tempRot);
		}
	}
}
