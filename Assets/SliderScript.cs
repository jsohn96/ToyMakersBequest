using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider))]
public class SliderScript : MonoBehaviour {

	[SerializeField] Transform _knob;
	Vector3 _targetPos;

	float _sliderLength;
	[SerializeField] float _sliderPercent;

	public bool _discreteToggleOn = false;
	[SerializeField] MinMax _toggleBuffer = new MinMax(0.1f, 0.9f);

	void Start () {
		_sliderLength = GetComponent<BoxCollider> ().size.x;
		_targetPos = _knob.localPosition;
	}
	
	void SliderMovement (bool noMoreDrag) {
			_sliderPercent = Mathf.Clamp01 ((_targetPos.x + _sliderLength / 2f) / _sliderLength);
		if (_sliderPercent >= _toggleBuffer.Max) {
			_discreteToggleOn = true;
			_targetPos.x = 0.5f - _knob.localScale.x / 2f;
			_knob.localPosition = _targetPos; 
		} else if (_sliderPercent <= _toggleBuffer.Min) {
			_discreteToggleOn = false;
			_targetPos.x = -0.5f + _knob.localScale.x / 2f;
			_knob.localPosition = _targetPos;
		} else if (!noMoreDrag) {
			_knob.localPosition = Vector3.Lerp (_knob.localPosition, _targetPos, Time.deltaTime * 10f);
		} else {
			_knob.localPosition = _targetPos;
		}
	}

	void OnTouchStay(Vector3 point){
		_targetPos = new Vector3 (transform.InverseTransformPoint(point).x, _targetPos.y, _targetPos.z);
		SliderMovement (false);
	}

	void OnTouchUp(Vector3 point){
		SliderMovement (true);
	}

	public float GetSliderPercent(){
		return _sliderPercent;
	}
}
