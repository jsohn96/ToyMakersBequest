using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbientSoundController : AudioSourceController {
	[SerializeField] AudioSystem _ambientAudioSystem1;
	[SerializeField] AudioSystem _ambientAudioSystem2;
	//Use _abientAudioSystem, 0: RoomToneForBook

	bool _ambienceReduced = false;

	void OnSceneLoaded(Scene Scene, LoadSceneMode mode){
		if (Scene.name == "notebookScene" || Scene.name == "ZoetropeTest" || Scene.name == "ControlRoom") {
			if (_ambientAudioSystem1.audioSource.isPlaying) {
				_ambientAudioSystem2.audioSource.clip = _ambientAudioSystem1.clips [0];
				AudioManager.instance.CrossFade (_ambientAudioSystem1.audioSource, _ambientAudioSystem2.audioSource, _ambientAudioSystem1.fadeDuration, _ambientAudioSystem1.volume);
			} else {
				_ambientAudioSystem1.audioSource.clip = _ambientAudioSystem1.clips [0];
				AudioManager.instance.CrossFade (_ambientAudioSystem2.audioSource, _ambientAudioSystem1.audioSource, _ambientAudioSystem1.fadeDuration, _ambientAudioSystem1.volume);
			}
		}
	}

	void AmbienceAdjustment(AmbientSoundAdjustmentEvent e){
		if (!_ambienceReduced && e.ReduceAmbientSound) {
			if (_ambientAudioSystem1.audioSource.isPlaying) {
				AdjustVolume (_ambientAudioSystem1, _ambientAudioSystem1.fadeDuration, 0.0f, _ambientAudioSystem1.audioSource.volume, true);
			} else {
				AdjustVolume (_ambientAudioSystem2, _ambientAudioSystem2.fadeDuration, 0.0f, _ambientAudioSystem2.audioSource.volume, true);
			}
			_ambienceReduced = true;
		} else if (_ambienceReduced && !e.ReduceAmbientSound) {
			if (_ambientAudioSystem1.audioSource.isPlaying) {
				AdjustVolume (_ambientAudioSystem1, _ambientAudioSystem1.fadeDuration, _ambientAudioSystem1.volume, _ambientAudioSystem1.audioSource.volume, true);
			} else {
				AdjustVolume (_ambientAudioSystem2, _ambientAudioSystem2.fadeDuration, _ambientAudioSystem2.volume, _ambientAudioSystem2.audioSource.volume, true);
			}
			_ambienceReduced = false;
		}
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
		Events.G.AddListener<AmbientSoundAdjustmentEvent> (AmbienceAdjustment);
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
		Events.G.RemoveListener<AmbientSoundAdjustmentEvent> (AmbienceAdjustment);
	}
}
