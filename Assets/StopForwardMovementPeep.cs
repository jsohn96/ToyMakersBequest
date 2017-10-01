using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopForwardMovementPeep : MonoBehaviour {

	[SerializeField] PeepholeWalk _peepHoleWalkScript;

	void OnTriggerEnter(Collider other){
		if (other.tag == "MainCamera") {
			_peepHoleWalkScript.StopMoveForward (true);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "MainCamera") {
			_peepHoleWalkScript.StopMoveForward (false);
		}
	}
}
