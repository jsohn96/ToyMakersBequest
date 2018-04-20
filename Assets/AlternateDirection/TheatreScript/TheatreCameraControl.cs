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
	[Header("Camera Goal Position Values")]
	[SerializeField] Vector3 _bottomCameraPos;
	[SerializeField] Vector3 _stageCameraPos;
	[SerializeField] Vector3 _stageCameraPos2;
	Vector3 _scrollDirectionMultiplier = new Vector3(0f, -10f, 0f);
	float _angleDirectionMultiplier = 1f;

	[Header("Camera Init Values")]
	[SerializeField] Vector3 _cameraZoomedOutViewPos;
	[SerializeField] float _cameraZoomedOutFOV = 60f;
	Quaternion _cameraZoomedOutRot;


	[SerializeField] Vector3 _cameraStartPos;
	[SerializeField] Vector3 _cameraStartRot;

	[Header("Slider Slide Camera View Values")]
	[SerializeField] Vector3 _sliderSlideCamPos;
	[SerializeField] Vector3 _sliderSlideCamRot;
		
	[SerializeField] float _cameraStartFOV = 28f;

	bool _initZoom = false;
	bool _initClick = false;
	bool _zoomedOut = false;
	bool _isZooming = false;
	bool _disableScrollFOV = true;
	bool _disableAllScroll = false;

	bool _scrollEasingToHalt = false;
	bool _angleMode = false;

	bool _up = true, _down = true, _left = true, _right = true;

	[SerializeField] BoxCollider _surfaceBoxCollider;

	[SerializeField] TraversalUI _traversalUI;
	[SerializeField] TheatreText _theatreText;
	[SerializeField] TapSoundPlayer _tapSoundPlayer;

	bool _isLerping = false;

	void Start () {
		if (AltTheatre.currentSate == TheatreState.waitingToStart) {
			_thisCameraHeighttContainer.position = _cameraZoomedOutViewPos;
			_thisCamera.fieldOfView = _cameraZoomedOutFOV;
			_cameraZoomedOutRot = _thisCamera.transform.rotation;
		} else {
			_initZoom = true;
			_thisCameraHeighttContainer.transform.position = _sliderSlideCamPos;
			_currentCameraYPos = _sliderSlideCamPos.y;
			CameraAngleCalculation ();
		}
	}

	public void Activate(){
		_initZoom = true;
		AltTheatre.currentSate++;
//		_altTheatre.CheckStateMachine ();
		StartCoroutine (ZoomInCamera ());
	}


	public void ZoomOut(){
		if (!_zoomedOut) {
			_isZooming = true;
			_zoomedOut = true;
			StartCoroutine (ZoomOutCamera ());
		}
	}

	public void ZoomIn(){
		if (_zoomedOut) {
			_isZooming = true;
			StartCoroutine (ZoomInCamera ());
		}
	}

	void Update(){
		if (_initZoom && !_isZooming) {
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

		if (Input.GetKeyDown (KeyCode.F)) {
			MoveCameraToStartPosition ();
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
			if(!_down){
				_down = true;
				_traversalUI.FadeIn(false, 1);
			}
			break;
		case Direction.down:
			_acceleration = 0.015f;
			_scrollDirectionMultiplier.y = -10f;
			_angleMode = false;
			if(!_up){
				_up = true;
				_traversalUI.FadeIn(false, 0);
			}
			break;
		case Direction.left:
		Debug.Log("left is being called th");
			_acceleration = 10f;
			_scrollDirectionMultiplier.y = -1f;
			_angleMode = true;
			if(!_right){
				_right = true;
				_traversalUI.FadeIn(false, 3);
			}
			break;
		case Direction.right:
			_acceleration = 10f;
			_scrollDirectionMultiplier.y = 1f;
			_angleMode = true;
			if(!_left){
				_left = true;
				_traversalUI.FadeIn(false, 2);
			}
			break;
		default:
			break;
		}
	}

	public void OnPointerUp(Direction whichDirection){
		_scrollEasingToHalt = true;
	}

	void ScrollCamera(bool lerpIt = false){
		if (lerpIt) {
			_isLerping = true;
		}
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
					if(_up){
						_up = false;
						_traversalUI.FadeOut(false, 0);
					}
					_cameraTempPos = _thisCameraHeighttContainer.transform.position;
					_cameraTempPos.y = _cameraMovementRange.Max;
					_currentCameraYPos = _cameraTempPos.y;
					_thisCameraHeighttContainer.transform.position = _cameraTempPos;
				} else if (_currentCameraYPos < _cameraMovementRange.Min) {
					if(_down){
						_down = false;
						_traversalUI.FadeOut(false, 1);
					}
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
						if(_right){
							_right = false;
							_traversalUI.FadeOut(false, 3);
						}
						tempAngle.y = _cameraRotation.Max;
						_thisCamera.transform.localEulerAngles = tempAngle;
					} 
				} else {
					tempAngleY = tempAngleY - 360f;
					if (tempAngleY < _cameraRotation.Min) {
						if(_left){
							_left = false;
							_traversalUI.FadeOut(false, 2);
						}
						tempAngle.y = _cameraRotation.Min;
						_thisCamera.transform.localEulerAngles = tempAngle;
					}
				}
			}
			CameraAngleCalculation (lerpIt);
		}
	}

	void CameraAngleCalculation(bool lerpIt = false){
		if (lerpIt) {
			_isLerping = true;
		}
		_currentCameraYPos = _thisCamera.transform.position.y;
		_cameraTempRot = _thisCamera.transform.eulerAngles;
		float nonCurvedLinMapValue = MathHelpers.LinMapTo01 (
			                             _cameraMovementRange.Min, 
			                             _cameraMovementRange.Max, 
			                             _currentCameraYPos);
		float linMapCurvedAngle = _cameraAngleCurve.Evaluate(nonCurvedLinMapValue);
		_cameraTempRot.x = MathHelpers.LinMapFrom01 (_cameraAngleRange.Min, _cameraAngleRange.Max, linMapCurvedAngle);

		if (lerpIt) {
			StartCoroutine (LerpAngle (_cameraTempRot));
		} else if(!_isLerping){
			_thisCamera.transform.rotation = Quaternion.Euler (_cameraTempRot);
		}

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
		
	IEnumerator LerpAngle(Vector3 tempRotate){
		float timer = 0f;
		float duration = 2f;
		Quaternion currentRotation = _thisCamera.transform.rotation;
		Quaternion tempRotationQuat = Quaternion.Euler (tempRotate);
		while(timer < duration){
			_thisCamera.transform.rotation = Quaternion.Lerp(currentRotation, tempRotationQuat, timer/duration);
			timer += Time.deltaTime;
			yield return null;
		}
		_thisCamera.transform.rotation = tempRotationQuat;
		_isLerping = false;
		yield return null;
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
		float duration = 6f;
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
		CameraAngleCalculation (true);
		yield return null;
		_altTheatre.MoveToNext ();
	}

	public void MoveCameraToLookAtStage(){
		StartCoroutine (MoveToStage ());
	}

	public void MoveCameraToLookAtStage2(){
		StartCoroutine (MoveToStage2 ());
	}

	public void MoveCameraToStartPosition(){
		StartCoroutine (MoveToStartView ());
	}

	IEnumerator MoveToStage(){
		float timer = 0f;
		float duration = 8f;
		_cameraTempPos = _thisCameraHeighttContainer.transform.position;
		Vector3 originEuler = _thisCamera.transform.eulerAngles;
		Vector3 goalEuler = new Vector3 (0f, 0f, 0f);
		//		_bottomCameraPos = _cameraTempPos;
		//		_bottomCameraPos.y = _cameraMovementRange.Min;
		_disableAllScroll = true;
		while (timer < duration) {
			timer += Time.deltaTime;
			_thisCameraHeighttContainer.transform.position = Vector3.Slerp (_cameraTempPos, _stageCameraPos, timer / duration);

			CameraAngleCalculation ();
			originEuler.x = _thisCamera.transform.eulerAngles.x;
			goalEuler.x = originEuler.x;
			_thisCamera.transform.rotation = Quaternion.Slerp (Quaternion.Euler(originEuler), Quaternion.Euler(goalEuler), timer / duration);
			yield return null;
		}
		_thisCameraHeighttContainer.transform.position = _stageCameraPos;
		CameraAngleCalculation (true);
		yield return null;
		//_altTheatre.MoveToNext ();
	}

	IEnumerator MoveToStage2(){
		float timer = 0f;
		float duration = 5f;
		_cameraTempPos = _thisCameraHeighttContainer.transform.position;
		Vector3 originEuler = _thisCamera.transform.eulerAngles;
		Vector3 goalEuler = new Vector3 (18.292f, 0f, 0f);
		//		_bottomCameraPos = _cameraTempPos;
		//		_bottomCameraPos.y = _cameraMovementRange.Min;
		_disableAllScroll = true;
		while (timer < duration) {
			timer += Time.deltaTime;
			_thisCameraHeighttContainer.transform.position = Vector3.Slerp (_cameraTempPos, _stageCameraPos2, timer / duration);

			CameraAngleCalculation ();
			originEuler.x = _thisCamera.transform.eulerAngles.x;
			goalEuler.x = originEuler.x;
			_thisCamera.transform.rotation = Quaternion.Slerp (Quaternion.Euler(originEuler), Quaternion.Euler(goalEuler), timer / duration);
			yield return null;
		}
		_thisCameraHeighttContainer.transform.position = _stageCameraPos2;
		CameraAngleCalculation (true);
		yield return null;
		_altTheatre.MoveToNext ();
	}

	IEnumerator MoveToStartView(){
		yield return new WaitForSeconds (1.5f);

		float timer = 0f;
		float duration = 3f;
		while (timer < duration) {
			timer += Time.deltaTime;
			_thisCameraHeighttContainer.transform.position = Vector3.Slerp (_sliderSlideCamPos, _cameraStartPos, timer / duration);
			_thisCamera.transform.rotation = Quaternion.Lerp (Quaternion.Euler(_sliderSlideCamRot), Quaternion.Euler(_cameraStartRot), timer / duration);
			yield return null;
		}
		_thisCameraHeighttContainer.transform.position = _cameraStartPos;
		_thisCamera.transform.rotation = Quaternion.Euler (_cameraStartRot);
		yield return null;
	}

	IEnumerator AdjustToScrollFOV(){
		while (_zoomedOut) {
			yield return null;
		}
		_disableAllScroll = false;
		ScrollCamera (true);
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
		float duration;
		if (!_initClick) {
			_initClick = true;
			duration = 9f;
		} else {
			duration = 5f;
		}
		Quaternion tempRot = _thisCamera.transform.rotation;
		float cameraTempFOV = _thisCamera.fieldOfView;
		Quaternion startRot = Quaternion.Euler (_sliderSlideCamRot);
		Vector3 cameraTempPos = _thisCamera.transform.position;
		while (timer < duration) {
			timer += Time.deltaTime;
			_thisCameraHeighttContainer.position = Vector3.Lerp (cameraTempPos, _sliderSlideCamPos, timer / duration);
			_thisCamera.transform.rotation = Quaternion.Lerp (tempRot, startRot, timer / duration);
			_thisCamera.fieldOfView = Mathf.Lerp (cameraTempFOV, _cameraStartFOV, timer / duration);
			yield return null;
		}
		_thisCameraHeighttContainer.position = _sliderSlideCamPos;
		_thisCamera.transform.eulerAngles = _sliderSlideCamRot;
		_thisCamera.fieldOfView = _cameraStartFOV;
		yield return null;
		_isScrolling = false;
		_isZooming = false;
		_zoomedOut = false;
		_tapSoundPlayer._activateSounds = true;
		_initClick = true;
		_traversalUI.FadeIn (true);
	}

	IEnumerator ZoomOutCamera(){
		_tapSoundPlayer._activateSounds = false;
		_traversalUI.FadeOut (true);
		float timer = 0f;
		float duration = 5f;
		_sliderSlideCamPos = _thisCameraHeighttContainer.position;
		_cameraStartFOV = _thisCamera.fieldOfView;
		_sliderSlideCamRot = _thisCamera.transform.eulerAngles;
		Quaternion startRot = Quaternion.Euler (_sliderSlideCamRot);

		while (timer < duration) {
			timer += Time.deltaTime;
			_thisCameraHeighttContainer.position = Vector3.Lerp (_sliderSlideCamPos, _cameraZoomedOutViewPos, timer / duration);
			_thisCamera.transform.rotation = Quaternion.Lerp (startRot, _cameraZoomedOutRot, timer / duration);
			_thisCamera.fieldOfView = Mathf.Lerp (_cameraStartFOV, _cameraZoomedOutFOV, timer / duration);
			yield return null;
		}
		_thisCameraHeighttContainer.position = _cameraZoomedOutViewPos;
		_thisCamera.transform.rotation = _cameraZoomedOutRot;
		_thisCamera.fieldOfView = _cameraZoomedOutFOV;
		yield return null;
		_isScrolling = false;
		_isZooming = false;
	}
}
