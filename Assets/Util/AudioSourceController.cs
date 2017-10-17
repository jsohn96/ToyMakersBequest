using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour {



	[SerializeField] MinMax _pitchRange;
	[SerializeField] AudioSource _audioSource;
	float duration = 0.2f;

	float _thisVolume = 1.0f;
	// Use this for initialization
	void Start () {
		_thisVolume = _audioSource.volume;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.E)) {
			duration = 5f;
			//c
			//May need to add maximum volume
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			AudioManager.instance.Stop (_audioSource,_thisVolume,duration);
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			AudioManager.instance.Pause (_audioSource,_thisVolume,duration);
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			AudioManager.instance.Play (_audioSource,_thisVolume,duration);
		}

		if (Input.GetKeyDown (KeyCode.U)) {
			AudioManager.instance.Resume (_audioSource,_thisVolume,duration);
		}
	}

	void RandomizePitchFromRange(float max, float min) {
		
	}

	void PlayAudio(){
	}

	void PauseAudio(){
	}

	void StopAudio(){
	}

}
