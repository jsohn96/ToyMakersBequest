using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public enum MusicBoxLayer {
	MusicBox = 0,
	MusicBoxAccompany = 1,
	PianoMelody = 2,
	PianoAccompany = 3,
	WoodClick = 4
}

[RequireComponent(typeof(AudioSource))]

public class MusicBoxKoreoController : AudioSourceController {
	[SerializeField] AudioSystem _audioSystem;

	[SerializeField] MusicBoxLayer _whichLayer = MusicBoxLayer.MusicBox;

	//sample rate will likely be 44100 for calculating delay for koreography
	[SerializeField] int _sampleRate = 44100;

	MultiMusicPlayer _multiMusicPlayer;
	[SerializeField] MultiMusicPlayer _sourceMultiMusicPlayer;

	// check this bool if koreography needs to be paused
	bool _stop = false;
	// check if the pause process has started
	bool _audioFadeStarted = false;
	//set to true if audio is paused but resume hasnt been called
	bool _waitingForResume = false;
	// check if the music box level has been started
	bool _isStarted = false;

	bool _readyToEnter = false;

	//[SerializeField] float _resumeAudioDuration = 0.05f;

	//The end sample value for holding where the track left off at
	int _tempSampleForUnpause = 0;

	IEnumerator _delayedPauseCoroutine;
	IEnumerator _storedResumeKoreography;

	void Start () {
		if (_audioSystem.audioSource == null) {
			_audioSystem.audioSource = GetComponent<AudioSource> ();
		}

		// Subscribe to Koreographer Events
		if (_whichLayer != MusicBoxLayer.WoodClick) {
			Koreographer.Instance.RegisterForEvents ("WoodGearClickTrack", TogglePhaseIn);
		}
		Koreographer.Instance.RegisterForEvents ("MusicBoxMelodyTrack", CircleKoreoHandle);

		// get koreography music player
		_multiMusicPlayer = GetComponent<MultiMusicPlayer> ();
	
	}

	void TogglePhaseIn(KoreographyEvent koreoEvent){
		if (_readyToEnter) {
			bool isInterrupted = false;

			if (_audioFadeStarted) {
				StopCoroutine (_delayedPauseCoroutine);
				isInterrupted = true;
				_audioFadeStarted = false;
			}
			if (!isInterrupted) {
				Debug.Log ("RESUME RESUME RESUEME");
				_multiMusicPlayer.SeekToSample(_tempSampleForUnpause);
				_multiMusicPlayer.Play ();

			}
			AdjustVolume (_audioSystem, _audioSystem.fadeDuration, _audioSystem.volume, _audioSystem.audioSource.volume, false, true);
			_readyToEnter = false;


		}
	}

	//Set so that the first event will provide information on the next segment and its length
	void CircleKoreoHandle(KoreographyEvent koreoEvent) {
		if (_stop && !_audioFadeStarted && !_waitingForResume) {
			_audioFadeStarted = true;
			_waitingForResume = true;
			float fadeDuration = CalculateFadeDuration (koreoEvent);

			if (_whichLayer != MusicBoxLayer.WoodClick) {
				//reduces the volume of the audio to 0, because koreographer requires a separate Pause
			//AdjustVolume (_audioSystem, fadeDuration, 0.0f, _audioSystem.audioSource.volume, true);
				//Coroutine to wait for the actual pause time
				_delayedPauseCoroutine = CountdownToKoreoPause (fadeDuration);
				StartCoroutine (_delayedPauseCoroutine);
			}
		} 
//		else if (!_stop && _waitingForResume) {
//			//if (!_inactive) {
//			_waitingForResume = false;
//				float fadeDuration = CalculateFadeDuration (koreoEvent);
//				StartCoroutine (TempResume (fadeDuration));
//			//}
//		}
	}

//	IEnumerator TempResume(float fadeDuration){
//		_audioFadeStarted = false;
//		yield return new WaitForSeconds (fadeDuration);
//		AdjustVolume (_audioSystem, _resumeAudioDuration, _audioSystem.volume, _audioSystem.audioSource.volume, true);
//
//	}

	IEnumerator CountdownToKoreoPause(float fadeDuration){
		//Maybe have the stop sound play at the halfway point?
		yield return new WaitForSeconds (fadeDuration);
		AdjustVolume (_audioSystem, 0.0f, 0.0f, _audioSystem.audioSource.volume, false, true);
		_multiMusicPlayer.Pause ();
		_audioFadeStarted = false;

		/*if (_dancerStopSoundEffectScript != null) {
			_dancerStopSoundEffectScript.ToggleStopSound (true);
		}*/

		yield return null;
	}
		
	//Calculates the duration of the koreoevent and converts it into seconds
	//Used for calculating the duration of the audio fade out
	float CalculateFadeDuration(KoreographyEvent koreoEvent){
		int sampleDifference = koreoEvent.EndSample - _sourceMultiMusicPlayer.GetSampleTimeForClip(_sourceMultiMusicPlayer.GetCurrentClipName());
		_tempSampleForUnpause = koreoEvent.EndSample;
		float duration = (float)sampleDifference / (float)_sampleRate;
		return duration;
	}

	// Function to set a bool to wait for pause
	void StopMusicBoxMusic(MBMusicMangerEvent e){
		_stop = !e.isMusicPlaying;
		if (!_isStarted && !_stop) {
			_isStarted = true;
			_multiMusicPlayer.Play ();
		}

		if (!_stop && _waitingForResume) {
			if (_delayedPauseCoroutine != null) {
				StopCoroutine (_delayedPauseCoroutine);
			}
			_waitingForResume = false;
			_audioFadeStarted = false;
			_readyToEnter = true;
		}
	}



	void ToggleMusicLayer(MBMusicLayerAdjustmentEvent e){
		if (_whichLayer == e.ThisMusicBoxLayer) {

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
