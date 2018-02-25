using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreSound : MonoBehaviour {
	public static TheatreSound _instance;

	void Start(){
		_instance = this;
	}

	[SerializeField] AudioSource _clappingSound;
	[SerializeField] AudioClip[] _clapClips = new AudioClip[4];

	[SerializeField] AudioSource _lightSwitch;

	[SerializeField] AudioSource _bellFeedback;
	[SerializeField] AudioSource _frogSound;

	[SerializeField] AudioSource _starSound;

	public void PlayClapSound(int intensityIndex){
		_clappingSound.clip = _clapClips [intensityIndex];
		_clappingSound.Play ();
	}

	public void PlayLightSwitch(){
		_lightSwitch.Play ();
	}

	public void PlayBellFeedback(){
		_bellFeedback.Play ();
	}

	public void PlayFrogSound(){
		_frogSound.Play ();
	}

	public void PlayStarSound(){
		_starSound.Play ();
	}
}
