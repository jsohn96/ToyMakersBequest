using System.Collections;
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

	void Start () {
		if (_audioSystem.audioSource == null) {
			_audioSystem.audioSource = GetComponent<AudioSource> ();
		}

		// Subscribe to Koreographer Events
		Koreographer.Instance.RegisterForEvents ("CircleKoreographyTrack", CircleKoreoHandle);
		// get koreography music player
		_simpleMusicPlayer = GetComponent<SimpleMusicPlayer> ();

		if (_inactive) {
			_audioSystem.audioSource.volume = 0.0f;
		}
	}

	//Set so that the first event will provide information on the next segment and its length
	void CircleKoreoHandle(KoreographyEvent koreoEvent) {
		if (_stop && !_audioFadeStarted && !_waitingForResume) {
			_audioFadeStarted = true;
			_waitingForResume = true;
			float fadeDuration = CalculateFadeDuration (koreoEvent);

			//reduces the volume of the audio to 0, because koreographer requires a separate Pause
			AdjustVolume (_audioSystem, fadeDuration, 0.0f, _audioSystem.audioSource.volume, true);
			//Coroutine to wait for the actual pause time
			_delayedPauseCoroutine = CountdownToKoreoPause (fadeDuration);
			StartCoroutine (_delayedPauseCoroutine);
		}
	}

	IEnumerator CountdownToKoreoPause(float fadeDuration){
		yield return new WaitForSeconds (fadeDuration);
		_simpleMusicPlayer.Pause ();
		_audioFadeStarted = false;
	}
		
	//Calculates the duration of the koreoevent and converts it into seconds
	//Used for calculating the duration of the audio fade out
	float CalculateFadeDuration(KoreographyEvent koreoEvent){
		int sampleDifference = koreoEvent.EndSample - _sourceSimpleMusicPlayer.GetSampleTimeForClip(_sourceSimpleMusicPlayer.GetCurrentClipName());
		_tempSampleForUnpause = koreoEvent.EndSample;
		float duration = (float)sampleDifference / (float)_sampleRate;
		return duration;
	}

	void ResumeKoreography(){
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
	}

	// Function to set a bool to wait for pause
	void StopMusicBoxMusic(MBMusicMangerEvent e){
		_stop = !e.isMusicPlaying;
		if (!_isStarted && !_stop) {
			_simpleMusicPlayer.Play ();
		}

		if (!_stop && _waitingForResume) {
			_waitingForResume = false;
			_audioFadeStarted = false;
			ResumeKoreography ();
		}
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

	void PathStateManage(PathStateManagerEvent e){
		if (e.activeEvent == PathState.descend_inital_stage) {
			if (_whichLayer == MusicBoxLayer.slowPiano) {
				ActivateLayer ();
			}
		} else if (e.activeEvent == PathState.open_gate) {
			if (_whichLayer == MusicBoxLayer.slowLowPiano) {
				ActivateLayer ();
			}

		} else if (e.activeEvent == PathState.flip_TM_stage) {
			if (_whichLayer == MusicBoxLayer.pianoMelody) {
				ActivateLayer ();
			}
		} else if (e.activeEvent == PathState.descend_to_layer_two) {
			if (_whichLayer == MusicBoxLayer.pianoMelody) {
				DeactivateLayer ();

			}
			if (_whichLayer == MusicBoxLayer.slowPiano) {
				DeactivateLayer ();
			}
			if (_whichLayer == MusicBoxLayer.slowLowPiano) {
				DeactivateLayer ();
			}
		}
	}

	void OnEnable(){
		Events.G.AddListener<MBMusicMangerEvent> (StopMusicBoxMusic);
		Events.G.AddListener<MBMusicLayerAdjustmentEvent> (ToggleMusicLayer);
		Events.G.AddListener<PathStateManagerEvent> (PathStateManage);
	}
	void OnDisable(){
		Events.G.RemoveListener<MBMusicMangerEvent> (StopMusicBoxMusic);
		Events.G.RemoveListener<MBMusicLayerAdjustmentEvent> (ToggleMusicLayer);
		Events.G.RemoveListener<PathStateManagerEvent> (PathStateManage);
	}
}
