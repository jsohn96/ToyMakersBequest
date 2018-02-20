using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreSound : MonoBehaviour {
	[SerializeField] AudioSource _clappingSound;
	[SerializeField] AudioClip[] _clapClips = new AudioClip[4];

	public void PlayClapSound(int intensityIndex){
		_clappingSound.clip = _clapClips [intensityIndex];
		_clappingSound.Play ();
	}
}
