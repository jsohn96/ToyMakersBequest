using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreFrog : MonoBehaviour {
	[SerializeField] AltTheatre _myTheatre;
	[SerializeField] Animator _frogAnim;
	Vector3 _finalPosition;

	//Vector3 _endPosition;
	bool _isMoving;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FrogJump(){
		_myTheatre.MoveToNext ();
		
	}
}
