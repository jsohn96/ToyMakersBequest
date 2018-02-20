using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlRoom : MonoBehaviour {

	[SerializeField] ControlRoomAudio _controlRoomAudio;
	[SerializeField] Fading _fading;


	[SerializeField] Camera _mainCamera;
	[SerializeField] Vector3 _defaultCameraPosition;

	static bool _zoomedIn = false;
	static Vector3 _lastZoomPosition;
	bool _preventNewInput = false;


	[SerializeField] GameObject[] _peepCovers = new GameObject[4];

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		if (_zoomedIn) {
			ZoomOutOfPeephole ();
		}
	}

	void DisableSceneInput(DisableSceneTransitionInput e){
		_preventNewInput = true;
	}

	public void LookIntoPeephole(int peepholeIndex, Vector3 zoomCameraPosition){
		if (!_preventNewInput) {
			Events.G.Raise (new DisableSceneTransitionInput ());
			// Play Zoom Sound
			_controlRoomAudio.PlayZoomAudio ();
			// Begin Fading the Scene
			_fading.BeginFade (1, 2f);

			// Set Camera Zoom In Position
			_lastZoomPosition = zoomCameraPosition;
			AltCentralControl._peepholeViewed [peepholeIndex] = true;
			_zoomedIn = true;
			AltDirectionUI._enteredPeephole = true;
			StartCoroutine (ZoomCamera (2f, true, peepholeIndex));
		}
	}

	public void ZoomOutOfPeephole(){
		_fading.BeginFade (-1, 2f);
		_zoomedIn = false;
		_controlRoomAudio.PlayZoomAudio ();
		StartCoroutine (ZoomCamera (1f, false));
	}

	IEnumerator ZoomCamera(float duration, bool zoomIn, int peepholeIndex = 0){
		float timer = 0f;
		while (timer < duration) {
			timer += Time.deltaTime;
			if (zoomIn) {
				_mainCamera.transform.position = Vector3.Slerp (_defaultCameraPosition, _lastZoomPosition, timer / duration);
			} else {
				_mainCamera.transform.position = Vector3.Slerp (_lastZoomPosition, _defaultCameraPosition, timer / duration);
			}

			yield return null;
		}
		if (zoomIn) {
			_mainCamera.transform.position = _lastZoomPosition;
			StartCoroutine (ChangeLevel (peepholeIndex));
		} else {
			_mainCamera.transform.position = _defaultCameraPosition;
		}
		yield return null;
	}


	IEnumerator ChangeLevel(int peepholeIndex){
		yield return new WaitForSeconds(0.5f);
		float fadeTime = _fading.BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		switch (peepholeIndex) {
		case 0:
			SceneManager.LoadScene ("Peep1");
			break;
		case 1:
			SceneManager.LoadScene ("Peep2");
			break;
		case 2:
			SceneManager.LoadScene ("Peep3");
			break;
		case 3:
			SceneManager.LoadScene ("Peep4");
			break;
		default:
			break;
		}
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;
		Events.G.AddListener<DisableSceneTransitionInput> (DisableSceneInput);
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
		Events.G.RemoveListener<DisableSceneTransitionInput> (DisableSceneInput);
	}



	void Start(){
		//Starts off removing cover if already uncovered
		PreInitializePeepholeCover ();
		Invoke ("CheckAndRemoveCover", 1f);
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
}
