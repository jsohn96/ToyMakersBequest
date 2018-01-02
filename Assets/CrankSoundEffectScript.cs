using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankSoundEffectScript : AudioSourceController {
	[SerializeField] AudioSystem _crankTickAudioSystem;
	[SerializeField] AudioSystem _whirAudioSystem;
	bool _stopCrankSound = false;

	void OnEnable(){
		Events.G.AddListener<DragRotationEvent> (DragRotationHandle);
	}

		void OnDisable(){
			Events.G.RemoveListener<DragRotationEvent> (DragRotationHandle);
		}

	void DragRotationHandle(DragRotationEvent e) {
		if (e.isRoating && !_stopCrankSound) {
			if (e.isDesiredDirection) {
				SwapClip (_crankTickAudioSystem, true);
			} else {
				SwapClip (_whirAudioSystem, true);
			}
		}
	}

	public void StopCrankSound(bool stop){
		_stopCrankSound = stop;
	}
}
