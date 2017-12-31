using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxSoundEffect : AudioSourceController {
	[SerializeField] AudioSystem _crankTickAudioSystem;
	[SerializeField] AudioSystem _connectedAudioSystem;
	int curRotateNode = -1;
	bool _stopForSceneTransition = false;
	bool _isInitialized = false;

	void OnEnable(){
		Events.G.AddListener<MBNodeRotate> (NodeRotateHandle);
		Events.G.AddListener<DragRotationEvent> (DragRotationHandle);
		Events.G.AddListener<PathConnectedEvent> (PathConnectedHandler);
		Events.G.AddListener<DancerChangeMoveEvent> (Initialize);
	}

	void OnDisable(){
		Events.G.RemoveListener<MBNodeRotate> (NodeRotateHandle);
		Events.G.RemoveListener<DragRotationEvent> (DragRotationHandle);
		Events.G.RemoveListener<PathConnectedEvent> (PathConnectedHandler);
		Events.G.RemoveListener<DancerChangeMoveEvent> (Initialize);
	}

	void DragRotationHandle(DragRotationEvent e) {
		if (e.isRoating) {
			SwapClip (_crankTickAudioSystem, true);
		}
	}

	void NodeRotateHandle(MBNodeRotate e){
		if (e.isRoating && !_stopForSceneTransition) {
			SwapClip (_crankTickAudioSystem, true);
			curRotateNode = e.nodeIndex;
		} else if(!e.isRoating && curRotateNode == e.nodeIndex){
			curRotateNode = -1;
		}
	}

	void PathConnectedHandler(PathConnectedEvent e){
		if (_isInitialized) {
			AudioManager.instance.RandomizePitchFromRange (_connectedAudioSystem);
			_connectedAudioSystem.audioSource.Play ();
		}
	}

	void Initialize(DancerChangeMoveEvent e) {
		if (e.Move == DancerMove.none) {
			_isInitialized = true;
		}
	}

	public void StopCrankSound(){
		_stopForSceneTransition = true;
	}
}
