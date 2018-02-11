﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreDancer : MonoBehaviour {
	AltTheatre _myTheatre;
	[SerializeField] Transform _dancerTransform;

	[SerializeField] Vector3 _startPosition;
	Vector3 _stillRotateAxis = new Vector3 (0f,1f,0f);
	[SerializeField] Transform _waterTankTransformForCenterAxis;

	[Header("First Water Tank Descend Values")]
	[SerializeField] Transform _waterTankPlatformTransform;
	[SerializeField] Vector3 _firstWaterTankStart;
	[SerializeField] Vector3 _firstWaterTankEnd;

	Vector3 _waterTankPlatformUpLocalPos = new Vector3 (0.00019f, 0.03096f, 0.00026f);
	Vector3 _waterTankPlatformDownLocalPos = new Vector3 (0.00019f, -0.0254f, 0.00026f);

	Vector3 _dancerTempPos;

	[SerializeField] GameObject _dancerVisibilityGameObject;

	// Use this for initialization
	void Start () {
		_myTheatre = FindObjectOfType<AltTheatre> ().GetComponent<AltTheatre> ();
		if (AltTheatre.currentSate == TheatreState.waitingToStart) {
			_dancerTransform.position = _startPosition;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (AltTheatre.currentSate == TheatreState.startShow || AltTheatre.currentSate == TheatreState.readyForDancerTank) {
			RotateInPlace ();
			RotateAroundCenter ();
		} else if (AltTheatre.currentSate >= TheatreState.dancerInTank) {
			RotateInPlace ();
		}
	}

	public void FirstDancerEnterTank(){
		StartCoroutine (FirstDancerTankCoroutine ());
	}

	IEnumerator FirstDancerTankCoroutine(){
		float timer = 0f;
		float duration = 5f;
//		while (timer < duration) {
//			timer += Time.deltaTime;
//			RotateAroundCenter ();
//			yield return null;
//		}
		timer = 0f;
		duration = 1.5f;
		while (timer < duration) {
			timer += Time.deltaTime;
			yield return null;
		}
		timer = 0f;
		duration = 3f;
		_dancerTempPos = _dancerTransform.localPosition;
		while (timer < duration) {
			timer += Time.deltaTime;
			_dancerTransform.localPosition = Vector3.Lerp (_dancerTempPos, _firstWaterTankStart, timer / duration);
			yield return null;
		}

		_dancerTransform.localPosition = _firstWaterTankStart;
		_dancerTransform.parent = _waterTankPlatformTransform;
		yield return new WaitForSeconds (0.7f);
		timer = 0f;
		duration = 4f;
		while (timer < duration) {
			timer += Time.deltaTime;
			_waterTankPlatformTransform.localPosition = Vector3.Lerp (_waterTankPlatformUpLocalPos, _waterTankPlatformDownLocalPos, timer / duration);
			yield return null;
		}
		_waterTankPlatformTransform.localPosition = _waterTankPlatformDownLocalPos;

		_myTheatre.MoveToNext ();
		yield return null;
	}

	void RotateInPlace(){
		_dancerTransform.Rotate (_stillRotateAxis * 53f * Time.deltaTime);
	}

	void RotateAroundCenter(){
		_dancerTransform.RotateAround (_waterTankTransformForCenterAxis.position, _stillRotateAxis, 20f * Time.deltaTime);
	}

	public void HideDancer(bool hide){
		_dancerVisibilityGameObject.SetActive (!hide);
	}
}
