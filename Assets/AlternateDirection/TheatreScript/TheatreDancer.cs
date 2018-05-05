using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreDancer : MonoBehaviour {
	AltTheatre _myTheatre;
	[SerializeField] Animator _dancerAnim;
	[SerializeField] Transform _dancerTransform;
	[SerializeField] Transform _closetLocator;
	[SerializeField] Transform _centerLocator;

	[SerializeField] Vector3 _startPosition;
	Vector3 _stillRotateAxis = new Vector3 (0f,1f,0f);
	[SerializeField] Transform _waterTankTransformForCenterAxis;

	[Header("First Water Tank Descend Values")]
	[SerializeField] Transform _waterTankPlatformTransform;
	[SerializeField] Vector3 _firstWaterTankStart;
	[Header("Kissing Locato")]
	[SerializeField] Transform _kissLocator;


	Vector3 _waterTankPlatformUpLocalPos = new Vector3 (0f, 0.01476911f, -0.001706005f);
	Vector3 _waterTankPlatformDownLocalPos = new Vector3 (0f, -0.0102f, -0.001706005f);

	Vector3 _dancerTempPos;

	[SerializeField] GameObject _dancerVisibilityGameObject;

	bool _stopMovement = false;
	bool _isFirstStart = false;

	bool _startDancing = false;

	[SerializeField] TheatreSound _theatreSound;

	// Use this for initialization
	void Start () {
		_myTheatre = FindObjectOfType<AltTheatre> ().GetComponent<AltTheatre> ();
//		if (_startDancing) {
			if (AltTheatre.currentSate == TheatreState.waitingToStart) {
//			_dancerTransform.position = _startPosition;
				_dancerTransform.parent = _waterTankPlatformTransform;
			}
//		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!_stopMovement) {
			if (AltTheatre.currentSate == TheatreState.readyForDancerTank) {
				RotateInPlace ();
				RotateAroundCenter ();
				PlayStartDancing ();
			} else if (AltTheatre.currentSate >= TheatreState.dancerInTank) {
				RotateInPlace ();
			} 
		} else {
			if (AltTheatre.currentSate == TheatreState.readyForDancerTank2) {
				_stopMovement = false;
			}
		}
	}

//	public void StartDancing(){
//		_startDancing = true;
//	}

	void PlayEnterWater(){
		_dancerAnim.Play ("dancer_enter_water");
		_dancerAnim.SetBool("isReadyToEnter", false);
	}

	public void DancerEnterWater(){
		_dancerAnim.Play ("dancer_enter_water");
	}


	void PlayGoInTank(){
		_dancerAnim.SetBool("isReadyToEnter", true);
	}

	void PlayStartDancing(){
		if (!_isFirstStart) {
			_isFirstStart = true;
			_dancerAnim.Play ("dancer_start");
		}
	}



	public void Kiss(){
		_dancerAnim.Play ("Kiss");
	}

	public void EndKiss(){
		_dancerAnim.SetBool ("isBack", true);

	}

	public void DancerBackToCenterNextDay(){
		StartCoroutine (DancerToCenter ());
	}

	public void Drown(){
		_dancerAnim.speed = 1f;
		_dancerAnim.Play ("dancer_drown");
	}




	public void FirstDancerEnterTank(){
		StartCoroutine (FirstDancerTankCoroutine ());

	}

	public void SecondDancerEnterTank(){
		StartCoroutine (SecondDancerTankCoroutine ());

	}

	public void ElevateTankPlatform(float duration = 3f){
		StartCoroutine (ElevateWaterTankPlatform (duration));
	}

	public void StopMovement(bool drownPrep = false){
		if (drownPrep) {
			_dancerAnim.Play ("dancer_drown", 0, 0f);
			_dancerAnim.speed = 0f;
		}
		_stopMovement = true;
	}

	public void ExitCloset(){
		HideDancer (false);
		StopMovement ();
		_dancerTransform.parent = null;
		_dancerTransform.position = _closetLocator.position;
		_dancerTransform.localRotation = _closetLocator.localRotation;
		_dancerAnim.Play ("dancer_exit_closet");
	}

	IEnumerator DancerToCenter(){
		yield return new WaitForSeconds (1.5f);
//		_dancerTransform.parent = _waterTankPlatformTransform;
		float timer = 0f;
		float duration = 2f;
		Vector3 centerPos = _centerLocator.position;

		//duration = 3f;
		_dancerTempPos = _dancerTransform.position;
		centerPos.y = _dancerTempPos.y;
		while (timer < duration) {
			timer += Time.deltaTime;
			_dancerTransform.position = Vector3.Lerp (_dancerTempPos, centerPos, timer / duration);
			yield return null;
		}
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
		PlayGoInTank ();
		timer = 0f;
		duration = 3f;
		_dancerTempPos = _dancerTransform.localPosition;
		while (timer < duration) {
			timer += Time.deltaTime;
			_dancerTransform.localPosition = Vector3.Lerp (_dancerTempPos, _firstWaterTankStart, timer / duration);
			yield return null;
		}

		_dancerTransform.localPosition = _firstWaterTankStart;


		yield return new WaitForSeconds (0.7f);
		timer = 0f;
		duration = 4f;
		bool playSoundOnce = false;

		while (timer < duration) {
			timer += Time.deltaTime;

			if (timer > 0.5f) {
				playSoundOnce = true;
				_theatreSound.PlayDancerEnterWaterSound ();
			}

			_waterTankPlatformTransform.localPosition = Vector3.Lerp (_waterTankPlatformUpLocalPos, _waterTankPlatformDownLocalPos, timer / duration);
			yield return null;
		}
		_waterTankPlatformTransform.localPosition = _waterTankPlatformDownLocalPos;

		yield return new WaitForSeconds (0.7f);
		PlayEnterWater ();

		_myTheatre.MoveToNext ();
		yield return new WaitForSeconds (1f);
		PlayEnterWater ();
	}

	IEnumerator ElevateWaterTankPlatform(float duration){
		float timer = 0f;

		while (timer < duration) {
			timer += Time.deltaTime;
			_waterTankPlatformTransform.localPosition = Vector3.Lerp (_waterTankPlatformDownLocalPos, _waterTankPlatformUpLocalPos, timer / duration);
			yield return null;
		}
		_waterTankPlatformTransform.localPosition = _waterTankPlatformUpLocalPos;
		yield return null;
	}

	IEnumerator SecondDancerTankCoroutine(){
		yield return new WaitForSeconds (1.5f);
		_dancerTransform.parent = _waterTankPlatformTransform;
		float timer = 0f;
		float duration = 4f;
		bool playSoundOnce = false;

		while (timer < duration) {
			timer += Time.deltaTime;
			if (timer > 0.5f) {
				playSoundOnce = true;
				_theatreSound.PlayDancerEnterWaterSound ();
			}

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
