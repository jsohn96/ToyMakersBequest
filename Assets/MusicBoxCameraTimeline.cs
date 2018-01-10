using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicBoxCameraStates {
	init = 0,
	intro = 1,
	activation = 2,
	firstStairsToDoor =3
}

public class MusicBoxCameraTimeline : MonoBehaviour {
	[SerializeField] MusicBoxCameraManager _musicBoxCameraManager;

	[SerializeField] CameraControlPoint _cameraStartPoint, _cameraEndPoint;
	[SerializeField] Transform _camerControlContainer;
	[SerializeField] CameraControlPoint[] _cameraControlPoints;
	int _controlPointCnt;

	bool _preventInput = true;

	int cnt = 0;
	bool _delayFiring = false;

	MusicBoxCameraStates _currentCameraState = MusicBoxCameraStates.init;

	[SerializeField] AudioSource _musicBoxAccompany;

	int _nodeDancerIsAboutToEnter = 0;

	bool _doubleEntrance12 = false, _doubleEntrance13 = false, _doubleEntrance14 = false;

	void Start(){
		int childCnt = _camerControlContainer.childCount;
		_cameraControlPoints = new CameraControlPoint[childCnt];
		for (int i = 0; i < childCnt; i++) {
			_cameraControlPoints [i] = _camerControlContainer.GetChild (i).GetComponent<CameraControlPoint>();
		}
		StartCoroutine (Delay (0.5f));
		_controlPointCnt = GetControlPointCount ();
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


	IEnumerator DelayedFollowCam(float duration, float fov = 0){
		_preventInput = true;
		yield return new WaitForSeconds(duration);
		if (fov == 0) {
			_musicBoxCameraManager.ActivateStaticFollow ();
		} else {
			_musicBoxCameraManager.ActivateStaticFollow (fov);
		}
	}

	IEnumerator DelayedNextControlPoint(float duration){
		yield return new WaitForSeconds(duration);
		_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
		cnt++;
	}


	void OnEnable(){
		Events.G.AddListener<DancerOnBoard> (DancerOnBoardHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<DancerOnBoard> (DancerOnBoardHandle);
	}

	void DancerOnBoardHandle(DancerOnBoard e){
		_nodeDancerIsAboutToEnter = e.NodeIdx;
		// 5: down the stairs
		if (cnt < _controlPointCnt) {
			if (_nodeDancerIsAboutToEnter == 5) {
				cnt = 0;
				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
				StartCoroutine (DelayedFollowCam (_cameraControlPoints [cnt].duration, 3.5f));
				cnt++;
			} else if (_nodeDancerIsAboutToEnter == 6) {
				cnt = 1;
				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
				cnt++;
			} else if (_nodeDancerIsAboutToEnter == 7) {
				cnt = 2;
				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
				cnt++;
				StartCoroutine (DelayedNextControlPoint (_cameraControlPoints [cnt-1].duration - 0.8f));
			} else if (_nodeDancerIsAboutToEnter == 8) {
				cnt = 4;
				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
				cnt++;
//				_musicBoxCameraManager.ActivateStaticFollow (5f);
			} else if (_nodeDancerIsAboutToEnter == 9) {
				_musicBoxCameraManager.ActivateStaticFollow (5f);
			} else if (_nodeDancerIsAboutToEnter == 10) {
				cnt = 5;
				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
				cnt++;
				StartCoroutine (DelayedFollowCam (_cameraControlPoints [cnt-1].duration, 3f));
			}
			else if (_nodeDancerIsAboutToEnter == 11) {
//				cnt = ;
//				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
//				cnt++;
				_musicBoxCameraManager.ActivateStaticFollow (5f);
			}
			else if (_nodeDancerIsAboutToEnter == 12) {
				if (!_doubleEntrance12) {
					cnt = 6;
					_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
					cnt++;
					StartCoroutine (DelayedNextControlPoint (_cameraControlPoints [cnt - 1].duration - 0.8f));
//				_musicBoxCameraManager.ActivateStaticFollow (5f);
					_doubleEntrance12 = true;
				} else {
					cnt = 8;
					_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
					cnt++;
				}
			}
			else if (_nodeDancerIsAboutToEnter == 13) {
				if (!_doubleEntrance13) {
					_musicBoxCameraManager.ActivateStaticFollow (7f);
					_doubleEntrance13 = true;
				} else {
					_musicBoxCameraManager.ActivateStaticFollow (9f);
				}
			}
			else if (_nodeDancerIsAboutToEnter == 14) {
				if (!_doubleEntrance14) {
					_musicBoxCameraManager.ActivateStaticFollow (5f);
					_doubleEntrance14 = true;
				} else {
					_musicBoxCameraManager.ActivateStaticFollow (7f);
				}
			}
			else if (_nodeDancerIsAboutToEnter == 17) {
				cnt = 9;
				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
				cnt++;
			}
			else if (_nodeDancerIsAboutToEnter == 18) {
				cnt = 10;
				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
				cnt++;
			}
			else if (_nodeDancerIsAboutToEnter == 20) {
				cnt = 11;
				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
				cnt++;
				StartCoroutine (DelayedNextControlPoint (_cameraControlPoints [cnt - 1].duration - 0.8f));
			}
			else if (_nodeDancerIsAboutToEnter == 21) {
				cnt = 13;
				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
				cnt++;
				StartCoroutine (DelayedNextControlPoint (_cameraControlPoints [cnt - 1].duration - 0.8f));
			}
		}
		// 6: towards the door
	}
}
