using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreCameraControl : MonoBehaviour {
	[SerializeField] AltTheatre _altTheatre;

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

	[Header("Camera Init Values")]
	[SerializeField] Vector3 _cameraZoomedOutViewPos;
	[SerializeField] float _cameraZoomedOutFOV = 60f;


	[SerializeField] Vector3 _cameraStartPos;
	[SerializeField] Vector3 _cameraStartRot;
	[SerializeField] float _cameraStartFOV = 28f;

	bool _initZoom = false;
	bool _initClick = false;

	void Start () {
		if (AltTheatre.currentSate == TheatreState.waitingToStart) {
			_thisCameraHeighttContainer.position = _cameraZoomedOutViewPos;
			_thisCamera.fieldOfView = _cameraZoomedOutFOV;
		} else {
			_initZoom = true;
			_thisCameraHeighttContainer.transform.position = _cameraStartPos;
			_currentCameraYPos = _cameraStartPos.y;
			CameraAngleCalculation ();
		}

	}

	void Update(){
		if (!_initZoom) {
			if (Input.GetMouseButtonDown (0) && !_initClick) {
				_initClick = true;
				AltTheatre.currentSate++;
				_altTheatre.CheckStateMachine ();
				StartCoroutine (ZoomInCamera ());
			}
		} else {
			if (_isScrolling) {
				ScrollCamera ();
			}
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

	IEnumerator ZoomInCamera(){
		float timer = 0f;
		float duration = 5f;
		_cameraTempRot = _thisCamera.transform.eulerAngles;
		while (timer < duration) {
			timer += Time.deltaTime;
			_thisCameraHeighttContainer.position = Vector3.Lerp (_cameraZoomedOutViewPos, _cameraStartPos, timer / duration);
			_thisCamera.transform.eulerAngles = Vector3.Lerp (_cameraTempRot, _cameraStartRot, timer / duration);
			_thisCamera.fieldOfView = Mathf.Lerp (_cameraZoomedOutFOV, _cameraStartFOV, timer / duration);
			yield return null;
		}
		_thisCameraHeighttContainer.position = _cameraStartPos;
		_thisCamera.transform.eulerAngles = _cameraStartRot;
		_thisCamera.fieldOfView = _cameraStartFOV;
		yield return null;
		_isScrolling = false;
		_initZoom = true;
	}
}
