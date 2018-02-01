using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldLadyAudio : AudioSourceController {
	[SerializeField] AudioSystem _switchAudio;
	[SerializeField] AudioSystem _stepsAudio;

	public void PlayLightSwitchSound(){
		SwapClip (_switchAudio, true);
	}

	public void PlayStepSound(){
		SwapClip (_stepsAudio);
	}
}
