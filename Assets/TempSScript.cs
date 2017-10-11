using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.S)) {
			gameObject.SetActive (false);
		}
	}

	public void DisplayText(){
		gameObject.SetActive (true);
	}
}
