using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScrollSound : MonoBehaviour {
	[Header("0: Open, 1: Close")]
	[SerializeField] AudioClip[] _audioClips;
	[SerializeField] AudioSource _audioSource;

	public void OpenWindowSound(){
		_audioSource.clip = _audioClips [0];
		_audioSource.Play ();
	}

	public void CloseWindowSound(){
		_audioSource.clip = _audioClips [1];
		_audioSource.Play ();
	}
}
