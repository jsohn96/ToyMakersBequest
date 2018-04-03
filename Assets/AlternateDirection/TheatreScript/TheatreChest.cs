using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreChest : MonoBehaviour {
	[SerializeField] TheatreFrog frogScript;
	[SerializeField] Animator chestAnim;
	BoxCollider _boxCollider;

	bool isActivated = false;
	bool _isOpen = false;
	bool isFrogOut = false;

	bool _takeAwayControl = false;

	[SerializeField] shaderGlowCustom _shaderGlowCustom;
	[SerializeField] AltTheatre _myTheatre;

	[SerializeField] TheatreSound _theatreSound;

	// Use this for initialization
//	void Start () {
//		_boxCollider = GetComponent<BoxCollider> ();
//		if (AltTheatre.currentSate < TheatreState.magicianLeft) {
//			_boxCollider.enabled = false;
//			isActivated = false;
//		}
//	}

	void OnTouchDown(){
		// 
		if (!_takeAwayControl) {
			if (!_isOpen) {
				chestAnim.SetBool ("Open", true);
				_isOpen = true;
				if (isActivated) {
					Debug.Log ("Empty chest open");
					if (!isFrogOut) {
						frogScript.FrogJump ();
						isFrogOut = true;
					}
				} 
			} else {
				chestAnim.SetBool ("Open", false);
				_isOpen = false;
			}
		}
			//frogScript.FrogJump ();
	}

	public void Activate(bool activate){
		isActivated = activate;
		if (activate) {
			_shaderGlowCustom.TriggerFadeIn ();
			if (_isOpen) {
				chestAnim.SetBool ("Open", false);
				_isOpen = false;
			}
		}
	}

	public void FrogPrep(){
		_takeAwayControl = true;
		if (_isOpen) {
			chestAnim.Play ("frogClose");
			StartCoroutine (DelayBeforeFrogJumpOut (0.9f));
		} else {
			StartCoroutine (DelayBeforeFrogJumpOut (0.1f));
		}
		chestAnim.SetBool ("Open", true);
	}

	IEnumerator DelayBeforeFrogJumpOut(float duration){
		yield return new WaitForSeconds (duration);
		_myTheatre.CallFrog ();
	}

	public void ChestCloseEndSound(){
		_theatreSound.PlayChestCloseEndSound ();
	}

	public void ChestOpenSound(){
		_theatreSound.PlayChestOpenSound ();
	}
}
