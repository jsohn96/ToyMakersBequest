using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreLetter : MonoBehaviour {
	
	bool _pickedUp = false;

	[SerializeField] Transform _theaterBackTransform;

	[Header("Close Up Values")]
	Vector3 _closeUpScale = new Vector3(1.32f, 1.32f, 1.32f);
	Vector3 _closeUpPos = new Vector3 (-0.008f, 0.078f, 0.077f);
	Vector3 _closeUpRot = new Vector3 (0f, 180f, -5f);

	[Header("Final Resting Location Values")]
	Vector3 _finalPosition = new Vector3 (0.127255f, 0.075102f, -0.02881f);
	Vector3 _finalRotation = new Vector3 (1.334f, 88.621f, 0.118f);


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTouchDown(Vector3 hit) {
		if (!_pickedUp) {
			_pickedUp = true;
			transform.parent = _theaterBackTransform;
			StartCoroutine (PickingUpLetter ());
		} else {
			//TODO: Handle other stuff here
		}
	}

	IEnumerator PickingUpLetter(){
		float timer = 0f;
		float duration = 3f;
		Vector3 originPos = transform.localPosition;
		Quaternion originRot = transform.localRotation;
		Vector3 originScale = transform.localScale;
		float mapValue;
		while (duration > timer) {
			timer += Time.deltaTime;
			mapValue = timer / duration;
			transform.localScale = Vector3.Lerp (originScale, _closeUpScale, mapValue);
			transform.localRotation = Quaternion.Lerp (originRot, Quaternion.Euler(_closeUpRot), mapValue);
			transform.localPosition = Vector3.Slerp (originPos, _closeUpPos, mapValue);
			yield return null;
		}
		transform.localScale = _closeUpScale;
		transform.localRotation = Quaternion.Euler (_closeUpRot);
		transform.localPosition = _closeUpPos;
		yield return null;
	}
}
