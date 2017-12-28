using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToOriginOnDisable : MonoBehaviour {
	Vector3 _originPosition;

	// Use this for initialization
	void Start () {
		_originPosition = transform.position;
	}
	
	void OnDisable(){
		transform.position = _originPosition;
	}
}
