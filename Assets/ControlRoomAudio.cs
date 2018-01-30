using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRoomAudio : AudioSourceController {
	[SerializeField] AudioSource _peepZoomSource;

	public void PlayZoomAudio(){
		_peepZoomSource.Play ();
	}
}
