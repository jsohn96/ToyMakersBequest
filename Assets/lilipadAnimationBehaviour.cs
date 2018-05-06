using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lilipadAnimationBehaviour : MonoBehaviour {
	[SerializeField] Transform _lilipad;
	float _rotateSpeed = 4f;
	bool _isFlipback = false;
	Quaternion _originAngle;
	Quaternion _finalAngle;

	// Use this for initialization
	void Awake () {
		//_lilipad = gameObject.transform;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_isFlipback && Mathf.Abs(Quaternion.Angle (_lilipad.localRotation, _finalAngle)) > 0.002f) {
			_lilipad.localRotation = Quaternion.Lerp (_lilipad.localRotation, _finalAngle, Time.deltaTime * _rotateSpeed);
			
		} else {
			_lilipad.localRotation = _finalAngle;
			_isFlipback = false;
		}
		
	}

	public void RotateOnce(){
	
	}

	public void Flipback(){
		if (!_isFlipback) {
			_isFlipback = true;
			_originAngle = _lilipad.localRotation;
			_finalAngle = _originAngle * Quaternion.Euler (180, 0, 0);
		}
	}
}
