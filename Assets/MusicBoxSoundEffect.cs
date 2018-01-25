using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxSoundEffect : AudioSourceController {
	[SerializeField] AudioSystem _crankTickAudioSystem;
	[SerializeField] AudioSystem _connectedAudioSystem;
	[SerializeField] AudioSystem _frogAudioSystem;
	int curRotateNode = -1;
	bool _stopForSceneTransition = false;
	bool _isInitialized = false;

	void OnEnable(){
		Events.G.AddListener<MBNodeRotate> (NodeRotateHandle);
		Events.G.AddListener<DragRotationEvent> (DragRotationHandle);
		Events.G.AddListener<PathConnectedEvent> (PathConnectedHandler);
		Events.G.AddListener<DancerChangeMoveEvent> (Initialize);
		Events.G.AddListener<FrogIsOnTheMoveEvent> (PlayFrogCroakSound);
		Events.G.AddListener<PathNodeStuckEvent> (PlayNodeStuckSound);
	}

	void OnDisable(){
		Events.G.RemoveListener<MBNodeRotate> (NodeRotateHandle);
		Events.G.RemoveListener<DragRotationEvent> (DragRotationHandle);
		Events.G.RemoveListener<PathConnectedEvent> (PathConnectedHandler);
		Events.G.RemoveListener<DancerChangeMoveEvent> (Initialize);
		Events.G.RemoveListener<PathNodeStuckEvent> (PlayNodeStuckSound);
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

	void PlayFrogCroakSound(FrogIsOnTheMoveEvent e){
		AudioManager.instance.RandomizePitchFromRange (_frogAudioSystem);
		StartCoroutine (DelayAudioPlay (_frogAudioSystem.audioSource, 1.5f));
	}

	IEnumerator DelayAudioPlay(AudioSource audioSource, float duration){
		yield return new WaitForSeconds (duration);
		audioSource.Play ();
	}

	void Initialize(DancerChangeMoveEvent e) {
		if (e.Move == DancerMove.none) {
			_isInitialized = true;
		}
	}



	public void StopCrankSound(){
		_stopForSceneTransition = true;
	}

	void PlayNodeStuckSound(){
		
	}
}
