using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreBack : MonoBehaviour {

	[SerializeField] TheatreCameraControl _theatreCameraControl;
	[SerializeField] TheatreRotation _theatreRotation;
	BoxCollider _thisBoxCollider;

	[SerializeField] Transform _backCover;


	[SerializeField] Animator _backAnimator;

	[SerializeField] TheatreBackSlider _theatreBackSlider;

	Vector3 _closedBackDoor = new Vector3(0f,0f,0f);
	Vector3 _openBackDoor = new Vector3(0f, -150f, 0f);

	void Start(){
		_thisBoxCollider = GetComponent<BoxCollider> ();
	
		_backCover.localRotation = Quaternion.Euler (_closedBackDoor);
	}


	void OnTouchDown(){
		//rotateTheater to face center
		// move camera to zoom in
		_thisBoxCollider.enabled = false;
		_theatreCameraControl.ZoomBack (true);
		_theatreRotation.StartBackRotation ();
		StartCoroutine (OpenBackDoor ());
	}

	IEnumerator OpenBackDoor(){
		float timer = 0f;
		float duration = 3f;
		Quaternion closedBackDoorQuat = Quaternion.Euler (_closedBackDoor);
		Quaternion openBackDoorQuat = Quaternion.Euler (_openBackDoor);
		while (timer < duration) {
			timer += Time.deltaTime;
			_backCover.localRotation = Quaternion.Lerp (closedBackDoorQuat, openBackDoorQuat, timer / duration);
			yield return null;
		}
		_backCover.localRotation = openBackDoorQuat;
		yield return null;
	}


	public void TickTrueEnding(bool isTrueEnding){
		if (isTrueEnding) {
			_backAnimator.Play ("upper_gear_turn");
			_theatreBackSlider.Activate ();
		} else {
			_backAnimator.Play ("lower_gear_stuck");
		}
	}

	public void ResumeSequence(){
		_theatreCameraControl.ZoomBack (false);
		_theatreRotation.StartInitRotation ();
	}
}
