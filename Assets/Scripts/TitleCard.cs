using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleCard : MonoBehaviour {
	string[] _titleCards = new string[4] {
		"Title Screen: Zoetrope",
		"Level 3: MusicBox",
		"Level 4: Peephole Theater",
		"The End"
	};

	[SerializeField] Text _titleText;

	void OnLevelLoad(Scene scene, LoadSceneMode mode){
		switch (StateManager._stateManager.currentState) {
		case State.Null:
			_titleText.text = _titleCards [0];
			break;
		case State.Zoetrope:
			_titleText.text = _titleCards [1];
			break;
		case State.MusicBox:
			_titleText.text = _titleCards [2];
			break;
		case State.PeepHole:
			_titleText.text = _titleCards [3];
			break;
		default:
			break;
		}
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			switch (StateManager._stateManager.currentState) {
			case State.Null:
				StateManager._stateManager.currentState = State.Zoetrope;
				StartCoroutine(StateManager._stateManager.ChangeLevel((int)StateManager._stateManager.currentState));
				break;
			case State.Zoetrope:
				StateManager._stateManager.currentState = State.MusicBox;
				StartCoroutine(StateManager._stateManager.ChangeLevel((int)StateManager._stateManager.currentState));
				break;
			case State.MusicBox:
				StateManager._stateManager.currentState = State.PeepHole;
				StartCoroutine(StateManager._stateManager.ChangeLevel((int)StateManager._stateManager.currentState));
				break;
			case State.PeepHole:
				StateManager._stateManager.currentState = State.Null;
				StartCoroutine(StateManager._stateManager.ChangeLevel(0));
				break;
			default:
				break;
			}
		}
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnLevelLoad;
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnLevelLoad;
	}
}
