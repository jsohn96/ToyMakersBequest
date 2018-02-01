using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRoomAudio : AudioSourceController {
	[SerializeField] AudioSource _peepZoomSource;
	[SerializeField] AudioSource _dogThudAudio;
	[SerializeField] AudioSource _dogWhineAudio;

	public void PlayZoomAudio(){
		_peepZoomSource.Play ();
	}

	public void PlayDogThud(){
		_dogThudAudio.Play ();
	}

	public void PlayDogWhine(){
		_dogWhineAudio.Play ();
	}
}
