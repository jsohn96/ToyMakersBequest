using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreWaterTankDoors : MonoBehaviour {
	Quaternion _openRot;
	[SerializeField] Quaternion _closeRot;

	[SerializeField] AltTheatre _myTheatre;

	BoxCollider _boxCollider;
	IEnumerator _tankDoorCoroutine;

	bool _isOpen = true;
	bool _isActivated = false;
	[SerializeField] shaderGlowCustom _shaderGlowCustom;

	void Start(){
		_openRot = transform.localRotation;
		_boxCollider = GetComponent<BoxCollider> ();
		//		if (!_isActivated) {
//			_boxCollider.enabled = false;
//		}
	}
		


	void OnTouchDown(){
		if (_isOpen) {
			_isOpen = false;
			if (_tankDoorCoroutine != null) {
				StopCoroutine (_tankDoorCoroutine);
			}
			_tankDoorCoroutine = CloseTank ();
			StartCoroutine (_tankDoorCoroutine);
		} else {
			if (!_isActivated) {
				_isOpen = true;
				if (_tankDoorCoroutine != null) {
					StopCoroutine (_tankDoorCoroutine);
				}
				_tankDoorCoroutine = OpenTank ();
				StartCoroutine (_tankDoorCoroutine);
			}
		}
	}

	IEnumerator CloseTank(){
		float timer = 0f;
		float duration = 1.5f;
		Quaternion _currentRot = transform.localRotation;
		while (timer < duration) {
			timer += Time.deltaTime;
			transform.localRotation = Quaternion.Slerp (_currentRot, _closeRot, timer / duration);
			yield return null;
		}
		transform.localRotation = _closeRot;
		yield return null;

		if (_isActivated) {
			_myTheatre.MoveToNext ();
		}
	}

	IEnumerator OpenTank(){
		if (!_isActivated) {
			float timer = 0f;
			float duration = 1.5f;
			Quaternion _currentRot = transform.localRotation;
			while (timer < duration) {
				timer += Time.deltaTime;
				transform.localRotation = Quaternion.Slerp (_currentRot, _openRot, timer / duration);
				yield return null;
			}
			transform.localRotation = _openRot;
			yield return null;
		}
	}



	public void Activate(bool activate){
		_isActivated = activate;
		if (activate) {
			_shaderGlowCustom.TriggerFadeIn ();
		}
	}
}
