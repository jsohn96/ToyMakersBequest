using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreWaterTankDoors : MonoBehaviour {
	Quaternion _openRot;
	[SerializeField] Quaternion _closeRot;

	[SerializeField] AltTheatre _myTheatre;

//	MeshCollider _meshCollider;
	IEnumerator _tankDoorCoroutine;

	bool _isOpen = true;
	bool _isActivated = false;
	[SerializeField] shaderGlowCustom _shaderGlowCustom;

	bool _finalWaterTankClose = false;
	bool _openBoth = false;
	bool _callOnce = false;

	void Start(){
		_openRot = transform.localRotation;
//		_meshCollider = GetComponent<MeshCollider> ();
		//		if (!_isActivated) {
//			_boxCollider.enabled = false;
//		}
	}
		


	void OnTouchDown(){
		if (!_finalWaterTankClose) {
			if (_openBoth) {
				_myTheatre.MoveToNext ();
			} else {
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

		//TODO: Need to prevent player from repeatedly calling this by only clicknig on one side over again
		if (_isActivated && !_callOnce) {
			_callOnce = true;
			_myTheatre.MoveToNext ();
		}
		if (_finalWaterTankClose) {
			_finalWaterTankClose = false;
			_openBoth = true;
		}
	}

	IEnumerator OpenTank(){
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


	public void OpenTankCall(){
		_isOpen = true;
		_tankDoorCoroutine = OpenTank ();
		StartCoroutine (_tankDoorCoroutine);
	}


	public void Activate(bool activate){
		_isActivated = activate;
		if (activate) {
			if (!_isOpen) {
				_isOpen = true;
				_tankDoorCoroutine = OpenTank ();
				StartCoroutine (_tankDoorCoroutine);
			}
			_shaderGlowCustom.TriggerFadeIn ();
		} 
	}

	public void FinalActivation(bool finalActivate){
		if(finalActivate) {
			_finalWaterTankClose = true;
			if (_isOpen) {
				_isOpen = false;
				_tankDoorCoroutine = CloseTank ();
				StartCoroutine (_tankDoorCoroutine);
			}

			_shaderGlowCustom.TriggerFadeIn ();
		}
	}
}
