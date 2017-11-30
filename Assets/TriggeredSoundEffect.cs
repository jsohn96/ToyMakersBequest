using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredSoundEffect : MonoBehaviour {
	AudioSource _audioSource;
	string _objectNameToCollide = "Dancer";
	[SerializeField] Transform _bell;
	Quaternion _leftAngle;
	Quaternion _rightAngle;
	Quaternion _originAngle;

	Timer _bellTimer;
	Vector3 _tempVector3;

	// Use this for initialization
	void Start () {
		_audioSource = GetComponent<AudioSource> ();

		_bellTimer = new Timer (0.7f);
		_originAngle = _bell.rotation;
		_tempVector3 = _bell.rotation.eulerAngles;
		_tempVector3.x = -12.0f;
		_leftAngle = Quaternion.Euler (_tempVector3);
		_tempVector3.x = 12.0f;
		_rightAngle = Quaternion.Euler (_tempVector3);
	}
	
	// Update is called once per frame
	void Update () {
		if (!_bellTimer.IsOffCooldown) {
			if (_bellTimer.PercentTimePassed > 0.8f) {
				_bell.rotation = Quaternion.Lerp (_bell.rotation, _originAngle, MathHelpers.LinMapTo01(0.8f, 1.0f, _bellTimer.PercentTimePassed));
			} else if (_bellTimer.PercentTimePassed > 0.2f) {
				_bell.rotation = Quaternion.Lerp (_bell.rotation, _rightAngle, MathHelpers.LinMapTo01(0.2f, 0.8f, _bellTimer.PercentTimePassed));
			} else {
				_bell.rotation = Quaternion.Lerp (_bell.rotation, _leftAngle, MathHelpers.LinMapTo01(0f, 0.2f, _bellTimer.PercentTimePassed));
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.name == _objectNameToCollide) {
			_audioSource.Play ();
			_bellTimer.Reset ();
		}
	}
}
