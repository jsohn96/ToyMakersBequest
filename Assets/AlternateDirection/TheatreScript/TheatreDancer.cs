using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreDancer : MonoBehaviour {
	Vector3 finalPosition;
	AltTheatre _myTheatre;

	// Use this for initialization
	void Start () {
		finalPosition = gameObject.transform.localPosition;
		Vector3 tempPos = finalPosition;
		tempPos.y -= 100f;
		gameObject.transform.localPosition = tempPos;

		_myTheatre = FindObjectOfType<AltTheatre> ().GetComponent<AltTheatre> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DancerEnterScene(){
		Debug.Log ("Dancer enter the scene");
		gameObject.transform.localPosition = finalPosition;
		_myTheatre.MoveToNext ();
	}
}
