using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancerStopSoundEffect : AudioSourceController {
	[SerializeField] AudioSystem _stopSoundEffect;

	float _timeBetweenNotesInSeconds = 0.789f;
	Timer _delayBetweenTimer;
	bool _isOn = false;
	bool _waitToStop = false;
	bool _timeToStart = false;

	void Start(){
		_delayBetweenTimer = new Timer (_timeBetweenNotesInSeconds);
	}

	public void ToggleStopSound(bool isOn){
		Debug.Log ("called this thing?:");
		if (isOn) {
			_isOn = isOn;
			_timeToStart = false;
			PlayStopSoundEffect ();
			_delayBetweenTimer.Reset ();
		} else {
			_waitToStop = true;
		}
	}

	public bool LookForTimeToStart(){
		return _timeToStart;
	}


	void Update() {
		if (_isOn) {
			if (_delayBetweenTimer.IsOffCooldown) {
				if (!_waitToStop) {
					PlayStopSoundEffect ();
					_delayBetweenTimer.Reset ();
				} else {
					_timeToStart = true;
					_waitToStop = false;
					_isOn = false;
				}
			}
		}
	}

	void PlayStopSoundEffect(){
		//_stopSoundEffect.audioSource.clip = AudioManager.instance.GetRandomClip (_stopSoundEffect);
		if (!_stopSoundEffect.audioSource.isPlaying) {
			_stopSoundEffect.audioSource.Play ();
		}
	}
}
