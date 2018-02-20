using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreCameraControl : MonoBehaviour {
	[SerializeField] bool _discreteMode = false;
	float _acceleration = 0f;

	[SerializeField] AltTheatre _altTheatre;

	[SerializeField] Transform _thisCameraHeighttContainer;
	[SerializeField] Camera _thisCamera;
	[SerializeField] MinMax _cameraMovementRange;
	[SerializeField] MinMax _cameraAngleRange;
	[SerializeField] MinMax _cameraFOVRange = new MinMax (28f, 60f);
	[SerializeField] AnimationCurve _cameraAngleCurve;
	[SerializeField] AnimationCurve _cameraFOVCurve1;
	[SerializeField] AnimationCurve _cameraFOVCurve2;
	bool _usingFOV1 = true;

	float _currentCameraYPos;
	Vector3 _clickStartPosition;
	bool _isScrolling = false;
	Vector3 _cameraTempPos;
	Vector3 _cameraTempRot;
	[SerializeField] Vector3 _bottomCameraPos;
	Vector3 _scrollDirectionMultiplier = new Vector3(0f, -10f, 0f);

	[Header("Camera Init Values")]
	[SerializeField] Vector3 _cameraZoomedOutViewPos;
	[SerializeField] float _cameraZoomedOutFOV = 60f;


	[SerializeField] Vector3 _cameraStartPos;
	[SerializeField] Vector3 _cameraStartRot;
	[SerializeField] float _cameraStartFOV = 28f;

	bool _initZoom = false;
	bool _initClick = false;
	bool _disableScrollFOV = true;
	bool _disableAllScroll = false;

	bool _scrollEasingToHalt = false;

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

		if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			_acceleration = 0.03f;
			_isScrolling = true;
			_scrollEasingToHalt = false;
			_scrollDirectionMultiplier.y = 10f;
		} else if (Input.GetKeyDown (KeyCode.DownArrow)|| Input.GetKeyDown(KeyCode.S)) {
			_acceleration = 0.03f;
			_isScrolling = true;
			_scrollEasingToHalt = false;
			_scrollDirectionMultiplier.y = -10f;
		}

		if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) {
			_scrollEasingToHalt = true;
		}

		if (_scrollEasingToHalt) {
			_acceleration -= 0.002f;
			if (_acceleration < 0.0f) {
				_scrollEasingToHalt = false;
				_isScrolling = false;
				_acceleration = 0.0f;
			}
		}
	}

	void ScrollCamera(){
		if (!_disableAllScroll) {
			if (!_discreteMode) {
				_acceleration = Input.mousePosition.y - _clickStartPosition.y;
				_thisCameraHeighttContainer.transform.Translate (_scrollDirectionMultiplier * (_acceleration / Screen.height) * Time.deltaTime);
			} else {
				_thisCameraHeighttContainer.transform.Translate (_scrollDirectionMultiplier * (_acceleration) * Time.deltaTime);
			}


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
	}

	void CameraAngleCalculation(){
		_currentCameraYPos = _thisCamera.transform.position.y;
		_cameraTempRot = _thisCamera.transform.eulerAngles;
		float nonCurvedLinMapValue = MathHelpers.LinMapTo01 (
			                             _cameraMovementRange.Min, 
			                             _cameraMovementRange.Max, 
			                             _currentCameraYPos);
		float linMapCurvedAngle = _cameraAngleCurve.Evaluate(nonCurvedLinMapValue);
		_cameraTempRot.x = MathHelpers.LinMapFrom01 (_cameraAngleRange.Min, _cameraAngleRange.Max, linMapCurvedAngle);
		_thisCamera.transform.rotation = Quaternion.Euler(_cameraTempRot);

		//Use the camera lin map calculation for FOV calculations as well
		if (!_disableScrollFOV) {
			float linMapCurvedFOV;
			if (_usingFOV1) {
				linMapCurvedFOV = _cameraFOVCurve1.Evaluate (nonCurvedLinMapValue);
			} else {
				linMapCurvedFOV = _cameraFOVCurve2.Evaluate (nonCurvedLinMapValue);
			}
			_thisCamera.fieldOfView = MathHelpers.LinMapFrom01 (_cameraFOVRange.Min, _cameraFOVRange.Max, linMapCurvedFOV);
		}
	}
		

	public void InitializeScroll(){
		_clickStartPosition = Input.mousePosition;
		_isScrolling = true;
	}
		
	public void EndScroll(){
		_isScrolling = false;
	}

	public void EnableScrollFOV(){
		StartCoroutine (AdjustToScrollFOV());
	}

	public void MoveCameraToLookAtTank(){
		StartCoroutine (MoveToTank ());
	}

	IEnumerator MoveToTank(){
		float timer = 0f;
		float duration = 4f;
		_cameraTempPos = _thisCameraHeighttContainer.transform.position;
//		_bottomCameraPos = _cameraTempPos;
//		_bottomCameraPos.y = _cameraMovementRange.Min;
		_disableAllScroll = true;
		while (timer < duration) {
			timer += Time.deltaTime;
			_thisCameraHeighttContainer.transform.position = Vector3.Slerp (_cameraTempPos, _bottomCameraPos, timer / duration);
			CameraAngleCalculation ();
			yield return null;
		}
		_thisCameraHeighttContainer.transform.position = _bottomCameraPos;
		CameraAngleCalculation ();
		yield return null;
		_altTheatre.MoveToNext ();
	}

	IEnumerator AdjustToScrollFOV(){
		_disableAllScroll = false;
		ScrollCamera ();
		float timer = 0f;
		float duration = 2f;
		float currentFOV = _thisCamera.fieldOfView;
		float nonCurvedLinMapValue;
		float linMapCurvedFOV;
		float goalFOV;
		while (timer < duration) {
			timer += Time.deltaTime;
			nonCurvedLinMapValue = MathHelpers.LinMapTo01 (
				_cameraMovementRange.Min, 
				_cameraMovementRange.Max, 
				_currentCameraYPos);
			if (_usingFOV1) {
				linMapCurvedFOV = _cameraFOVCurve1.Evaluate (nonCurvedLinMapValue);
			} else {
				linMapCurvedFOV = _cameraFOVCurve2.Evaluate (nonCurvedLinMapValue);
			}
			goalFOV = MathHelpers.LinMapFrom01 (_cameraFOVRange.Min, _cameraFOVRange.Max, linMapCurvedFOV);
			_thisCamera.fieldOfView = Mathf.Lerp (currentFOV, goalFOV, timer / duration);
			yield return null;
		}
		_disableScrollFOV = false;
		yield return null;
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
