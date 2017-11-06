using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundController : AudioSourceController {
	[SerializeField] AudioSystem _audioSystem1;
	[SerializeField] AudioSystem _audioSystem2;

	//Use AudioSystem1, 0: RoomTone, 1: FrogTone, 2: SquirrelTone

	private bool _audioSystem1IsPlaying = true;

	// Use this for initialization
	void Start () {
		_audioSystem1.audioSource.clip = _audioSystem1.clips [0];
		_audioSystem1.audioSource.loop = true;

		_audioSystem2.audioSource.clip = _audioSystem1.clips [1];
		_audioSystem2.audioSource.loop = true;

		Play (_audioSystem1);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
			SwitchToFrog ();
	}

	void SwitchToFrog(){
		if (_audioSystem1IsPlaying) {
			AudioManager.instance.CrossFade (_audioSystem1.audioSource, _audioSystem2.audioSource);
			_audioSystem1IsPlaying = false;
			Debug.Log ("1 to 2");
		}

		else if (!_audioSystem1IsPlaying) {
			AudioManager.instance.CrossFade (_audioSystem2.audioSource, _audioSystem1.audioSource);
			_audioSystem1IsPlaying = true;
			Debug.Log ("2 to 1");
		}
	}
}
