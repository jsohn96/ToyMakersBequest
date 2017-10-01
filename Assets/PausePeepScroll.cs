using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePeepScroll : MonoBehaviour {

	[SerializeField] PeepholeWalk _peepHoleWalkScript;
	Timer _pauseTimer;
	bool _isTriggered = false;
	// Use this for initialization
	void Start () {
		_pauseTimer = new Timer (3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (_isTriggered) {
			if (_pauseTimer.IsOffCooldown) {
				_peepHoleWalkScript.enabled = true;
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "MainCamera") {
			if (!_isTriggered) {
				_pauseTimer.Reset ();
				_peepHoleWalkScript.enabled = false;
				_isTriggered = true;
			}
		}
	}
}
