using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider))]
public class SliderScript : MonoBehaviour {
	[SerializeField] bool _setKnobAsinitializer = false;
	[SerializeField] Transform _knob;
	Vector3 _targetPos;
	Vector3 _tempPos;

	float _sliderLength;
	[SerializeField] float _sliderPercent;

	public bool _discreteToggleOn = false;
	[SerializeField] MinMax _toggleBuffer = new MinMax(0.1f, 0.9f);
	BoxCollider _boxCollider;
	IEnumerator _sliderLerpCoroutine;


	void Start () {
		_boxCollider = GetComponent<BoxCollider> ();
		_sliderLength = _boxCollider.size.x;
		_targetPos = _knob.localPosition;
		if (_setKnobAsinitializer) {
			_boxCollider.enabled = false;
		}
		SliderMovement (true);
	}
	
	void SliderMovement (bool noMoreDrag) {
		_sliderPercent = Mathf.Clamp01 ((_targetPos.x + _sliderLength / 2f) / _sliderLength);
		if (_sliderPercent >= _toggleBuffer.Max) {
			_discreteToggleOn = true;
			_targetPos.x = 0.5f - _knob.localScale.x / 2f;
		} else if (_sliderPercent <= _toggleBuffer.Min) {
			_discreteToggleOn = false;
			_targetPos.x = -0.5f + _knob.localScale.x / 2f;
		}
		if (!noMoreDrag) {
			_knob.localPosition = Vector3.Lerp (_knob.localPosition, _targetPos, Time.deltaTime * 3f);
		} else {
			if (_sliderPercent >= _toggleBuffer.Max || _sliderPercent <= _toggleBuffer.Min) {
				if (_sliderLerpCoroutine != null) {
					StopCoroutine (_sliderLerpCoroutine);
				}
				_sliderLerpCoroutine = LerpIntoPlace ();
				StartCoroutine (_sliderLerpCoroutine);
			} else {
				_knob.localPosition = _targetPos;
				// calculate slider percent again after change
				_sliderPercent = Mathf.Clamp01 ((_targetPos.x + _sliderLength / 2f) / _sliderLength);
			}
		}
	}

	void OnTouchStay(Vector3 point){
		_targetPos = new Vector3 (transform.InverseTransformPoint (point).x, _targetPos.y, _targetPos.z);
		SliderMovement (false);
	}

	void Update(){
		if (Input.GetMouseButtonUp (0)) {
			SliderMovement (true);
			if (_setKnobAsinitializer) {
				_boxCollider.enabled = false;
			}
		}
	}

	IEnumerator LerpIntoPlace(){
		float timer = 0f;
		float duration = 0.3f;
		_tempPos = _knob.localPosition;
		while (duration > timer) {
			timer += Time.deltaTime;
			_knob.localPosition = Vector3.Lerp (_tempPos, _targetPos, timer/duration);
			yield return null;
		}
		_knob.localPosition = _targetPos;
		// calculate slider percent again after change
		_sliderPercent = Mathf.Clamp01 ((_targetPos.x + _sliderLength / 2f) / _sliderLength);
		yield return null;
	}

	public float GetSliderPercent(){
		return _sliderPercent;
	}

	public void KnobInitialization(){
		if (_setKnobAsinitializer) {
			_boxCollider.enabled = true;
		}
	}
}
