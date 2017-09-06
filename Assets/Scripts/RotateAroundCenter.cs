using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundCenter : MonoBehaviour {
	[SerializeField] Transform _centerPoint;
	MinMax _speedMinMax = new MinMax(10.0f, 100.0f);
	float _counter = 0.0f;
	float _counterDuration = 3.0f;

	bool _isClockCompleted = false;
	float _maxSpeed = 300.0f;
	
	void Update () {
		if ((_isClockCompleted && _counter > _counterDuration) || Input.GetKey(KeyCode.Space)) {
			transform.RotateAround (_centerPoint.position, _centerPoint.up, _maxSpeed * Time.deltaTime);
		} else {
			if (Input.GetKey (KeyCode.R)) {
				_counter += Time.deltaTime;
				transform.RotateAround (_centerPoint.position, _centerPoint.up, (MathHelpers.LinMapFrom01 (_speedMinMax.Min, _speedMinMax.Max, _counter / _counterDuration)) * Time.deltaTime);
			} else {
				if (_counter > 0.0f) {
					_counter -= 0.1f;
					transform.RotateAround (_centerPoint.position, _centerPoint.up, (MathHelpers.LinMapFrom01 (_speedMinMax.Min, _speedMinMax.Max, _counter / _counterDuration)) * Time.deltaTime);
				} else {
					_counter = 0.0f;
				}
			}
		}
	}

	void ClockCompleted(ClockCompletionEvent e){
		_maxSpeed = e.MaxSpeed;
		_isClockCompleted = e.IsClockCompleted;

		if (_isClockCompleted) {
			_counterDuration = 3.0f;
		}
	}

	void OnEnable(){
		Events.G.AddListener<ClockCompletionEvent> (ClockCompleted);
	}
	void OnDisable(){
		Events.G.RemoveListener<ClockCompletionEvent> (ClockCompleted);
	}

}
