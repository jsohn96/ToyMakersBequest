using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreBackSlider : MonoBehaviour {
	bool _isDown = false;
	[SerializeField] Camera _mainCamera;
	Vector3 _tempSliderPosition;

	[SerializeField] MinMax _sliderRange;
	[SerializeField] TheatreBack _theatreBack;

	[SerializeField] SpriteFade _arrowSliderSpriteFade;

	float _zDifference;

	float _snapValueUpperBound;

	bool _isActivated = false;

	void Start(){
		float snapOffsetValue = (_sliderRange.Max - _sliderRange.Min) * 0.1f;
		_snapValueUpperBound = _sliderRange.Max - snapOffsetValue;
	}

	void OnTouchDown(Vector3 point){
		if (_isActivated) {
			_zDifference = transform.position.z - _mainCamera.transform.position.z;
			_isDown = true;
			_tempSliderPosition = transform.position;
		}
	}

	void Update(){
		if (_isActivated) {
			if (Input.GetMouseButtonUp (0)) {
				_isDown = false;
			}
			if (_isDown) {
				float mouseY = _mainCamera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, _zDifference)).y;

				if (mouseY >= _snapValueUpperBound) {
					mouseY = _sliderRange.Max;
					_isDown = false;
					_isActivated = false;
					_theatreBack.ResumeSequence ();
					_arrowSliderSpriteFade.TurnItOffForGood ();
				}
				_tempSliderPosition.y = mouseY;
				transform.position = _tempSliderPosition;
			}
		}
	}

	public void Activate(){
		_isActivated = true;
		_arrowSliderSpriteFade.CallFadeSpriteIn ();
	}
}
