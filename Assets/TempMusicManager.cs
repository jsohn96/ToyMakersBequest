using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMusicManager : MonoBehaviour {

	AudioSource _myAudio;

	void OnEnable(){
		Events.G.AddListener<MBMusicMangerEvent> (MusicPlayHandle);

	}

	void OnDisable(){
		Events.G.RemoveListener<MBMusicMangerEvent> (MusicPlayHandle);


	}

	// Use this for initialization
	void Awake () {
		_myAudio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void MusicPlayHandle(MBMusicMangerEvent e){
		print ("MB MAnager: " + e.isMusicPlaying);
		if (e.isMusicPlaying) {
			if (!_myAudio.isPlaying) {
				_myAudio.Play ();
			} 
		}else {
			if (_myAudio.isPlaying) {
				_myAudio.Pause ();
			}

		}
	}
}
