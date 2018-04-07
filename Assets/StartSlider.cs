using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSlider : MonoBehaviour {
	[SerializeField] Animator _sliderAnim;
	[SerializeField] Transform _sliderPoint;
	int _hashID;

	bool _sliderStarted = false;
	float _goalNormalizedValue = 0f;
	float _currentNormalizedValue = 0f;
	float _originNormalizedValue = 0f;

	float timer = 0f;
	float duration = 6f;
	bool _isLerping = false;

	Vector3 _pointRotationAxis;
	[SerializeField] float _xAxisRight;
	[SerializeField] float _xAxisLeft;

	bool _touchDown = false;
	[SerializeField] Camera _mainCamera;
	[SerializeField] BoxCollider _draggableCollider;
	[SerializeField] TheatreCameraControl _theatreCameraControl;

	[SerializeField] AltTheatre _myTheatre;

//	float _previousDragNorm ;
//	float _currentDragNorm;
//
	void Start () {
		_hashID = Animator.StringToHash ("Slide");
		_sliderAnim.Play (_hashID, -1, 1f);
		_pointRotationAxis = _sliderPoint.forward;
	}

	void FixedUpdate(){
		if (_sliderStarted) {
			_sliderPoint.Rotate (_pointRotationAxis, -1f);

			if (timer < duration) {
				timer += Time.deltaTime / duration;

				_currentNormalizedValue = Mathf.Lerp (_originNormalizedValue, _goalNormalizedValue, timer);
				_sliderAnim.Play (_hashID, -1, _currentNormalizedValue);
			} else if(_isLerping) {
				_sliderAnim.Play (_hashID, -1, _goalNormalizedValue);
				_isLerping = false;
			}
		}
	}

	void Update(){
		if (_touchDown) {
			float mouseX = _mainCamera.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)).x;
			float normalizedLinMap = MathHelpers.LinMapTo01 (_xAxisLeft, _xAxisRight, mouseX);
			_sliderAnim.Play (_hashID, -1, normalizedLinMap);
			if (normalizedLinMap < 0.05f) {
				_sliderStarted = true;
				_draggableCollider.enabled = false;
				_touchDown = false;
				TriggerStart ();
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			_touchDown = false;
		}
	}
	
	public void SetSliderState (float thisValue, float outOfThisTotal) {
		if (_sliderStarted) {
			_isLerping = true;
			timer = 0f;
			_originNormalizedValue = _sliderAnim.GetCurrentAnimatorStateInfo (0).normalizedTime;
			_goalNormalizedValue = thisValue / outOfThisTotal;
		}
	}

	void OnTouchDown(Vector3 hitPoint){
		_touchDown = true;
//		StartCoroutine (ResetSlider ());
	}

	void TriggerStart(){
		_theatreCameraControl.MoveCameraToStartPosition ();
		_myTheatre.CheckStateMachine ();
	}

	IEnumerator ResetSlider(){
		float resetTimer = 0f;
		float resetDuration = 4f;
		while(resetTimer < resetDuration){
			resetTimer += Time.deltaTime;
			_sliderAnim.Play (_hashID, -1, 1f - resetTimer/resetDuration);
			_sliderPoint.Rotate (_pointRotationAxis, 5f);
			yield return null;
		}
		_sliderAnim.Play (_hashID, -1, 0f);
		yield return new WaitForSeconds (0.5f);

		_sliderStarted = true;
		yield return null;
	}
}
