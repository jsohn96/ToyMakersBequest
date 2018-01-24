using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredSoundEffect : MonoBehaviour {
	[SerializeField] bool _keepTriggersOn = false;
	AudioSource _audioSource;
	[SerializeField] string _objectNameToCollide = "Dancer";
	[SerializeField] Transform _bell;
	Quaternion _leftAngle;
	Quaternion _rightAngle;
	Quaternion _originAngle;

	Timer _bellTimer;
	Vector3 _tempVector3;
	BoxCollider _boxCollider;

	[SerializeField] SliderScript _sliderScript;
	[SerializeField] AudioClip _correctClip, _wrongClip;

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
		_boxCollider = GetComponent<BoxCollider> ();
		if (!_keepTriggersOn) {
			_boxCollider.enabled = false;
		}
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
			if (_sliderScript != null) {
				if (_sliderScript != null && _sliderScript._discreteToggleOn) {
					_audioSource.clip = _correctClip;
				} else {
					_audioSource.clip = _wrongClip;
				}
			
			} else {
				
				_audioSource.clip = _correctClip;
			
			}			
			_audioSource.Play ();
			_bellTimer.Reset ();
		}
	}

	void OnEnable ()
	{
		Events.G.AddListener<DancerOnBoard> (DancerOnBoardHandle);
	}

	void OnDisable ()
	{
		Events.G.RemoveListener<DancerOnBoard> (DancerOnBoardHandle);
	}

	void DancerOnBoardHandle(DancerOnBoard e){
		int nodeIndex = e.NodeIdx;
		if (nodeIndex == 234) {
			_boxCollider.enabled = true;
		}
	}
}
