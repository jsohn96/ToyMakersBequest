using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxManager : MonoBehaviour {
	bool isBoxOpen = false;
	Animator _myAnim;

	// Use this for initialization
	void Awake () {
		_myAnim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			isBoxOpen = !isBoxOpen;
			
		}

		if (isBoxOpen) {
			_myAnim.Play("Open");
		}
		
	}
}
