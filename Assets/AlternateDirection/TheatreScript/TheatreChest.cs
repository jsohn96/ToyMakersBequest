using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreChest : MonoBehaviour {
	[SerializeField] TheatreFrog frogScript;
	[SerializeField] Animator chestAnim;
	BoxCollider _boxCollider;

	bool isActivated = false;

	// Use this for initialization
	void Start () {
		_boxCollider = GetComponent<BoxCollider> ();
		if (AltTheatre.currentSate < TheatreState.magicianLeft) {
			_boxCollider.enabled = false;
			isActivated = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTouchDown(){
		// 
		if (!isActivated) {
			Debug.Log ("Empty chest open");
		} else {
			Debug.Log ("Chest open with frog");
			//frogScript.FrogJump ();
		}
	}

	public void Activate(bool activate){
		isActivated = activate;
		_boxCollider.enabled = activate;
	}
}
