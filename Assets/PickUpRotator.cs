using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRotator : MonoBehaviour 
{
	[SerializeField] float _sensitivity;
	Vector3 _mouseReference;
	Vector3 _mouseOffset;
	Vector3 _rotation;
	bool _isRotating;

	Vector3 _tempRot;

	// pickupable layer mask
	int _pickUpLayerMask = 1 << 9;

	void Start ()
	{
		_rotation = Vector3.zero;
	}

	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Input.GetMouseButtonDown(0)){
			if(Physics.Raycast(ray, out hit, Mathf.Infinity, _pickUpLayerMask)){
				_isRotating = true;
				_mouseReference = Input.mousePosition;
			}
		} else if (Input.GetMouseButtonUp(0)){
			_isRotating = false;
		}
	}

	void OnMouseDrag() {
		if(_isRotating)
		{
			
			// offset
			_mouseOffset = (Input.mousePosition - _mouseReference);

			// apply rotation
			_rotation.z = (_mouseOffset.x) * _sensitivity;
			_rotation.x = (_mouseOffset.y) * _sensitivity;

			// rotate
			transform.Rotate(_rotation, Space.Self);

			// prevent Z axis from rotating
//			Vector3 pos = Camera.main.transform.position;
//			Vector3 dir = (this.transform.position - pos).normalized;
//
//			Quaternion currentRotation = transform.rotation; 
//			currentRotation = Quaternion.LookRotation(dir); 
//			transform.rotation = currentRotation;

			//clamp Rotation
			_tempRot = transform.rotation.eulerAngles;
			if (_tempRot.x > 80.0f && _tempRot.x < 270.0f) {
				_tempRot.x = 80.0f;
			} else if (_tempRot.x < 300.0f && _tempRot.x > 90.0f) {
				_tempRot.x = 300.0f;
			}
			transform.rotation = Quaternion.Euler (_tempRot);

			// store mouse
			_mouseReference = Input.mousePosition;
		}
	}
}
