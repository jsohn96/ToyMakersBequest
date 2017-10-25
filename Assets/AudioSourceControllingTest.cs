using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class AudioSourceControllingTest : AudioSourceController {
	[SerializeField] AudioSystem _audioSystem;
	bool _stop = false;
	bool _reduceStart = false;
	bool _waitForComeback = false;


	SimpleMusicPlayer _simpleMusicPlayer;

	// Use this for initialization
	void Start () {
		_audioSystem.audioSource = GetComponent<AudioSource> ();

		Koreographer.Instance.RegisterForEvents ("KoreographyTrackTest", TestFunction);
		Koreographer.Instance.RegisterForEvents ("KoreographyMelodyTrack", ResumeFunction);
		Koreographer.Instance.RegisterForEvents ("NewKoreographyTrackOff", TrackOff);

		_simpleMusicPlayer = GetComponent<SimpleMusicPlayer> ();
	}
	void TestFunction(KoreographyEvent koreoEvent) {
		if (_stop && !_reduceStart && !_waitForComeback) {
			_reduceStart = true;
			AdjustVolume (_audioSystem, 0.3f, 0.0f, 1.0f);
			Debug.Log ("volume change in");
		}
		Debug.Log ("test function called");
	}

	void ResumeFunction(KoreographyEvent koreoEvent) {
		if (!_stop && _waitForComeback) {
			_reduceStart = false;
			_waitForComeback = false;
			_audioSystem.audioSource.volume = 1.0f;
			_simpleMusicPlayer.Play ();

		}
		Debug.Log ("resume function called");
	}

	void TrackOff(KoreographyEvent koreoEvent){
		Debug.Log (koreoEvent.GetIntValue ());
		int koreoInt = koreoEvent.GetIntValue ();

		if (_stop && _reduceStart && !_waitForComeback) {
			_simpleMusicPlayer.Pause ();
			Debug.Log ("off in");
		}
		Debug.Log ("trackoff function called");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			_audioSystem.audioSource.time += 5f;
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (!_stop) {
				_stop = true;
			} else {
				_stop = false;
				_waitForComeback = true;
			}
		}
	}
}
