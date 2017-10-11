using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum State {
	Zoetrope = 2,
	ShadowPuppet = 99,
	Marionette = 98,
	MusicBox = 3,
	PeepHole = 4,
	End = 97,
	Null = 100
}

public class StateManager : MonoBehaviour {
	public static StateManager _stateManager;
	Fading _fadeScript;

	public State currentState {
		get { return _currentState; }
		set { _currentState = value; }
	}
	public State _currentState = State.Zoetrope;

	void Awake () {
		if (_stateManager == null) {
			DontDestroyOnLoad (gameObject);
			_stateManager = this;
		}
		else if (_stateManager != this) {
			Destroy (gameObject);
		}
	}

	void Update(){
		//Restart
		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}


	public IEnumerator ChangeLevel(int sceneIndex){
		yield return new WaitForSeconds(0.5f);
		float fadeTime = GameObject.Find ("Fade").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene (sceneIndex);
	}
}
