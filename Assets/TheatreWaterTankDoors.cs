using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreWaterTankDoors : MonoBehaviour {
	Quaternion _openRot;
	[SerializeField] Quaternion _closeRot;

	[SerializeField] AltTheatre _myTheatre;

	BoxCollider _boxCollider;

	bool _isOpen = true;
	bool _isActivated = false;

	void Start(){
		_openRot = transform.localRotation;
		_boxCollider = GetComponent<BoxCollider> ();
		if (!_isActivated) {
			_boxCollider.enabled = false;
		}
	}

	void Update(){
		if (AltTheatre.currentSate == TheatreState.CloseTank1 || AltTheatre.currentSate == TheatreState.CloseTank2) {
			Activate (true);
		} else {
			Activate (false);
		}
	}


	void OnTouchDown(){
		if (_isOpen) {
			_isOpen = false;
			_boxCollider.enabled = false;
			StartCoroutine (CloseTank ());
		}
	}

	IEnumerator CloseTank(){
		float timer = 0f;
		float duration = 3f;
		while (timer < duration) {
			timer += Time.deltaTime;
			transform.localRotation = Quaternion.Slerp (_openRot, _closeRot, timer / duration);
			yield return null;
		}
		transform.localRotation = _closeRot;
		yield return null;
		_myTheatre.MoveToNext ();
	}

	public void Activate(bool activate){
		_isActivated = activate;
		_boxCollider.enabled = activate;
	}
}
