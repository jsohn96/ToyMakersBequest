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
	[SerializeField] MinMax _cameraRotation = new MinMax(-20f, 20f);
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
	float _angleDirectionMultiplier = 1f;

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
	bool _angleMode = false;

	[SerializeField] BoxCollider _surfaceBoxCollider;

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

	public void Activate(){
		if (!_initClick) {
			_initClick = true;
			AltTheatre.currentSate++;
			_altTheatre.CheckStateMachine ();
			StartCoroutine (ZoomInCamera ());
		}
	}

	void Update(){
		if (_initZoom) {
			if (_isScrolling) {
				ScrollCamera ();
			}
		}



		if (_scrollEasingToHalt) {
			if (!_angleMode) {
				_acceleration -= 0.03f * Time.deltaTime;
			} else {
				_acceleration -= 20f * Time.deltaTime;
			}
			if (_acceleration < 0.0f) {
				_scrollEasingToHalt = false;
				_isScrolling = false;
				_acceleration = 0.0f;
			}
		}
	}

	public void OnPointerDown(Direction whichDirection){
		_isScrolling = true;
		_scrollEasingToHalt = false;
		switch (whichDirection) {
		case Direction.up:
			_acceleration = 0.015f;
			_scrollDirectionMultiplier.y = 10f;
			_angleMode = false;
			break;
		case Direction.down:
			_acceleration = 0.015f;
			_scrollDirectionMultiplier.y = -10f;
			_angleMode = false;
			break;
		case Direction.left:
			_acceleration = 10f;
			_scrollDirectionMultiplier.y = -1f;
			_angleMode = true;
			break;
		case Direction.right:
			_acceleration = 10f;
			_scrollDirectionMultiplier.y = 1f;
			_angleMode = true;
			break;
		default:
			break;
		}
	}

	public void OnPointerUp(Direction whichDirection){
		_scrollEasingToHalt = true;
	}

	void ScrollCamera(){
		if (!_disableAllScroll) {
			if (!_discreteMode) {
				_acceleration = Input.mousePosition.y - _clickStartPosition.y;
				_thisCameraHeighttContainer.transform.Translate (_scrollDirectionMultiplier * (_acceleration / Screen.height) * Time.deltaTime);
			} else {
				if (!_angleMode) {
					_thisCameraHeighttContainer.transform.Translate (_scrollDirectionMultiplier * (_acceleration) * Time.deltaTime);
				} else {
					_thisCamera.transform.Rotate (_acceleration * _scrollDirectionMultiplier * Time.deltaTime, Space.World);
				}
			}

			if (!_angleMode) {
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
			} else {
				Vector3 tempAngle = _thisCamera.transform.localEulerAngles;
				float tempAngleY = tempAngle.y;
				if (tempAngleY < 180f) {
					if (tempAngleY > _cameraRotation.Max) {
						tempAngle.y = _cameraRotation.Max;
						_thisCamera.transform.localEulerAngles = tempAngle;
					} 
				} else {
					tempAngleY = tempAngleY - 360f;
					if (tempAngleY < _cameraRotation.Min) {
						tempAngle.y = _cameraRotation.Min;
						_thisCamera.transform.localEulerAngles = tempAngle;
					}
				}
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
		Vector3 originEuler = _thisCamera.transform.eulerAngles;
		Vector3 goalEuler = new Vector3 (0f, 0f, 0f);
//		_bottomCameraPos = _cameraTempPos;
//		_bottomCameraPos.y = _cameraMovementRange.Min;
		_disableAllScroll = true;
		while (timer < duration) {
			timer += Time.deltaTime;
			_thisCameraHeighttContainer.transform.position = Vector3.Slerp (_cameraTempPos, _bottomCameraPos, timer / duration);

			CameraAngleCalculation ();
			originEuler.x = _thisCamera.transform.eulerAngles.x;
			goalEuler.x = originEuler.x;
			_thisCamera.transform.rotation = Quaternion.Slerp (Quaternion.Euler(originEuler), Quaternion.Euler(goalEuler), timer / duration);
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
