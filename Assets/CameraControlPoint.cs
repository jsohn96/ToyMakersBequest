using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlPoint : MonoBehaviour {
	[SerializeField] Transform[] _cameraControlPoints;

	void Start(){
		int childCnt = transform.childCount;
		_cameraControlPoints = new Transform[childCnt];
		for (int i = 0; i < childCnt; i++) {
			_cameraControlPoints [i] = transform.GetChild (i);
		}
	}

	public Transform GetControlPoint(int index){
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
