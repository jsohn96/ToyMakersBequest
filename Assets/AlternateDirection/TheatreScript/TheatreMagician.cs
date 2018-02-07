using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreMagician : MonoBehaviour {
	[SerializeField] Transform _magicianTransform;
	Animator _magicianAnim;
	AltTheatre _myTheatre;
	Vector3 _finalPosition;
	[SerializeField] Vector3 _startPosition;
	[SerializeField] Transform _startPlatform;

	//Vector3 _endPosition;
	bool _isMoving;
	bool _isWaitingForLeft;

	// Use this for initialization
	void Awake () {
		_isMoving = false;
		_isWaitingForLeft = false;
		//_finalPosition = _magicianTransform.localPosition;
		_myTheatre = FindObjectOfType<AltTheatre> ().GetComponent<AltTheatre> ();
		//_magicianTransform = gameObject.transform;
	}

	void Start(){
		if (AltTheatre.currentSate == TheatreState.waitingToStart) {
			_magicianTransform.position = _startPosition;
			_magicianTransform.parent = _startPlatform;
		}
	}

	// Update is called once per frame
	void Update () {
		if (_isMoving && Vector3.Distance (_magicianTransform.localPosition, _finalPosition) >= 0.2f) {
			_magicianTransform.localPosition = Vector3.Lerp (_magicianTransform.localPosition, _finalPosition, Time.deltaTime * 2f);
		} else {
			_isMoving = false;
			//_magicianTransform.localPosition = _finalPosition;
			if (_isWaitingForLeft) {
				// play left point animation
				_myTheatre.MoveToNext();
				_isWaitingForLeft = false;
			}
		}
	}

	public void GoToStart(){
//		_isWaitingForLeft = true;
//		_finalPosition = _magicianTransform.localPosition;
//		_finalPosition.z = -3f;
//		_isMoving = true;
	}
}
