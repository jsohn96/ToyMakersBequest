using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreAudience : MonoBehaviour {
	Quaternion _originAngle;
	Quaternion _goalAngle;

	bool _isEntered = false;

	[SerializeField] TheatreSound _theatreSound;

	void Start(){
		_goalAngle = transform.rotation;
		Vector3 tempAngle = _goalAngle.eulerAngles;
		tempAngle.y += 180.0f;
		_originAngle = Quaternion.Euler (tempAngle);
		transform.rotation = _originAngle;
	}

	void OnTouchDown(){
		//Make the audience Caw
		_theatreSound.PlayCrowCawSound();
	}

	public void AudienceEnter(){
		_isEntered = true;
		StartCoroutine (TurnAround ());
	}

	IEnumerator TurnAround(){
		float timer = 0f;
		float duration = 1.5f;
		while (timer < duration) {
			timer += Time.deltaTime;
			transform.rotation = Quaternion.Lerp (_originAngle, _goalAngle, timer / duration);
			yield return null;
		}
		transform.rotation = _goalAngle;
		Clap ();
		yield return null;
	}

	public void Clap(){
		_theatreSound.PlayCrowCawSound();
	}
}
