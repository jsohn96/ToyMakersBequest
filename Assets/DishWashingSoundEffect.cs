using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishWashingSoundEffect : AudioSourceController {
	[SerializeField] AudioSystem _dishWashingAudioSystem;
	[SerializeField] AudioSystem _dishCrackAudioSystem;
	bool _stopCrankSound = false;

	void OnEnable(){
		Events.G.AddListener<DragRotationEvent> (DragRotationHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<DragRotationEvent> (DragRotationHandle);
	}

	void DragRotationHandle(DragRotationEvent e) {
//		if (e.isRoating && !_stopCrankSound) {
//			if (e.isDesiredDirection) {
//				if (!_dishWashingAudioSystem.audioSource.isPlaying) {
//					Play (_dishWashingAudioSystem);
//				}
//			} else {
//				SwapClip (_dishCrackAudioSystem, true);
//			}
//		}
	}

	public void StopCrankSound(bool stop){
		_stopCrankSound = stop;
	}

	public void PlayShatterPlateSound(){
		SwapClip (_dishCrackAudioSystem, true);
	}
}
