using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lilipadAnimationBehaviour : MonoBehaviour {
	[SerializeField] Transform _lilipad;
	float _rotateSpeed = 4f;
	bool _isFlipback = false;
	Quaternion _originAngle;
	Quaternion _finalAngle;
	bool _isPathConnecting;
	BoxCollider _bCol;
	[SerializeField] PathNode _pn;
	Vector3 _upPos;
	Vector3 _downPos;
	bool _isUp = false;
	bool _isClickActive = false;

	[SerializeField] TheatreLighting _theatreLighting;
	[SerializeField] int _thisPadIndex;

	// Use this for initialization
	void Awake () {
		//_lilipad = gameObject.transform;
		_pn = GetComponent<PathNode>();
		_bCol = GetComponent<BoxCollider> ();
		_bCol.enabled = false;
		_downPos = _lilipad.localPosition;
		_upPos = _downPos;
		_upPos.z -= 0.9f;
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

	void OnTouchDown(){
		if (_isUp && _isClickActive) {
			StartCoroutine (ClickLilipad ());
		}

	}

	public void RotateOnce(){
	
	}

	public void Flipback(){
		if (!_isFlipback) {
			_isFlipback = true;
			_originAngle = _lilipad.localRotation;
			_finalAngle = _originAngle * Quaternion.Euler (180, 0, 0);
			ActivateClick ();
		}
	}

	IEnumerator ClickLilipad(){
		_theatreLighting.LilySpotLight (_thisPadIndex+1);
		float timer = 0f;
		float duration = 0.5f;
		while (duration > timer) {
			timer += Time.deltaTime;
			_lilipad.localPosition = Vector3.Lerp (_lilipad.localPosition, _downPos, timer/duration);
			yield return null;
		}
		_lilipad.localPosition = _downPos;
		_isUp = false;
		_pn.ActivateCheck ();
		yield return null;
	}

	public IEnumerator MoveLilipadUp(){
		float timer = 0f;
		float duration = 1f;
		while (duration > timer) {
			timer += Time.deltaTime;
			_lilipad.localPosition = Vector3.Lerp (_lilipad.localPosition, _upPos, timer/duration);
			yield return null;
		}
		_lilipad.localPosition = _upPos;
		_isUp = true;
		yield return null;
	}

	public void ActivateClick(){
		if (!_isClickActive) {
			_isClickActive = true;
			_bCol.enabled = true;
		}
	}
}
