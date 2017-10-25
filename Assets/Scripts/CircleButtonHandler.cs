using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleButtonHandler : MonoBehaviour {

	[SerializeField] AudioClip[] _crankClips;
	[SerializeField] AudioSource _audioSource;

	public void CallRed(){
		Events.G.Raise(new CircleTurnButtonPressEvent(ButtonColor.Red));
	}

	public void CallYellow(){
		Events.G.Raise(new CircleTurnButtonPressEvent(ButtonColor.Yellow));
	}

	public void CallBrown(){
		Events.G.Raise(new CircleTurnButtonPressEvent(ButtonColor.Brown));
	}

	public void PlayCrankSound(){
		if (!_audioSource.isPlaying) {
			int index = Random.Range (0, 4);
			_audioSource.clip = _crankClips [index];
			_audioSource.Play ();
		}
	}

	void CircleTurned(MBTurnColorCircle e){
		PlayCrankSound ();
	}

	void OnEnable(){
		Events.G.AddListener<MBTurnColorCircle> (CircleTurned);
	}

	void OnDisable(){
		Events.G.RemoveListener<MBTurnColorCircle> (CircleTurned);
	}
}
