using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreMainStageElevation : MonoBehaviour {
	[SerializeField] TheatrePulley[] _pulleys;
	[SerializeField] Transform _danceInCircleParent;
	[SerializeField] Transform _dancerTrans;
	[SerializeField] Transform _magicianTrans;
	[SerializeField] Transform[] _toBeChildTransforms;
	[SerializeField] float _localGoalY = 0.1071f;
	Vector3 _goalElevation;
	bool _isCircling = false;

	void Start () {
		_goalElevation = transform.localPosition;
		_goalElevation.y = _localGoalY;
	}

	void Update(){
		if (_isCircling) {
			_danceInCircleParent.RotateAround (_danceInCircleParent.position, _danceInCircleParent.up, 20f * Time.deltaTime);
		}

//		if (Input.GetKey (KeyCode.A)) {
//			DanceInCircle ();
//		}
	}

	public void BringEveryoneUnderWing(){
		int length = _toBeChildTransforms.Length;
		for (int i = 0; i < length; i++) {
			_toBeChildTransforms [i].SetParent (transform);
		}
	}

	public void BeginElevation(float duration){
		StartCoroutine (Elevate (duration));
	}

	IEnumerator Elevate(float duration){
		// start puley rotate 
		_pulleys[0].StartRotate();
		_pulleys[1].StartRotate();
		float timer = 0f;
		Vector3 originLocalPosition = transform.localPosition;
		while (duration > timer) {
			timer += Time.deltaTime;
			transform.localPosition = Vector3.Slerp (originLocalPosition, _goalElevation, timer / duration);
			yield return null;
		}
		transform.localPosition = _goalElevation;
		yield return null;

		// stop pulley rotate
		_pulleys[0].StopRotate();
		_pulleys[1].StopRotate();
	}

	public void DanceInCircle(){
		_magicianTrans.parent = _danceInCircleParent;
		_dancerTrans.parent = _danceInCircleParent;
		_isCircling = true;
	}
}
