﻿using System.Collections;
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
	[SerializeField] AudioSource _frogLandingSound;
	[SerializeField] AudioSource _frogPuddleSound;

	[SerializeField] AudioSource _crowCawSound;

	[SerializeField] AudioSource _kissSound;
	[SerializeField] AudioSource _waterTankMoveSound;

	[SerializeField] AudioSource _dancerEnterWaterSound;

	[SerializeField] AudioSource _chestOpenSound;
	[SerializeField] AudioSource _chestCloseShutSound;
	[SerializeField] AudioSource _chestCloseSound;

	[SerializeField] AudioSource[] _waterTankDoorSounds;
	// 0: open, 1: close
	[SerializeField] AudioClip[] _waterTankAudioClips;
	int _whichTankSource = 0;

	[SerializeField] AudioSource _waterTankLidSound;
	// 0: open, 1: close
	[SerializeField] AudioClip[] _waterTankLidClips;

	[SerializeField] AudioSource _magicRevealSound;

	[SerializeField] AudioSource _theatreApplauseSound;

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

	public void PlayFrogLandingSOund(){
		_frogLandingSound.Play ();
	}

	public void PlayFrogPuddleSound(){
		_frogPuddleSound.Play ();
	}


	public void PlayCrowCawSound(){
		StartCoroutine (DelayedCawSound ());
	}

	IEnumerator DelayedCawSound(){
		yield return new WaitForSeconds (0.4f);
		if (!_crowCawSound.isPlaying) {
			_crowCawSound.Play ();
		}
	}

	public void PlayKissSound(){
		if (!_kissSound.isPlaying) {
			_kissSound.Play ();
		}
	}

	public void WaterTankMoveSound(){
		if (!_waterTankMoveSound.isPlaying) {
			_waterTankMoveSound.Play ();
		}
	}

	public void PlayDancerEnterWaterSound(){
		if (!_dancerEnterWaterSound.isPlaying) {
			_dancerEnterWaterSound.Play ();
		}
	}

	public void PlayChestOpenSound(){
		if (!_chestOpenSound.isPlaying) {
			_chestOpenSound.Play ();
		}
	}

	public void PlayChestCloseEndSound(){
		if (!_chestCloseShutSound.isPlaying) {
			_chestCloseShutSound.Play ();
		}
	}

	public void PlayChestCloseSound(){
		if (!_chestCloseSound.isPlaying) {
			_chestCloseSound.Play ();
		}
	}

	public void PlayApplauseSound(){
		if (!_theatreApplauseSound.isPlaying) {
			_theatreApplauseSound.Play ();
		}
	}

	public void PlayWaterTankSound (bool open, bool isLeftDoor){
		if (isLeftDoor == true) {
			_whichTankSource = 0;
		} else {
			_whichTankSource = 1;
		}

		_waterTankDoorSounds[_whichTankSource].Stop ();
		_waterTankDoorSounds [_whichTankSource].pitch = Random.Range (0.98f, 1.02f);
		if(open){
			_waterTankDoorSounds[_whichTankSource].clip = _waterTankAudioClips [0]; 
		} else {
			_waterTankDoorSounds[_whichTankSource].clip = _waterTankAudioClips [1];
		}
		_waterTankDoorSounds[_whichTankSource].Play ();
	}

	public void PlayWaterTankLidSound(bool open){
		_waterTankLidSound.Stop ();
		if (open) {
			_waterTankLidSound.clip = _waterTankLidClips [0];
		} else {
			_waterTankLidSound.clip = _waterTankLidClips [1];
		}
		_waterTankLidSound.Play ();
	}

	public void PlayMagicRevealSound(){
		if (!_magicRevealSound.isPlaying) {
			_magicRevealSound.Play ();
		}
	}
}
