using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopForwardMovementPeep : MonoBehaviour {
	[SerializeField] PeepIn _peepInScript;
	AudioSource _audioSource;
	[SerializeField] AudioClip _audioClip;

	[SerializeField] PeepholeWalk _peepHoleWalkScript;
	bool _triggerOnce = false;
	bool _noMoreFunctioning = false;

	void Start(){
		_audioSource = GetComponent<AudioSource> ();
	
	}


	void Update(){
		if (_peepInScript._isPeepingIn) {
			if (!_audioSource.isPlaying) {
				_audioSource.Play ();
			}
		} else {
			_audioSource.Stop ();
		}
	}

	public void AllowPassage(){
		_peepHoleWalkScript.StopMoveForward (false);
		_noMoreFunctioning = true;
	}

	void OnTriggerEnter(Collider other){
		if (!_noMoreFunctioning) {
			if (other.tag == "MainCamera") {
				_peepHoleWalkScript.StopMoveForward (true);
				if (!_triggerOnce) {
					Events.G.Raise (new GearsReadyForPickupEvent ());
					_audioSource.Stop ();
					_audioSource.clip = _audioClip;
					_audioSource.Play ();
					_triggerOnce = true;
				}
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "MainCamera") {
			_peepHoleWalkScript.StopMoveForward (false);
		}
	}
}
