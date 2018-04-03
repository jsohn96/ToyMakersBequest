using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreWaterTank : MonoBehaviour {

	[SerializeField] Animator WaterTankAnim;
	BoxCollider _boxCollider;
	[SerializeField] AltTheatre _myTheatre;

	[SerializeField] shaderGlowCustom _shaderGlowCustom;

	bool isActivated = false;
	bool _isOpen = false;

	// Use this for initialization
	void Start () {
		_boxCollider = GetComponent<BoxCollider> ();
		_boxCollider.enabled = false;
	}

	void OnTouchDown(){
		// 
		if (isActivated) {
			if (!_isOpen) {
				WaterTankAnim.SetBool ("Open", true);
				_isOpen = true;
				if (AltTheatre.currentSate == TheatreState.readyForDancerTank || AltTheatre.currentSate == TheatreState.readyForDancerTank2) {
					_myTheatre.MoveToNext ();
				}
			} else {
				WaterTankAnim.SetBool ("Open", false);
				_isOpen = false;
			}
		} 
		//frogScript.FrogJump ();
	}

	public void OpenLid(bool open){
		if (open) {
			if (!_isOpen) {
				//PlaySound HERE
				WaterTankAnim.SetBool ("Open", true);
				_isOpen = true;
			}
		} else {
			if (_isOpen) {
				//PLAY SOUND HERR
				WaterTankAnim.SetBool ("Open", false);
				_isOpen = false;
			}
		}
	}

	public void DisableLid(bool disable){
		isActivated = !disable;
	}

	public void Activate(bool activate){
		isActivated = activate;
		_boxCollider.enabled = activate;
		if (activate) {
			_shaderGlowCustom.TriggerFadeIn ();
		}
	}
}
