using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatrePart2Music : AudioSourceController {

	public enum WhichPart2 {
		piano,
		melody,
		accompany
	}

	public static TheatrePart2Music _instance;

	[SerializeField] AudioSource _piano;
	[SerializeField] AudioSource _melody;
	[SerializeField] AudioSource _accompany;
	[SerializeField] AudioSource _accompanyExtend;
	bool _waitForEnd = false;
	bool _repeatOnce = false;
	[SerializeField] AltTheatre _myTheatre;

	void Awake () {
		_instance = this;
		DontDestroyOnLoad (this);
	}

	void FixedUpdate(){
		if (_waitForEnd) {
			if (_repeatOnce) {
				if (!_accompanyExtend.isPlaying) {
					_myTheatre.MoveToNext ();
					_waitForEnd = false;
				}
			} else if (!_accompany.isPlaying) {
				_accompanyExtend.Play ();
				_repeatOnce = true;
				
			}
		}
	}
		
	public void BeginPlay(bool includeMelody = false){
		_piano.volume = 0f;
		_accompany.volume = 1f;
		if (includeMelody) {
			_melody.volume = 1f;
			_accompany.loop = true;
			_piano.volume = 0.5f;
			_accompany.PlayDelayed (0f);
			_piano.PlayDelayed (0f);
			_melody.PlayDelayed (0f);
			_waitForEnd = false;
		} else {
			_accompany.loop = false;
			_melody.volume = 0f;
			_waitForEnd = true;
			_accompany.Play ();
		}
	}

	public void StopAll(){
		_piano.Stop();
		_melody.Stop ();
		_accompany.Stop();
	}

	IEnumerator VolumeChange(bool on, WhichPart2 part2){
		float duration = 3f;
		float timer = 0f;
		float tempVolume;
		while (duration > timer) {
			timer += Time.deltaTime;
			if (on) {
				tempVolume = Mathf.Lerp (0f, 1f, timer / duration);
			} else {
				tempVolume = Mathf.Lerp (1f, 0f, timer / duration);
			}
			if (part2 == WhichPart2.accompany) {
				_accompany.volume = tempVolume;
			} else if (part2 == WhichPart2.melody) {
				_melody.volume = tempVolume;
			} else {
				tempVolume = tempVolume / 2f;
				_piano.volume = tempVolume;
			}
			yield return null;
		}
		if (on) {
			tempVolume = 1f;
		} else {
			tempVolume = 0f;
		}
		if (part2 == WhichPart2.accompany) {
			_accompany.volume = tempVolume;
		} else if (part2 == WhichPart2.melody) {
			_melody.volume = tempVolume;
		} else {
			tempVolume = tempVolume / 2f;
			_piano.volume = tempVolume;
		}
		yield return null;
	}

	public void PlayAccompany(bool on){
		if (on) {
			StartCoroutine (VolumeChange (true, WhichPart2.accompany));
		} else {
			StartCoroutine (VolumeChange (false, WhichPart2.accompany));
		}
	}

	public void PlayMelody(bool on){
		if (on) {
			StartCoroutine (VolumeChange (true, WhichPart2.melody));
		} else {
			StartCoroutine (VolumeChange (false, WhichPart2.melody));
		}
	}

	public void PlayPiano(bool on){
		if (on) {
			StartCoroutine (VolumeChange (true, WhichPart2.piano));
		} else {
			StartCoroutine (VolumeChange (false, WhichPart2.piano));
		}
	}
}
