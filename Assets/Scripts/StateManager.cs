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

	public bool _isShadowPuppetCompleted = false, 
		_isMarionetteCompleted = false, 
		_isMusicBoxCompleted = false, 
		_isPeepHoleCompleted = false,
		_finalCompleted = false;

	public bool _justUnlocked = false;


	public bool _openSpecificPage = false;
	public int _notebookStartingPage = 0;

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


	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded (Scene scene, LoadSceneMode mode){
		//0 = unlock all, and 1~4 are corresponding locks
		if (scene.name == "notebookScene") {
			if (_isMusicBoxCompleted) {
				if (_finalCompleted) {
					Events.G.Raise (new LeatherUnlockEvent (0, _justUnlocked));
				} else {
					if (!_openSpecificPage) {
						_notebookStartingPage = 4;
					} 
					Events.G.Raise (new LeatherUnlockEvent (3, _justUnlocked));
				}
			} else {
				if (!_openSpecificPage) {
					_notebookStartingPage = 0;
				} 
				Events.G.Raise (new LeatherUnlockEvent (2, _justUnlocked));
			}

			_justUnlocked = false;
		}
	}
}
