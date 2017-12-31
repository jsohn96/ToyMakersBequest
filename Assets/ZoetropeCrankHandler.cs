using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoetropeCrankHandler : MonoBehaviour {
	AudioSource _audioSource;

	int _crankCnt = 0;

	bool _startRotate = false;
	float _speed = 0.001f;
	Timer _speedTimer;
	[SerializeField] AnimationCurve _speedCurve;
	MinMax _speedRange = new MinMax(0.1f, 600.0f);
	[SerializeField] Transform[] _otherGears;

	[SerializeField] ZoetropeLightFlicker _dLight;
	[SerializeField] CameraZoom _cameraZoomScript;

	[SerializeField] AudioSource _audioSourceWhir;
	[SerializeField] DragRotation _dragRotationScript;


	// Use this for initialization
	void Awake () {
		_audioSource = GetComponent<AudioSource> ();
		_speedTimer = new Timer (5.0f);
	}

	
	// Update is called once per frame
	void Update () {
		if (_crankCnt > 240) {
			if (!_startRotate) {
				_dragRotationScript.enabled = false;
				_startRotate = true;
				_speedTimer.Reset ();
				_dLight.PlayTick ();
				StartCoroutine (DelayShutDown ());
			}
		} else if (_crankCnt > 180) {
			_dLight.DarkerFlicker ();
		} else if (_crankCnt > 140) {
			_dLight.LittleFlicker ();
		}

		if (_startRotate) {

			_speed = MathHelpers.LinMapFrom01(_speedRange.Min, _speedRange.Max, _speedCurve.Evaluate (_speedTimer.PercentTimePassed));

			transform.Rotate (Vector3.back * Time.deltaTime * _speed);
			for (int i = 0; i < _otherGears.Length; i++) {
				_otherGears [i].Rotate (Vector3.forward * Time.deltaTime * _speed);
			}


		}
	}


	IEnumerator DelayShutDown(){
		_audioSourceWhir.Play ();
		yield return new WaitForSeconds (5.0f);
		_cameraZoomScript.BeginZoom ();
		yield return new WaitForSeconds (1.0f);
		_dLight.ShutDown ();
	}

	void OnEnable(){
		Events.G.AddListener<DragRotationEvent> (DragRotationHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<DragRotationEvent> (DragRotationHandle);
	}

	void DragRotationHandle(DragRotationEvent e){
		if (e.isRoating) {
			if (e.isDesiredDirection) {
				_crankCnt++;
			} else {
				if (_crankCnt > 0) {
					_crankCnt--;
				}
			}

		}
	}
}
