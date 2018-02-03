using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlRoomManager : MonoBehaviour {
	
	[SerializeField] TextMeshPro _tmpInstruction;

	[SerializeField] ControlRoomAudio _controlRoomAudio;

	[SerializeField] Fading _fading;

	[SerializeField] Camera _mainCamera;
	[SerializeField] Camera _peepCamera;
	[SerializeField] Camera _peepZoomCamera;
	[SerializeField] Canvas _mainCanvas;
	[SerializeField] Canvas _peepCanvas;

	[SerializeField] Vector3 _defaultCameraPosition;

	[SerializeField] AltPeepText _peepText;

	[SerializeField] GameObject[] _peepCovers = new GameObject[4];

	[SerializeField] GameObject _dog;

	Vector3 _tempZoomedPosition;
	bool _zoomedIn = false;

	void Start(){
		_peepCamera.enabled = false;
		_peepZoomCamera.enabled = false;
		_peepCanvas.enabled = false;
		_mainCamera.enabled = true;
		_mainCanvas.enabled = true;
		_tempZoomedPosition = _defaultCameraPosition;
		PreInitializePeepholeCover ();
		Invoke ("CheckAndRemoveCover", 1f);

		if (AltCentralControl._currentState == AltStates.keyUnlock) {
			_dog.SetActive (true);
		}
	}

	void PreInitializePeepholeCover(){
		int tempCurrentState = (int)AltCentralControl._currentState;
		if (AltCentralControl._peepAnimated [0] == true) {
			_peepCovers [0].SetActive (false);
		} 
		if (AltCentralControl._peepAnimated [1] == true) {
			_peepCovers [1].SetActive (false);
		}
		if (AltCentralControl._peepAnimated [2] == true) {
			_peepCovers [2].SetActive (false);
		}
		if (AltCentralControl._peepAnimated [3] == true) {
			_peepCovers [3].SetActive (false);
		}
	}

	void CheckAndRemoveCover(){
		int tempCurrentState = (int)AltCentralControl._currentState;

		if (tempCurrentState >= 0 && AltCentralControl._peepAnimated [0] == false) {
			_peepCovers [0].SetActive (false);
			AltCentralControl._peepAnimated [0] = true;
		} else if (tempCurrentState >= 1 && AltCentralControl._peepholeViewed[0] == true && AltCentralControl._peepAnimated [1] == false) {
				_peepCovers [1].SetActive (false);
				AltCentralControl._peepAnimated [1] = true;
		} else if (tempCurrentState >= 2 && AltCentralControl._peepholeViewed[1] == true && AltCentralControl._peepAnimated [2] == false) {
			_peepCovers [2].SetActive (false);
			AltCentralControl._peepAnimated [2] = true;
		} else if (tempCurrentState >= 3 && AltCentralControl._peepholeViewed[2] == true && AltCentralControl._peepAnimated [3] == false) {
			_peepCovers [3].SetActive (false);
			AltCentralControl._peepAnimated [3] = true;
		}

		Events.G.Raise (new PeepHoleActivationCheck ());
	}

	public void LookIntoPeephole(int peepholeIndex, Vector3 zoomCameraPosition){
		_tmpInstruction.enabled = false;
		_controlRoomAudio.PlayZoomAudio ();
		_fading.BeginFade (1, 2f);
		_tempZoomedPosition = zoomCameraPosition;
		StartCoroutine (WaitForCameraSwap (2f, _tempZoomedPosition));
		_peepText.ChangeText (peepholeIndex);
		AltCentralControl._peepholeViewed [peepholeIndex] = true;
		if (peepholeIndex == 3) {
			StartCoroutine (DropDog ());
		}
	}

	public void ZoomOutOfPeephole(){
		if (_zoomedIn) {
			_zoomedIn = false;
			_controlRoomAudio.PlayZoomAudio ();
			_fading.BeginFade (1);
			StartCoroutine (WaitForCameraSwap (-1f, _tempZoomedPosition));
		}
	}

	IEnumerator WaitForCameraSwap(float duration, Vector3 zoomCameraPosition){
		float timer = 0.0f;

		if (duration > 0f) {
			while (duration > timer) {
				timer += Time.deltaTime;
				_mainCamera.transform.position = Vector3.Slerp (_defaultCameraPosition, zoomCameraPosition, timer / duration);
				yield return null;
			}
			_mainCamera.transform.position = zoomCameraPosition;
			_mainCamera.enabled = false;
			_mainCanvas.enabled = false;
			_peepCamera.enabled = true;
			_peepZoomCamera.enabled = true;
			_peepCanvas.enabled = true;
			yield return null;
			_fading.BeginFade (-1);
			_zoomedIn = true;
		} else {
			yield return new WaitForSeconds (1f);
			_fading.BeginFade (-1);
			_mainCamera.enabled = true;
			_mainCanvas.enabled = true;
			_peepCamera.enabled = false;
			_peepZoomCamera.enabled = false;
			_peepCanvas.enabled = false;
			while (-duration > timer) {
				timer += Time.deltaTime;
				_mainCamera.transform.position = Vector3.Slerp (zoomCameraPosition, _defaultCameraPosition, timer / -duration);
				yield return null;
			}
			_mainCamera.transform.position = _defaultCameraPosition;
			yield return new WaitForSeconds(1f);
			CheckAndRemoveCover ();
		}
	}


	IEnumerator DropDog(){
		yield return new WaitForSeconds (5.0f);
		if (!AltCentralControl._dogDropped) {
			AltCentralControl._dogDropped = true;
			_controlRoomAudio.PlayDogThud ();
			_dog.SetActive (true);
		}
	}
}
