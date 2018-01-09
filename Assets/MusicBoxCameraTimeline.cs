using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicBoxCameraStates {
	init = 0,
	intro = 1,
	activation = 2
}

public class MusicBoxCameraTimeline : MonoBehaviour {
	[SerializeField] MusicBoxCameraManager _musicBoxCameraManager;

	[SerializeField] CameraControlPoint _cameraStartPoint, _cameraEndPoint;
	[SerializeField] Transform _camerControlContainer;
	[SerializeField] CameraControlPoint[] _cameraControlPoints;

	bool _preventInput = true;

	int cnt = 0;
	bool _delayFiring = false;

	MusicBoxCameraStates _currentCameraState = MusicBoxCameraStates.init;

	[SerializeField] AudioSource _musicBoxAccompany;

	void Start(){
		int childCnt = _camerControlContainer.childCount;
		_cameraControlPoints = new CameraControlPoint[childCnt];
		for (int i = 0; i < childCnt; i++) {
			_cameraControlPoints [i] = _camerControlContainer.GetChild (i).GetComponent<CameraControlPoint>();
		}
		StartCoroutine (Delay (0.5f));
	}


	void Update(){
		if (!_preventInput) {
			if (Input.GetMouseButtonDown (0)) {
				_currentCameraState++;
				_preventInput = true;
				float lerpDuration = 0f;
				switch (_currentCameraState) {
				case MusicBoxCameraStates.intro:
					lerpDuration = _cameraStartPoint.duration;
					_musicBoxCameraManager.MoveToWayPoint (_cameraStartPoint.transform, lerpDuration, _cameraStartPoint.fov);
					StartCoroutine (Delay (lerpDuration));
					break;
				case MusicBoxCameraStates.activation:
					StartCoroutine (DelayEventFiring (3.0f));
					StartCoroutine(AudioManager.instance.FadeOut (_musicBoxAccompany, 2.2f, 0.0f, 0.3f, false, true));
					_musicBoxCameraManager.ActivateStaticFollow (5.0f);
					break;
				default:
					break;
				}
				if (!_delayFiring) {
					Events.G.Raise (new MBCameraStateManagerEvent (_currentCameraState, lerpDuration));
				}





					
			} else if (Input.GetKeyDown (KeyCode.E)) {
				_musicBoxCameraManager.MoveToWayPoint (_cameraEndPoint.transform, _cameraEndPoint.duration, _cameraEndPoint.fov, true);
			}
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			if (cnt < GetControlPointCount ()) {
				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
				StartCoroutine (DelayedFollowCam (_cameraControlPoints [cnt].duration));
				cnt++;
			}
		}
	}

	public CameraControlPoint GetControlPoint(int index){
		if (index < _cameraControlPoints.Length) {
			return _cameraControlPoints [index];
		} else {
			return null;
		}
	}

	public int GetControlPointCount(){
		return _cameraControlPoints.Length;
	}

	IEnumerator Delay(float duration){
		yield return new WaitForSeconds (duration);

		_preventInput = false;
	}

	IEnumerator DelayEventFiring(float duration){
		_delayFiring = true;
		_preventInput = true;
		yield return new WaitForSeconds(duration);
		_delayFiring = false;
		_preventInput = false;
		Events.G.Raise (new MBCameraStateManagerEvent (_currentCameraState, 0.0f));
	}


	IEnumerator DelayedFollowCam(float duration){
		_preventInput = true;
		yield return new WaitForSeconds(duration+0.5f);
		_musicBoxCameraManager.ActivateStaticFollow (3.0f);
	}
}
