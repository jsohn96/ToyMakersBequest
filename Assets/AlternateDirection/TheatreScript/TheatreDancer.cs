using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreDancer : MonoBehaviour {
	Vector3 finalPosition;
	AltTheatre _myTheatre;
	[SerializeField] Transform _dancerTransform;

	[SerializeField] Vector3 _startPosition;
	Vector3 _stillRotateAxis = new Vector3 (0f,1f,0f);
	[SerializeField] Transform _waterTankTransformForCenterAxis;

	// Use this for initialization
	void Start () {
		finalPosition = gameObject.transform.position;
		_myTheatre = FindObjectOfType<AltTheatre> ().GetComponent<AltTheatre> ();
		if (AltTheatre.currentSate == TheatreState.waitingToStart) {
			_dancerTransform.position = _startPosition;
		}
	}
	
	// Update is called once per frame
	void Update () {
		RotateInPlace ();
		RotateAroundCenter ();
	}

	public void DancerEnterScene(){
		Debug.Log ("Dancer enter the scene");
		gameObject.transform.position = finalPosition;
		_myTheatre.MoveToNext ();
	}


	void RotateInPlace(){
		_dancerTransform.Rotate (_stillRotateAxis * 50f * Time.deltaTime);
	}

	void RotateAroundCenter(){
		_dancerTransform.RotateAround (_waterTankTransformForCenterAxis.position, _stillRotateAxis, 0.5f);
	}
}
