﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreChest : MonoBehaviour {
	[SerializeField] TheatreFrog frogScript;
	[SerializeField] Animator chestAnim;
	BoxCollider _boxCollider;

	bool isActivated = false;
	bool _isOpen = false;
	bool isFrogOut = false;

	// Use this for initialization
	void Start () {
		_boxCollider = GetComponent<BoxCollider> ();
		if (AltTheatre.currentSate < TheatreState.magicianLeft) {
			_boxCollider.enabled = false;
			isActivated = false;
		}
	}

	void OnTouchDown(){
		// 
		if (isActivated) {
			Debug.Log ("Empty chest open");
			if (!isFrogOut) {
				frogScript.FrogJump ();
			} else {
			
			}
			if (!_isOpen) {
				chestAnim.SetBool ("Open", true);
				_isOpen = true;
			} else {
				chestAnim.SetBool ("Open", false);
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
