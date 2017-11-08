using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxCameraTimeline : MonoBehaviour {
	[SerializeField] MusicBoxCameraManager _musicBoxCameraManager;

	[SerializeField] CameraControlPoint _cameraStartPoint, _cameraEndPoint;
	[SerializeField] Transform _camerControlContainer;
	[SerializeField] CameraControlPoint[] _cameraControlPoints;

	bool _initialized = false;

	int cnt = 0;

	void Start(){
		int childCnt = _camerControlContainer.childCount;
		_cameraControlPoints = new CameraControlPoint[childCnt];
		for (int i = 0; i < childCnt; i++) {
			_cameraControlPoints [i] = _camerControlContainer.GetChild (i).GetComponent<CameraControlPoint>();
		}
	}


	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (!_initialized) {
				_initialized = true;
				_musicBoxCameraManager.MoveToWayPoint (_cameraStartPoint.transform, _cameraStartPoint.duration, _cameraStartPoint.fov);
			}
		} else if (Input.GetKeyDown(KeyCode.E)) {
			_musicBoxCameraManager.MoveToWayPoint (_cameraEndPoint.transform, _cameraEndPoint.duration, _cameraEndPoint.fov, true);
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			if (cnt < GetControlPointCount ()) {
				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
				cnt++;
			}
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			_musicBoxCameraManager.ActivateStaticFollow ();
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
}
