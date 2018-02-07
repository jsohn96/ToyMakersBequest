using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreCameraControl : MonoBehaviour {
	[SerializeField] Transform _thisCameraHeighttContainer;
	[SerializeField] Camera _thisCamera;
	[SerializeField] MinMax _cameraMovementRange;
	[SerializeField] MinMax _cameraAngleRange;
	[SerializeField] AnimationCurve _cameraAngleCurve;

	float _currentCameraYPos;
	Vector3 _clickStartPosition;
	bool _isScrolling = false;
	Vector3 _cameraTempPos;
	Vector3 _cameraTempRot;
	Vector3 _scrollDirectionMultiplier = new Vector3(0f, -10f, 0f);

	[SerializeField] float _startYPos;

	void Start () {
		_cameraTempPos = _thisCameraHeighttContainer.transform.position;
		_cameraTempPos.y = _startYPos;
		_thisCameraHeighttContainer.transform.position = _cameraTempPos;
		_currentCameraYPos = _startYPos;
		CameraAngleCalculation ();
	}

	void Update(){
		if (_isScrolling) {
			ScrollCamera ();
		}
	}

	void ScrollCamera(){
		float acceleration = Input.mousePosition.y - _clickStartPosition.y;
		_thisCameraHeighttContainer.transform.Translate (_scrollDirectionMultiplier * (acceleration /Screen.height) * Time.deltaTime);
		_currentCameraYPos = _thisCameraHeighttContainer.transform.position.y;
		if (_currentCameraYPos > _cameraMovementRange.Max) {
			_cameraTempPos = _thisCameraHeighttContainer.transform.position;
			_cameraTempPos.y = _cameraMovementRange.Max;
			_currentCameraYPos = _cameraTempPos.y;
			_thisCameraHeighttContainer.transform.position = _cameraTempPos;
		} else if (_currentCameraYPos < _cameraMovementRange.Min) {
			_cameraTempPos = _thisCameraHeighttContainer.transform.position;
			_cameraTempPos.y = _cameraMovementRange.Min;
			_currentCameraYPos = _cameraTempPos.y;
			_thisCameraHeighttContainer.transform.position = _cameraTempPos;
		}
		CameraAngleCalculation ();
	}

	void CameraAngleCalculation(){
		_cameraTempRot = _thisCamera.transform.eulerAngles;
		float linMapCurvedAngle = _cameraAngleCurve.Evaluate( MathHelpers.LinMapTo01 (
			_cameraMovementRange.Min, 
			_cameraMovementRange.Max, 
			_currentCameraYPos));
		_cameraTempRot.x = MathHelpers.LinMapFrom01 (_cameraAngleRange.Min, _cameraAngleRange.Max, linMapCurvedAngle);
		_thisCamera.transform.rotation = Quaternion.Euler(_cameraTempRot);
	}

	public void InitializeScroll(){
		_clickStartPosition = Input.mousePosition;
		_isScrolling = true;
	}
		
	public void EndScroll(){
		_isScrolling = false;
	}
}
