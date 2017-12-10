using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbientSoundController : AudioSourceController {
	[SerializeField] AudioSystem _ambientAudioSystem1;
	[SerializeField] AudioSystem _ambientAudioSystem2;
	//Use _abientAudioSystem, 0: RoomToneForBook

	void OnSceneLoaded(Scene Scene, LoadSceneMode mode){
		if (Scene.name == "notebookScene") {
			if (_ambientAudioSystem1.audioSource.isPlaying) {
				_ambientAudioSystem2.audioSource.clip = _ambientAudioSystem1.clips [0];
				AudioManager.instance.CrossFade (_ambientAudioSystem1.audioSource, _ambientAudioSystem2.audioSource, _ambientAudioSystem1.fadeDuration, _ambientAudioSystem1.volume);
			} else {
				_ambientAudioSystem1.audioSource.clip = _ambientAudioSystem1.clips [0];
				AudioManager.instance.CrossFade (_ambientAudioSystem2.audioSource, _ambientAudioSystem1.audioSource, _ambientAudioSystem1.fadeDuration, _ambientAudioSystem1.volume);
			}
		}
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
