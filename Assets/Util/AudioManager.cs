using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSystem {
	public AudioSource audioSource;
	public float volume;
	public float fadeDuration;
	public AudioClip[] clips;
	public MinMax pitchRange;
	public IEnumerator coroutine;
	public int previouslyPlayedIndex;

	public AudioSystem (
		AudioSource _audioSource,
		float _volume,
		float _fadeDuration,
		AudioClip[] _clips,
		MinMax _pitchRange,
		IEnumerator _coroutine,
		int _previouslyPlayedIndex){
		audioSource = _audioSource;
		volume = _volume;
		fadeDuration = _fadeDuration;
		clips = _clips;
		pitchRange = _pitchRange;
		coroutine = _coroutine;
		previouslyPlayedIndex = -99;
	}
}

public class AudioManager : MonoBehaviour {
	//persists the script between scenes and makes script accessible by all
	public static AudioManager instance = null;

	//Animation curve variable to have exponential descent for fade
	[SerializeField] AnimationCurve _audioFadeCurve;

	void Awake () {
		//assign an instance of this gameobject if it hasn't been assigned before
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}


	public IEnumerator SmoothPlay(AudioSource audioSource, float sourceVolume, float duration){
		Timer audioTimer = new Timer (duration);
		audioTimer.Reset ();
		audioSource.volume = 0.0f;
		if (!audioSource.isPlaying) {
			audioSource.Play ();
		}

		while (!audioTimer.IsOffCooldown) {
			audioSource.volume = MathHelpers.LinMapFrom01(0.0f, sourceVolume, _audioFadeCurve.Evaluate (audioTimer.PercentTimePassed));
			yield return null;
		}
		audioSource.volume = sourceVolume;
	}
		
	public IEnumerator SmoothPause(AudioSource audioSource, float sourceVolume, float duration){
		Timer audioTimer = new Timer (duration);
		audioTimer.Reset ();
		while (!audioTimer.IsOffCooldown) {
			audioSource.volume = MathHelpers.LinMapFrom01(0.0f, sourceVolume, _audioFadeCurve.Evaluate (audioTimer.PercentTimeLeft));
			yield return null;
		}
		audioSource.Pause ();
		audioSource.volume = sourceVolume;
	}
		
	public IEnumerator SmoothResume(AudioSource audioSource, float sourceVolume, float duration){
		Timer audioTimer = new Timer (duration);
		audioTimer.Reset ();
		audioSource.volume = 0.0f;
		audioSource.UnPause ();
		while (!audioTimer.IsOffCooldown) {
			audioSource.volume = MathHelpers.LinMapFrom01(0.0f, sourceVolume, _audioFadeCurve.Evaluate (audioTimer.PercentTimePassed));
			yield return null;
		}

		audioSource.volume = sourceVolume;
	}
		
	public IEnumerator SmoothStop(AudioSource audioSource, float sourceVolume, float duration){
		Timer audioTimer = new Timer (duration);
		audioTimer.Reset ();
		while (!audioTimer.IsOffCooldown) {
			audioSource.volume = MathHelpers.LinMapFrom01(0.0f, sourceVolume, _audioFadeCurve.Evaluate (audioTimer.PercentTimeLeft));
			yield return null;
		}
		audioSource.Stop ();
		audioSource.volume = sourceVolume;
	}


	public void AdjustVolume(AudioSource audioSource, float duration, float goalVolume, float originVolume){
		if (goalVolume > originVolume) {
			StartCoroutine (FadeIn (audioSource, duration, goalVolume, originVolume));
		} else {
			StartCoroutine (FadeOut (audioSource, duration, goalVolume, originVolume));
		}
	}

	//Crossfades audio, defaults to 1 second
	public void CrossFade(AudioSource outAudio, AudioSource inAudio, float fadeDuration = 1f, float onVolume = 1f) {
		StartCoroutine(FadeIn (inAudio, fadeDuration, onVolume));
		StartCoroutine(FadeOut (outAudio, fadeDuration, 0f, outAudio.volume));
	}

	IEnumerator FadeIn(AudioSource audioSource, float duration, float goalVolume = 1.0f, float originVolume = 0.0f){
		if (!audioSource.isPlaying) {
			audioSource.Play ();
		}
		Timer fadeTimer = new Timer (duration);
		fadeTimer.Reset ();
		while (!fadeTimer.IsOffCooldown) {
			audioSource.volume = MathHelpers.LinMapFrom01(originVolume, goalVolume, _audioFadeCurve.Evaluate (fadeTimer.PercentTimePassed));
			yield return null;
		}
		audioSource.volume = goalVolume;
	}

	IEnumerator FadeOut(AudioSource audioSource, float duration, float goalVolume = 0.0f, float originVolume = 1.0f){
		float tempOnVolume = audioSource.volume;
		Timer fadeTimer = new Timer (duration);
		fadeTimer.Reset ();
		while (!fadeTimer.IsOffCooldown) {
			audioSource.volume = MathHelpers.LinMapFrom01(originVolume, goalVolume, _audioFadeCurve.Evaluate (fadeTimer.PercentTimeLeft));
			yield return null;
		}
		if (goalVolume == 0.0f) {
			audioSource.Stop ();
		} else {
			audioSource.volume = goalVolume;
		}

	}

	public void RandomizePitchFromRange(AudioSystem audioSystem) {
		audioSystem.audioSource.pitch = Random.Range (audioSystem.pitchRange.Min, audioSystem.pitchRange.Max);
	}

	//pseudo randomly chooses between audioclips
	public AudioClip GetRandomClip(AudioSystem audioSystem){
		int index;
		index = Random.Range (0, audioSystem.clips.Length);
		if (audioSystem.previouslyPlayedIndex != -99 && index == audioSystem.previouslyPlayedIndex) {
			// if it is a repeated clip, call this function again until a different result
			return GetRandomClip (audioSystem);
		} else {
			audioSystem.previouslyPlayedIndex = index;
			return audioSystem.clips [index];
		}
	}
}
