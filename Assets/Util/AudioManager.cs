using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public void Play(AudioSource audioSource, float duration = 0.5f){
		StartCoroutine (SmoothPlay (audioSource, duration));
	}
	IEnumerator SmoothPlay(AudioSource audioSource, float duration){
		float tempVolume = audioSource.volume;
		Timer audioTimer = new Timer (duration);
		audioTimer.Reset ();
		audioSource.volume = 0.0f;
		if (!audioSource.isPlaying) {
			audioSource.Play ();
		}

		while (!audioTimer.IsOffCooldown) {
			audioSource.volume = MathHelpers.LinMapFrom01(0.0f, tempVolume, _audioFadeCurve.Evaluate (audioTimer.PercentTimePassed));
			yield return null;
		}
		audioSource.volume = tempVolume;
	}

	public void Pause(AudioSource audioSource, float duration = 0.5f){
		StartCoroutine (SmoothPause (audioSource, duration));
	}
	IEnumerator SmoothPause(AudioSource audioSource, float duration){
		float tempVolume = audioSource.volume;
		Timer audioTimer = new Timer (duration);
		audioTimer.Reset ();
		while (!audioTimer.IsOffCooldown) {
			audioSource.volume = MathHelpers.LinMapFrom01(0.0f, tempVolume, _audioFadeCurve.Evaluate (audioTimer.PercentTimeLeft));
			yield return null;
		}
		audioSource.Pause ();
		audioSource.volume = tempVolume;
	}

	public void Resume(AudioSource audioSource, float duration = 0.5f){
		StartCoroutine (SmoothResume (audioSource, duration));
	}
	IEnumerator SmoothResume(AudioSource audioSource, float duration){
		float tempVolume = audioSource.volume;
		Timer audioTimer = new Timer (duration);
		audioTimer.Reset ();
		audioSource.volume = 0.0f;
		audioSource.UnPause ();
		while (!audioTimer.IsOffCooldown) {
			audioSource.volume = MathHelpers.LinMapFrom01(0.0f, tempVolume, _audioFadeCurve.Evaluate (audioTimer.PercentTimePassed));
			yield return null;
		}

		audioSource.volume = tempVolume;
	}

	public void Stop(AudioSource audioSource, float duration = 0.5f){
		StartCoroutine (SmoothStop (audioSource, duration));
	}
	IEnumerator SmoothStop(AudioSource audioSource, float duration){
		float tempVolume = audioSource.volume;
		Timer audioTimer = new Timer (duration);
		audioTimer.Reset ();
		while (!audioTimer.IsOffCooldown) {
			audioSource.volume = MathHelpers.LinMapFrom01(0.0f, tempVolume, _audioFadeCurve.Evaluate (audioTimer.PercentTimeLeft));
			yield return null;
		}
		audioSource.Stop ();
		audioSource.volume = tempVolume;
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
}
