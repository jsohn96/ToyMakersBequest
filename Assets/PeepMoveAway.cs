using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepMoveAway : MonoBehaviour {
	[SerializeField] Vector3 _goalPos;
	Timer _moveTimer;
	bool _isMoved = false;

	void Awake(){
		_moveTimer = new Timer (5.0f);
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "MainCamera") {
			if (!_isMoved) {
				_moveTimer.Reset ();
				_isMoved = true;
			}
		}
	}

	void Update(){
		if (_isMoved) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, _goalPos, _moveTimer.PercentTimePassed);
		}
	}
}
