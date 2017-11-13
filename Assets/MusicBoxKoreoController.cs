﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public enum MusicBoxLayer {
	slowMusicBox = 0,
	slowPiano = 1,
	slowLowPiano = 2,
	pianoMelody = 3
}

[RequireComponent(typeof(AudioSource))]

public class MusicBoxKoreoController : AudioSourceController {
	[SerializeField] AudioSystem _audioSystem;

	[SerializeField] MusicBoxLayer _whichLayer = MusicBoxLayer.slowMusicBox;

	[SerializeField] bool _inactive = false;

	//sample rate will likely be 44100 for calculating delay for koreography
	[SerializeField] int _sampleRate = 44100;

	SimpleMusicPlayer _simpleMusicPlayer;
	[SerializeField] SimpleMusicPlayer _sourceSimpleMusicPlayer;

	// check this bool if koreography needs to be paused
	bool _stop = false;
	// check if the pause process has started
	bool _audioFadeStarted = false;
	//set to true if audio is paused but resume hasnt been called
	bool _waitingForResume = false;
	// check if the music box level has been started
	bool _isStarted = false;

	[SerializeField] float _resumeAudioDuration = 0.05f;

	//The end sample value for holding where the track left off at
	int _tempSampleForUnpause = 0;

	IEnumerator _delayedPauseCoroutine;
	IEnumerator _storedResumeKoreography;

	//[SerializeField] DancerStopSoundEffect _dancerStopSoundEffectScript;

	void Start () {
		if (_audioSystem.audioSource == null) {
			_audioSystem.audioSource = GetComponent<AudioSource> ();
		}

		// Subscribe to Koreographer Events
		if (_whichLayer != MusicBoxLayer.slowLowPiano) {
			Koreographer.Instance.RegisterForEvents ("MusicBoxMelodyTrack", CircleKoreoHandle);
			Koreographer.Instance.RegisterForEvents ("MusicBoxAccompanyTrack", TogglePhaseIn);
		}

		// get koreography music player
		_simpleMusicPlayer = GetComponent<SimpleMusicPlayer> ();

		if (_inactive) {
			_audioSystem.audioSource.volume = 0.0f;
		}
	}

	void TogglePhaseIn(KoreographyEvent koreoEvent){
		
	}

	//Set so that the first event will provide information on the next segment and its length
	void CircleKoreoHandle(KoreographyEvent koreoEvent) {
		if (_stop && !_audioFadeStarted && !_waitingForResume) {
			_audioFadeStarted = true;
			_waitingForResume = true;
			float fadeDuration = CalculateFadeDuration (koreoEvent);

			//reduces the volume of the audio to 0, because koreographer requires a separate Pause
			AdjustVolume (_audioSystem, fadeDuration, 0.0f, _audioSystem.audioSource.volume, true);
			_inactive = true;
			//Coroutine to wait for the actual pause time
			//_delayedPauseCoroutine = CountdownToKoreoPause (fadeDuration);
			//StartCoroutine (_delayedPauseCoroutine);
		} else if (!_stop && _waitingForResume) {
			//if (!_inactive) {
			_waitingForResume = false;
				float fadeDuration = CalculateFadeDuration (koreoEvent);
				StartCoroutine (TempResume (fadeDuration));
			//}
		}
	}

	IEnumerator TempResume(float fadeDuration){
		_audioFadeStarted = false;
		yield return new WaitForSeconds (fadeDuration);
		_inactive = false;
		AdjustVolume (_audioSystem, _resumeAudioDuration, 1.0f, _audioSystem.audioSource.volume, true);

	}

	IEnumerator CountdownToKoreoPause(float fadeDuration){
		//Maybe have the stop sound play at the halfway point?


		yield return new WaitForSeconds (fadeDuration);
		_simpleMusicPlayer.Pause ();
		_audioFadeStarted = false;

		/*if (_dancerStopSoundEffectScript != null) {
			_dancerStopSoundEffectScript.ToggleStopSound (true);
		}*/

		yield return null;
	}
		
	//Calculates the duration of the koreoevent and converts it into seconds
	//Used for calculating the duration of the audio fade out
	float CalculateFadeDuration(KoreographyEvent koreoEvent){
		int sampleDifference = koreoEvent.EndSample - _sourceSimpleMusicPlayer.GetSampleTimeForClip(_sourceSimpleMusicPlayer.GetCurrentClipName());
		_tempSampleForUnpause = koreoEvent.EndSample;
		float duration = (float)sampleDifference / (float)_sampleRate;
		return duration;
	}

	IEnumerator ResumeKoreography(){
		/*
		_dancerStopSoundEffectScript.ToggleStopSound (false);
		while (!_dancerStopSoundEffectScript.LookForTimeToStart ()) {
			yield return null;
		}
*/
		bool isInterrupted = false;
		if (_delayedPauseCoroutine != null) {
			StopCoroutine (_delayedPauseCoroutine);
			isInterrupted = true;
			_audioFadeStarted = false;
		}
		if (!isInterrupted) {
			_simpleMusicPlayer.SeekToSample (_tempSampleForUnpause);
		}
		if (!_inactive) {
			AdjustVolume (_audioSystem, _resumeAudioDuration, 1.0f, _audioSystem.audioSource.volume, true);
		}
		yield return null;
	}

	// Function to set a bool to wait for pause
	void StopMusicBoxMusic(MBMusicMangerEvent e){
		_stop = !e.isMusicPlaying;
		if (!_isStarted && !_stop) {
			_simpleMusicPlayer.Play ();
		}

		//if (!_stop && _waitingForResume) {
		//	_waitingForResume = false;
		//	_audioFadeStarted = false;
		//	StartCoroutine(ResumeKoreography ());
		//}
	}

	public void ActivateLayer(){
		if (_inactive) {
			_inactive = false;
			AdjustVolume (_audioSystem, 0.5f, 1.0f, _audioSystem.audioSource.volume, true);
		}
	}

	public void DeactivateLayer(){
		if (!_inactive) {
			_inactive = true;
			_stop = true;
		}
	}

	void ToggleMusicLayer(MBMusicLayerAdjustmentEvent e){
		if (_whichLayer == e.ThisMusicBoxLayer) {
			if (e.IsOn) {
				ActivateLayer ();
			} else {
				DeactivateLayer ();
			}
		}
	}

	void OnEnable(){
		Events.G.AddListener<MBMusicMangerEvent> (StopMusicBoxMusic);
		Events.G.AddListener<MBMusicLayerAdjustmentEvent> (ToggleMusicLayer);
		//Events.G.AddListener<PathStateManagerEvent> (PathStateManage);
	}
	void OnDisable(){
		Events.G.RemoveListener<MBMusicMangerEvent> (StopMusicBoxMusic);
		Events.G.RemoveListener<MBMusicLayerAdjustmentEvent> (ToggleMusicLayer);
		//Events.G.RemoveListener<PathStateManagerEvent> (PathStateManage);
	}
}
