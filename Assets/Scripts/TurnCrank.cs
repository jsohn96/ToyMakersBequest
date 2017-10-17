using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCrank : MonoBehaviour {
	[SerializeField] Camera _mainCamera;
	float _crankTurnSensitivity = 1000.0f;
	int _traversalExclusionLayerMask = 1 << 8;

	[SerializeField] Transform[] _otherGears;
	[SerializeField] AudioClip[] _crankClips;
	AudioSource _audioSource;

	[SerializeField] bool _isZoetrope = false;
	[SerializeField] bool _isReverse = false;
	int _crankCnt = 0;

	bool _startRotate = false;
	float _speed = 0.001f;
	Timer _speedTimer;
	[SerializeField] AnimationCurve _speedCurve;
	MinMax _speedRange = new MinMax(0.1f, 600.0f);

	[SerializeField] ZoetropeLightFlicker _dLight;
	[SerializeField] CameraZoom _cameraZoomScript;

	[SerializeField] AudioSource _audioSourceWhir;

	// Use this for initialization
	void Awake () {
		_traversalExclusionLayerMask = ~_traversalExclusionLayerMask;
		_audioSource = GetComponent<AudioSource> ();
		_speedTimer = new Timer (5.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!_isZoetrope || !_startRotate) {
			if (!_isZoetrope && Input.GetAxis ("Mouse ScrollWheel") > 0f) {
				PlayCrankSound ();

				transform.Rotate (Vector3.right * Time.deltaTime * _crankTurnSensitivity);

				for (int i = 0; i < _otherGears.Length; i++) {
					if (!_isReverse) {
						_otherGears [i].Rotate (Vector3.down * Time.deltaTime * 300.0f);
					} else {
						_otherGears [i].Rotate (Vector3.up * Time.deltaTime * 300.0f);
					}
				}
			} else if (Input.GetAxis ("Mouse ScrollWheel") < 0f) {
				_crankCnt++;

				PlayCrankSound ();
				transform.Rotate (Vector3.left * Time.deltaTime * _crankTurnSensitivity);

				for (int i = 0; i < _otherGears.Length; i++) {
					if (!_isReverse) {
						_otherGears [i].Rotate (Vector3.up * Time.deltaTime * 300.0f);
					} else {
						_otherGears [i].Rotate (Vector3.down * Time.deltaTime * 300.0f);
					}
				}
			}
			if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.DownArrow)) {
				PlayCrankSound ();
				_crankCnt++;

				transform.Rotate (Vector3.left * Time.deltaTime * _crankTurnSensitivity);

				for (int i = 0; i < _otherGears.Length; i++) {
					if (!_isReverse) {
						_otherGears [i].Rotate (Vector3.up * Time.deltaTime * 300.0f);
					} else {
						_otherGears [i].Rotate (Vector3.down * Time.deltaTime * 300.0f);
					}
				}
			}
		}


		if (_isZoetrope) {
			if (_crankCnt > 122) {
				if (!_startRotate) {
					_startRotate = true;
					_speedTimer.Reset ();
					_dLight.PlayTick ();
					StartCoroutine (DelayShutDown ());
				}
			} else if (_crankCnt > 90) {
				_dLight.DarkerFlicker ();
			}
			else if (_crankCnt > 45) {
				_dLight.LittleFlicker ();
			}

			if (_startRotate) {

				_audioSource.Stop ();

				_speed = MathHelpers.LinMapFrom01(_speedRange.Min, _speedRange.Max, _speedCurve.Evaluate (_speedTimer.PercentTimePassed));

				transform.Rotate (Vector3.right * Time.deltaTime * _speed);
				for (int i = 0; i < _otherGears.Length; i++) {
					_otherGears [i].Rotate (Vector3.up * Time.deltaTime * _speed);
				}


			}
		}
	}	

	void PlayCrankSound(){
		if (!_audioSource.isPlaying) {
			int index = Random.Range (0, 4);
			_audioSource.clip = _crankClips [index];
			_audioSource.Play ();
		}
	}

	IEnumerator DelayShutDown(){
		_audioSourceWhir.Play ();
		yield return new WaitForSeconds (5.0f);
		_cameraZoomScript.BeginZoom ();
		yield return new WaitForSeconds (1.0f);
		_dLight.ShutDown ();
	}
}
