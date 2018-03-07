using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogSwirlTest : MonoBehaviour {
	[SerializeField] Transform _frogTransform;
	[SerializeField] Transform _centerPoint;
	float _counter = 0.0f;
	float _counterDuration = 20.0f;

	[SerializeField] MinMax _frogSwimRange = new MinMax(12.402f, 12.612f);
	[SerializeField] AnimationCurve _swimHeightCurve;

	Vector3 _tempPosition;

	float _maxSpeed = 60.0f;

	bool _activateSwirl = false;
	[SerializeField] Animator _frogAnimator;

	public void ActivateSwirl(){
		_frogAnimator.enabled = false;
		_activateSwirl = true;
	}

	void Update () {
		if (_activateSwirl) {
			_counter += Time.deltaTime;
			_frogTransform.RotateAround (_centerPoint.position, _centerPoint.up, _maxSpeed * Time.deltaTime);
			_tempPosition = _frogTransform.position;
			_tempPosition.y = Mathf.Lerp(_frogSwimRange.Max, _frogSwimRange.Min, _swimHeightCurve.Evaluate(_counter/_counterDuration));
			_frogTransform.position = _tempPosition;

			if(_counter/_counterDuration>=1f){
				_counter = 0f;
			}
		}
	}
}
