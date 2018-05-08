using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatrePulley : MonoBehaviour {
	//[SerializeField] bool _isClockWise;
	[SerializeField] Transform _wheel;
	float _rotateSpeed = 0f;
	float _friction = 100f;
	bool _isRotating = false;

	Quaternion _reactAngle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_isRotating) {
			Debug.Log ("Pulley Rotating");
			if (_rotateSpeed <= 80f) {
				_rotateSpeed += Time.deltaTime * _friction;
			}
			_wheel.RotateAround (_wheel.position, _wheel.forward, _rotateSpeed * Time.deltaTime);
		} 

//		if (Input.GetKey (KeyCode.A)) {
//			_isRotating = !_isRotating;
//		}
		
	}

	public void StartRotate(){
		if (!_isRotating) {
			_isRotating = true;
		}
	}

	public void StopRotate(){
		if (_isRotating) {
			_isRotating = false;
		}
		
	}

	void ReactToClick(){
		
	}
}
