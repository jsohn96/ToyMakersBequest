using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreMainStageElevation : MonoBehaviour {

	[SerializeField] Transform[] _toBeChildTransforms;
	[SerializeField] float _localGoalY = 0.1071f;
	Vector3 _goalElevation;

	void Start () {
		_goalElevation = transform.localPosition;
		_goalElevation.y = _localGoalY;
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
		float timer = 0f;
		Vector3 originLocalPosition = transform.localPosition;
		while (duration > timer) {
			timer += Time.deltaTime;
			transform.localPosition = Vector3.Slerp (originLocalPosition, _goalElevation, timer / duration);
			yield return null;
		}
		transform.localPosition = _goalElevation;
		yield return null;
	}
}
