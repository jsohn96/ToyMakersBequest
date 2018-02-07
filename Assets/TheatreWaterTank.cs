using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreWaterTank : MonoBehaviour {

	[SerializeField] Animator WaterTankAnim;
	BoxCollider _boxCollider;
	[SerializeField] AltTheatre _myTheatre;

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
				Debug.Log ("called");
				WaterTankAnim.SetBool ("Open", true);
				_isOpen = true;
				if (AltTheatre.currentSate == TheatreState.readyForDancerTank) {
					_myTheatre.MoveToNext ();
				}
			} else {
				WaterTankAnim.SetBool ("Open", false);
				_isOpen = false;
			}
		} 
		//frogScript.FrogJump ();
	}

	public void Activate(bool activate){
		isActivated = activate;
		_boxCollider.enabled = activate;

	}
}
