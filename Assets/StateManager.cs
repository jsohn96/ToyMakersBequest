using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
	Zoetrope = 0,
	ShadowPuppet = 1,
	Marionette = 2,
	MusicBox = 3,
	PeepHole = 4,
	End = 5,
	Null = 6
}

public class StateManager : MonoBehaviour {
	public static StateManager _stateManager;

	public State currentState {
		get { return _currentState; }
		set { _currentState = value; }
	}
	State _currentState = State.Zoetrope;

	void Start () {
		if (_stateManager == null) {
			DontDestroyOnLoad (gameObject);
			_stateManager = this;
		}
		else if (_stateManager != this) {
			Destroy (gameObject);
		}
	}
}
