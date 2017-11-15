using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxSoundEffect : AudioSourceController {
	[SerializeField] AudioSource _audio;
	AudioSource[] _audioPool;
	[SerializeField] AudioClip[] _clips;
	int curRotateNode = -1;

	void OnEnable(){
		Events.G.AddListener<MBNodeRotate> (NodeRotateHandle);

	}

	void OnDisable(){
		Events.G.RemoveListener<MBNodeRotate> (NodeRotateHandle);

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void NodeRotateHandle(MBNodeRotate e){
		if (e.isRoating && curRotateNode != e.nodeIndex) {
			PlayDraggingSound ();
			curRotateNode = e.nodeIndex;
		} else if(!e.isRoating && curRotateNode == e.nodeIndex){
			StopDraggingSound ();
			curRotateNode = -1;

		}
	}

	void PlayDraggingSound(){
		if (_audio != _clips [0]) {
			_audio.clip = _clips [0];
			_audio.Play ();
		} 

	}

	void StopDraggingSound(){
		_audio.Stop();
	}
}
