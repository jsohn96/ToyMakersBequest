using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepIn : MonoBehaviour {
	[SerializeField] Camera _mainCamera;
	int _traversalExclusionLayerMask = 1 << 8;
	[SerializeField] ObjectRotator _traversalScript;
	Transform _traversalTransform;

	Vector3 _cameraOriginPosition = new Vector3(0.0f, 1.82f, 0.0f);
	Vector3 _cameraOriginRotation = new Vector3 (0.0f, 20.798f, 0.0f);
	Vector3 _cameraPositionCache;
	Vector3 _cameraRotationCache;
	Vector3 _cameraPeepPosition = new Vector3(1.8f, 2.6f, 4.6f);
	Vector3 _cameraPeepRotation = new Vector3 (0.0f, 20.798f, 0.0f);

	Vector3 _cameraPeepInPosition = new Vector3(2.49f, 2.59f, 6.42f);

	bool _isPeeping = false;
	public bool _isPeepingIn = false;
	bool _isTransitioning = false;
	bool _isPeepTransitioning = false;
	Timer _peepAtTimer;
	Timer _peepInTimer;

	Vector3 _tempRotation;
	Vector3 _tempPosition;

	[SerializeField] TurnCrank _turnCrankScript;
	[SerializeField] Fading _fadeScript;

	// Use this for initialization
	void Awake () {
		_traversalTransform = _traversalScript.gameObject.transform;
		_traversalExclusionLayerMask = ~_traversalExclusionLayerMask;

		_peepAtTimer = new Timer (1.0f);
		_peepInTimer = new Timer (1.5f);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = _mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, _traversalExclusionLayerMask)) {
				if (hit.collider.name == "Peep") {
					_isPeeping = true;
					_traversalScript.enabled = false;
					_isTransitioning = true;
					Debug.Log ("works");
					_cameraPositionCache = _traversalTransform.localPosition;
					_cameraRotationCache = _traversalTransform.localRotation.eulerAngles;

					_peepAtTimer.Reset ();
				} else {
					if (_isPeeping) {
						_isPeeping = false;
						_isTransitioning = true;
						_cameraPositionCache = _traversalTransform.localPosition;
						_cameraRotationCache = _traversalTransform.localRotation.eulerAngles;

						_peepAtTimer.Reset ();
					}
				}
			} 
		}

		if (_isPeeping) {
			if (Input.GetKeyDown (KeyCode.E) && !_isPeepTransitioning) {
				_isPeepingIn = !_isPeepingIn;

				if (_isPeepingIn) {
					StartCoroutine (FadeInTransition ());
				} else {
					StartCoroutine (FadeOutTransition ());
				}

			}
		}

		if (_isPeepTransitioning) {
			if (_peepInTimer.IsOffCooldown) {
				_isPeepTransitioning = false;
			} else if (_isPeepingIn) {
				_turnCrankScript._isHoldingCrank = true;
				_tempPosition = Vector3.Lerp (_cameraPeepPosition, _cameraPeepInPosition, _peepInTimer.PercentTimePassed);
				_traversalTransform.localPosition = _tempPosition;
			} else {
				_turnCrankScript._isHoldingCrank = false;
				_tempPosition = Vector3.Lerp (_cameraPeepInPosition, _cameraPeepPosition, _peepInTimer.PercentTimePassed);
				_traversalTransform.localPosition = _tempPosition;
			}
		} else {
			if (_isTransitioning) {
				if (_peepAtTimer.IsOffCooldown) {
					if (!_isPeeping) {
						_traversalScript.enabled = true;
					}
					_isTransitioning = false;
				} else if (_isPeeping) {
					_tempPosition = Vector3.Lerp (_cameraPositionCache, _cameraPeepPosition, _peepAtTimer.PercentTimePassed);
					_tempRotation = Vector3.Lerp (_cameraRotationCache, _cameraPeepRotation, _peepAtTimer.PercentTimePassed);
					_traversalTransform.SetPositionAndRotation (_tempPosition, Quaternion.Euler (_tempRotation));

				} else {
					_tempPosition = Vector3.Lerp (_cameraPositionCache, _cameraOriginPosition, _peepAtTimer.PercentTimePassed);
					_tempRotation = Vector3.Lerp (_cameraRotationCache, _cameraOriginRotation, _peepAtTimer.PercentTimePassed);
					_traversalTransform.SetPositionAndRotation (_tempPosition, Quaternion.Euler (_tempRotation));
				}
			}
		}
	}

	IEnumerator FadeInTransition(){
		_isPeepTransitioning = true;
		_peepInTimer.Reset ();
		_fadeScript.BeginFade (1);
		yield return new WaitForSeconds (1.5f);
		Events.G.Raise (new InsidePeepHoleEvent (true));
		_fadeScript.BeginFade (-1);
	}

	IEnumerator FadeOutTransition(){
		_fadeScript.BeginFade (1);
		yield return new WaitForSeconds (1f);
		Events.G.Raise (new InsidePeepHoleEvent (false));
		yield return new WaitForSeconds (0.25f);
		_isPeepTransitioning = true;
		_peepInTimer.Reset ();
		_fadeScript.BeginFade (-1);
	}
}
