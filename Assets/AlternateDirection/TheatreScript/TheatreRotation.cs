using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreRotation : MonoBehaviour {

	[SerializeField] Vector3 _initRotation;
	[SerializeField] Vector3 _startRotation;
	[SerializeField] AltTheatre _myTheatre;
	Vector3 _rotateAxis;
	bool _rotateRight = false;
	bool _rotateLeft = false;

	[SerializeField] TheatreSound _theatreSound;

	void Start () {
		transform.rotation = Quaternion.Euler (_initRotation);
		_rotateAxis = transform.up;
	}

	void FixedUpdate(){
//		if(Input.GetKey(KeyCode.LeftArrow)){
//			transform.Rotate (_rotateAxis, 1f);
//		}
//
//		if(Input.GetKey(KeyCode.RightArrow)){
//			transform.Rotate (_rotateAxis, -1f);
//		}

		if (_rotateRight) {
			transform.Rotate (_rotateAxis, -1f);
		} else if (_rotateLeft) {
			transform.Rotate (_rotateAxis, 1f);
		}
	}
	

	public void StartInitRotation(){
		StartCoroutine (InitRotate ());
	}

	public void StartBackRotation(){
		StartCoroutine (BackRotate ());
	}

	public void StartResumeRotation(){
		StartCoroutine (ResumeRotate ());
	}

	IEnumerator InitRotate(){
//		yield return new WaitForSeconds (1f);
		_theatreSound.PlayTheatreRotateSound (true);
		float duration = 6f;
		float timer = 0f;
		Quaternion initRot = Quaternion.Euler (_initRotation);
		Quaternion startRot = Quaternion.Euler (_startRotation);
		while (duration > timer) {
			timer += Time.deltaTime;
			transform.rotation = Quaternion.Lerp (initRot, startRot, timer/duration);
			yield return null;
		}
		transform.rotation = startRot;
		_theatreSound.PlayTheatreRotateSound (false);
		yield return null;
	}

	IEnumerator BackRotate(){
		_theatreSound.PlayTheatreRotateSound (true);
		float duration = 3f;
		float timer = 0f;
		Quaternion initRot = transform.rotation;
		Quaternion backRot = Quaternion.Euler (Vector3.zero);
		while (duration > timer) {
			timer += Time.deltaTime;
			transform.rotation = Quaternion.Lerp (initRot, backRot, timer/duration);
			yield return null;
		}
		transform.rotation = backRot;
		_theatreSound.PlayTheatreRotateSound (false);
		yield return null;
	}

	IEnumerator ResumeRotate(){
		yield return new WaitForSeconds (4f);
		_theatreSound.PlayTheatreRotateSound (true);
		float duration = 4f;
		float timer = 0f;
		Quaternion initRot = transform.rotation;
		Quaternion startRot = Quaternion.Euler (_startRotation);
		while (duration > timer) {
			timer += Time.deltaTime;
			transform.rotation = Quaternion.Lerp (initRot, startRot, timer/duration);
			yield return null;
		}
		transform.rotation = startRot;
		_myTheatre.MoveToNext ();
		_theatreSound.PlayTheatreRotateSound (false);
		yield return null;
	}


	public void OnPointerUp(Direction whichDirection){
		_theatreSound.PlayTheatreRotateSound (false);
		switch (whichDirection) {
		case Direction.right:
			_rotateRight = false;
			break;
		case Direction.left:
			_rotateLeft = false;
			break;
		default:
			break;
		}
	}

	public void OnPointerDown(Direction whichDirection){
		_theatreSound.PlayTheatreRotateSound (true);
		switch (whichDirection) {
		case Direction.right:
			_rotateRight = true;
			_rotateLeft = false;
			break;
		case Direction.left:
			_rotateRight = false;
			_rotateLeft = true;
			break;
		default:
			break;
		}
	}


//		switch (whichDirection) {
//		case Direction.up:
//			_acceleration = 0.015f;
//			_scrollDirectionMultiplier.y = 10f;
//			_angleMode = false;
//			if(!_down){
//				_down = true;
//				_traversalUI.FadeIn(false, 1);
//			}
//			break;
//		case Direction.down:
//			_acceleration = 0.015f;
//			_scrollDirectionMultiplier.y = -10f;
//			_angleMode = false;
//			if(!_up){
//				_up = true;
//				_traversalUI.FadeIn(false, 0);
//			}
//			break;
//		case Direction.left:
//			Debug.Log("left is being called th");
//			_acceleration = 10f;
//			_scrollDirectionMultiplier.y = -1f;
//			_angleMode = true;
//			if(!_right){
//				_right = true;
//				_traversalUI.FadeIn(false, 3);
//			}
//			break;
//		case Direction.right:
//			_acceleration = 10f;
//			_scrollDirectionMultiplier.y = 1f;
//			_angleMode = true;
//			if(!_left){
//				_left = true;
//				_traversalUI.FadeIn(false, 2);
//			}
//			break;
//		default:
//			break;
//		}
}
