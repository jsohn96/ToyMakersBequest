using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DogManager : MonoBehaviour {
	[SerializeField] ControlRoomAudio _controlRoomAudio;

	void OnTouchDown(){
		_controlRoomAudio.PlayDogWhine ();
		if (AltCentralControl._currentState == AltStates.allCharm) {
			StartCoroutine (ChangeLevel ());
		}
	}

	IEnumerator ChangeLevel(){
		yield return new WaitForSeconds (2f);
		float fadeTime = GameObject.Find ("Fade").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene ("InitialCredits");
	}
}
