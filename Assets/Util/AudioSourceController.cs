using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour {

	public void SwapClip(AudioSystem audioSystem, bool abruptSwap = false){
		if (abruptSwap || !audioSystem.audioSource.isPlaying) {
			audioSystem.audioSource.clip = AudioManager.instance.GetRandomClip (audioSystem);
			Play (audioSystem);		
		}
	}

	// Handles the coroutines and calls appropriate functions in AudioManager
	public void Play(AudioSystem audioSystem){
		if (audioSystem.coroutine != null) {
			StopCoroutine (audioSystem.coroutine);
		}
		audioSystem.coroutine = AudioManager.instance.SmoothPlay (audioSystem.audioSource, audioSystem.volume, audioSystem.fadeDuration);
		StartCoroutine (audioSystem.coroutine);
	}

	public void Pause(AudioSystem audioSystem){
		if (audioSystem.coroutine != null) {
			StopCoroutine (audioSystem.coroutine);
		}
		audioSystem.coroutine = AudioManager.instance.SmoothPause (audioSystem.audioSource, audioSystem.audioSource.volume, audioSystem.fadeDuration);
		StartCoroutine (audioSystem.coroutine);
	}

	public void Resume(AudioSystem audioSystem){
		if (audioSystem.coroutine != null) {
			StopCoroutine (audioSystem.coroutine);
		}
		audioSystem.coroutine = AudioManager.instance.SmoothResume (audioSystem.audioSource, audioSystem.volume, audioSystem.fadeDuration);
		StartCoroutine (audioSystem.coroutine);
	}

	public void Stop(AudioSystem audioSystem){
		if (audioSystem.coroutine != null) {
			StopCoroutine (audioSystem.coroutine);
		}
		audioSystem.coroutine = AudioManager.instance.SmoothStop (audioSystem.audioSource, audioSystem.audioSource.volume, audioSystem.fadeDuration);
		StartCoroutine (audioSystem.coroutine);
	}

	public void AdjustVolume(AudioSystem audioSystem, float duration, float goalVolume, float originVolume, bool continueAfterZero = false, bool isKoreo = false){
		if (audioSystem.coroutine != null) {
			StopCoroutine (audioSystem.coroutine);
		}
		if (goalVolume > originVolume) {
			audioSystem.coroutine = AudioManager.instance.FadeIn (audioSystem.audioSource, duration, goalVolume, originVolume, isKoreo);
		} else {
			audioSystem.coroutine = AudioManager.instance.FadeOut (audioSystem.audioSource, duration, goalVolume, originVolume, continueAfterZero, isKoreo);
		}
		StartCoroutine (audioSystem.coroutine);
	}
}
