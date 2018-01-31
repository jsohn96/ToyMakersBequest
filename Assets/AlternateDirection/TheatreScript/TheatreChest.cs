using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreChest : MonoBehaviour {
	[SerializeField] TheatreFrog frogScript;
	[SerializeField] Animator chestAnim;

	bool isActivated = false;

	// Use this for initialization
	void Start () {
		
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
			frogScript.FrogJump ();
		}
	}

	public void Activate(){
		if (!isActivated) {
			isActivated = true;
		}
	}
}
