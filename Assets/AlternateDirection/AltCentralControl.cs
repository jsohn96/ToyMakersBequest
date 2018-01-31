using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AltStates{
	noCharm = 0,
	oneCharm = 1,
	twoCharm = 2,
	allCharm = 3,
	keyUnlock = 4
}

public class AltCentralControl : MonoBehaviour {

	public static AltCentralControl _instance;
	public static bool _love = false, _regret = false, _freedom = false, _loveEnd = false, _regretEnd = false, _freedomEnd = false;

	public static bool[] _peepholeViewed = new bool[] {false, false, false, false};
	public static bool[] _peepAnimated = new bool[]{false, false, false, false};
	public static AltStates _currentState = AltStates.noCharm;

	void Awake () {
		//assign an instance of this gameobject if it hasn't been assigned before
		if (_instance == null) {
			_instance = this;
		} else if (_instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);
	}


	void Update(){
		if (Input.GetKeyDown (KeyCode.Q)) {
			_currentState = AltStates.oneCharm;
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			_currentState = AltStates.twoCharm;
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			_currentState = AltStates.allCharm;
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			_currentState = AltStates.keyUnlock;
		}
	}
}
