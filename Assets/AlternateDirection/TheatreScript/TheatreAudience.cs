using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreAudience : MonoBehaviour {
	Quaternion _originAngle;
	Quaternion _goalAngle;

	bool _isEntered = false;

	[SerializeField] TheatreSound _theatreSound;
	[SerializeField] AltTheatre _myTheatre;
	[SerializeField] AudienceHeadFollow _audienceAnimation;

	void Start(){
//		_goalAngle = transform.rotation;
//		Vector3 tempAngle = _goalAngle.eulerAngles;
//		tempAngle.y += 180.0f;
//		_originAngle = Quaternion.Euler (tempAngle);
//		transform.rotation = _originAngle;
	}

	void OnTouchDown(){
		//Make the audience Caw
		_theatreSound.PlayCrowCawSound();
	}

	public void AudienceEnter(){
//		_isEntered = true;
//		StartCoroutine (TurnAround (true));
	}

	public void AudienceLeave(){
//		_isEntered = false;
//		StartCoroutine (TurnAround (false));
	}

	IEnumerator TurnAround(bool enter){
		float timer = 0f;
		float duration = 1.5f;
		if (enter) {
			while (timer < duration) {
				timer += Time.deltaTime;
				transform.rotation = Quaternion.Lerp (_originAngle, _goalAngle, timer / duration);
				yield return null;
			}
			transform.rotation = _goalAngle;
			Clap ();
		} else {
			Clap ();
			yield return new WaitForSeconds (0.3f);
			while (timer < duration) {
				timer += Time.deltaTime;
				transform.rotation = Quaternion.Lerp (_goalAngle, _originAngle, timer / duration);
				yield return null;
			}
			transform.rotation = _originAngle;
//			_myTheatre.MoveToNext ();
		}
		yield return null;
	}

	public void Clap(){
		_theatreSound.PlayCrowCawSound();
		_audienceAnimation.PlayClap ();
	}
}
