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
		StartCoroutine (AudioManager.instance.SmoothPause (audioSystem.audioSource, audioSystem.volume, audioSystem.fadeDuration));
		StartCoroutine (audioSystem.coroutine);
	}

	public void Resume(AudioSystem audioSystem){
		if (audioSystem.coroutine != null) {
			StopCoroutine (audioSystem.coroutine);
		}
		StartCoroutine (AudioManager.instance.SmoothResume (audioSystem.audioSource, audioSystem.volume, audioSystem.fadeDuration));
		StartCoroutine (audioSystem.coroutine);
	}

	public void Stop(AudioSystem audioSystem){
		if (audioSystem.coroutine != null) {
			StopCoroutine (audioSystem.coroutine);
		}
		StartCoroutine (AudioManager.instance.SmoothStop (audioSystem.audioSource, audioSystem.volume, audioSystem.fadeDuration));
		StartCoroutine (audioSystem.coroutine);
	}
}
