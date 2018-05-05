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
	bool _waitForClose = false;
	[SerializeField] shaderGlowCustom _shaderGlowCustom;

	bool _finalWaterTankClose = false;
	bool _openBoth = false;
	bool _callOnce = false;

	bool _disableTouchInput = true;
	[SerializeField] bool _isLeftDoor = false;

	bool _tappedIn = false;
	public bool _firstClose = false;
	public bool _secondClose = false;

	bool _hideOnce = false;

	[SerializeField] TheatreWaterTankDoors _otherWaterTankDoor;

	[SerializeField] SpriteFade _spriteFade;
	bool _finalActivation = false;

	void Start(){
		_openRot = transform.localRotation;
//		_meshCollider = GetComponent<MeshCollider> ();
		//		if (!_isActivated) {
//			_boxCollider.enabled = false;
//		}
	}
		
	void OnTouchDown(){
		
		if (!_disableTouchInput) {

			if (_firstClose) {
				_firstClose = false;
				_tappedIn = false;
				_otherWaterTankDoor._firstClose = false;
				_disableTouchInput = true;
				_spriteFade.TurnItOffForGood ();
				//close Tank
				if (_tankDoorCoroutine != null) {
					StopCoroutine (_tankDoorCoroutine);
				}
				_tankDoorCoroutine = CloseTank ();
				StartCoroutine (_tankDoorCoroutine);
				_myTheatre.MoveToNext ();

			} else if (_secondClose) {
				_disableTouchInput = true;
				_spriteFade.TurnItOffForGood ();
				//close Tank
				if (_tankDoorCoroutine != null) {
					StopCoroutine (_tankDoorCoroutine);
				}
				_tankDoorCoroutine = CloseTank ();
				StartCoroutine (_tankDoorCoroutine);
			}

			else {
				if (!_waitForClose) {
					if (_openBoth) {
						OpenTankCall ();
						if (_finalActivation) {
							_myTheatre.MoveToNext ();
						}
						_otherWaterTankDoor.OpenTankCall ();
						_waitForClose = true;
						_openBoth = false;

					} else if (_isOpen) {
						_isOpen = false;
						if (_tankDoorCoroutine != null) {
							StopCoroutine (_tankDoorCoroutine);
						}
						_tankDoorCoroutine = CloseTank ();
						StartCoroutine (_tankDoorCoroutine);
					} else {
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

//	void OnTouchDown(){
//		if (!_disableTouchInput) {
//			if (!_finalWaterTankClose) {
//				if (_openBoth) {
//					_myTheatre.MoveToNext ();
//				} else {
//					if (_isOpen) {
//						if (!_isActivated && !_waitForClose) {
//							_isOpen = false;
//							if (_tankDoorCoroutine != null) {
//								StopCoroutine (_tankDoorCoroutine);
//							}
//							_tankDoorCoroutine = CloseTank ();
//							StartCoroutine (_tankDoorCoroutine);
//						}
//					} else {
//						if (!_waitForClose) {
//							_isOpen = true;
//							if (_tankDoorCoroutine != null) {
//								StopCoroutine (_tankDoorCoroutine);
//							}
//							_tankDoorCoroutine = OpenTank ();
//							StartCoroutine (_tankDoorCoroutine);
//						}
//					}
//				}
//			}
//		}
//	}

	IEnumerator CloseTank(){
		TheatreSound._instance.PlayWaterTankSound (false, _isLeftDoor);
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
		if (_finalWaterTankClose) {
			_finalWaterTankClose = false;
			_openBoth = true;
//			_shaderGlowCustom.TriggerFadeIn ();
		}
		_waitForClose = false;
		if (_secondClose && !_firstClose && _tappedIn) {
			_tappedIn = false;
			_secondClose = false;
			_otherWaterTankDoor._secondClose = false;
			_myTheatre.MoveToNext ();
		}

		if (_isActivated && !_hideOnce) {
			_hideOnce = true;
			_myTheatre.HideDancer ();
		}
	}

	IEnumerator OpenTank(){
		TheatreSound._instance.PlayWaterTankSound (true, _isLeftDoor);
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
		if (_isActivated && !_callOnce) {
			_callOnce = true;
			_myTheatre.MoveToNext ();
		}

		_waitForClose = false;

		if (_openBoth) {
//			_shaderGlowCustom.enabled = false;
		}
	}


	public void OpenTankCall(){
		_isOpen = true;
		_openBoth = false;
		_waitForClose = true;
		_tankDoorCoroutine = OpenTank ();
		StartCoroutine (_tankDoorCoroutine);
	}

	public void DisableTouchInput(bool disable){
		_disableTouchInput = disable;
		if (!disable) {
//			_shaderGlowCustom.TriggerFadeIn ();
		}
	}


	public void Activate(bool activate){

		_disableTouchInput = false;
		_isActivated = activate;
		if (activate) {
			_spriteFade.CallFadeSpriteIn (0.5f);
			_openBoth = true;
			_firstClose = true;
			_secondClose = true;
			_tappedIn = true;

//			if (_isOpen) {
//				_isOpen = false;
//			if (_tankDoorCoroutine != null) {
//				StopCoroutine (_tankDoorCoroutine);
//			}
//				_waitForClose = true;
//				_tankDoorCoroutine = CloseTank ();
//				StartCoroutine (_tankDoorCoroutine);
//			}
		} 
	}

	public void FinalActivation(bool finalActivate){
		_callOnce = false;
		_disableTouchInput = false;
		_isActivated = finalActivate;
		_finalActivation = finalActivate;
		if (_isActivated) {
			_spriteFade.CallFadeSpriteIn (0.5f);
			_openBoth = true;
			_firstClose = true;
			_secondClose = true;
			_tappedIn = true;
		}
//		if(finalActivate) {
//			_finalWaterTankClose = true;
//			if (_isOpen) {
//				_isOpen = false;
//				if (_tankDoorCoroutine != null) {
//					StopCoroutine (_tankDoorCoroutine);
//				}
//				_tankDoorCoroutine = CloseTank ();
//				StartCoroutine (_tankDoorCoroutine);
//			}
//		}
	}
}
