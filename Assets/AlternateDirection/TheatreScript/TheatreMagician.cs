﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreMagician : MonoBehaviour {
	[SerializeField] Transform _magicianTransform;
	[SerializeField] Animator _magicianAnim;
	AltTheatre _myTheatre;
	[SerializeField] Vector3 _startPosition;
	[SerializeField] Transform _startPlatform;
	[SerializeField] Transform _waterTank;
	[SerializeField] Vector3 _onWaterTank;
	[SerializeField] Vector3 _stepOffWaterTank;

	Vector3 _tempPos;

	//Vector3 _endPosition;
	bool _isMoving;
	bool _isWaitingForLeft;

	// Use this for initialization
	void Awake () {
		_isMoving = false;
		_isWaitingForLeft = false;
		_myTheatre = FindObjectOfType<AltTheatre> ().GetComponent<AltTheatre> ();

		//_magicianTransform = gameObject.transform;
	}

	void Start(){
		if (AltTheatre.currentSate == TheatreState.waitingToStart) {
			_magicianTransform.position = _startPosition;
			_magicianTransform.parent = _startPlatform;
		}
	}

		
	public void StepOnTank(){
		_magicianTransform.parent = _waterTank;
		StartCoroutine (MoveMagician (_magicianTransform.position, _onWaterTank, 2f));

	}

	public void StepOffTank(){
		StartCoroutine (MoveMagician (_magicianTransform.position, _stepOffWaterTank, 3f));
		//PointToCenter (false);
	}

	IEnumerator MoveMagician(Vector3 start, Vector3 end, float duration){
		float timer = 0f;
		while (timer < duration) {
			timer += Time.deltaTime;
			_magicianTransform.position = Vector3.Slerp (start, end, timer / duration);
			yield return null;
		}
		_magicianTransform.position = end;
		yield return null;
		_myTheatre.MoveToNext ();
	}

	public void GoToStart(){
//		_isWaitingForLeft = true;
//		_finalPosition = _magicianTransform.localPosition;
//		_finalPosition.z = -3f;
//		_isMoving = true;
	}

	public void PointToCenter(bool isPointing){
		if (isPointing) {
			_magicianAnim.Play ("mg_greet");
		} else {
			_magicianAnim.Play ("mg_pointCenter_back_center");
		}
	}

	public void PointToLeft(bool isPointing){
		if (isPointing) {
			_magicianAnim.Play ("mg_pointLeft");
		} else {
			_magicianAnim.Play ("mg_pointLeft_to_center");
		}
	}

	public void PointToRight(bool isPointing){
		if (isPointing) {
			_magicianAnim.Play ("mg_pointRight");
		} else {
			_magicianAnim.Play ("mg_pointRight_to_center");
		}
	}




}
