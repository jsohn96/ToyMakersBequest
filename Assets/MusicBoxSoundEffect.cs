﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxSoundEffect : AudioSourceController {
	[SerializeField] AudioSystem _crankTickAudioSystem;
	int curRotateNode = -1;

	void OnEnable(){
		Events.G.AddListener<MBNodeRotate> (NodeRotateHandle);

	}

	void OnDisable(){
		Events.G.RemoveListener<MBNodeRotate> (NodeRotateHandle);

	}

	void NodeRotateHandle(MBNodeRotate e){
		if (e.isRoating) {
			SwapClip (_crankTickAudioSystem, true);
			curRotateNode = e.nodeIndex;
		} else if(!e.isRoating && curRotateNode == e.nodeIndex){
			curRotateNode = -1;
		}
	}
}
