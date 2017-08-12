using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour 
{

	[SerializeField] float _sensitivity;
	Vector3 _mouseReference;
	Vector3 _mouseOffset;
	Vector3 _rotation;
	bool _isRotating;

	Vector3 _tempRot;

	void Start ()
	{
		_rotation = Vector3.zero;
	}

	void Update()
	{
	// Temporary disabling of rotate
		if(Input.GetKeyDown(KeyCode.Q)){
			GetComponent<BoxCollider>().enabled = false;
		}
		if(Input.GetKeyDown(KeyCode.W)){
			GetComponent<BoxCollider>().enabled = true;
		}


		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
		if(Input.GetMouseButtonDown(0)){
			if(Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Traversal"))){
				if(hit.collider.gameObject.name == "Traversal"){
					_isRotating = true;
					_mouseReference = Input.mousePosition;
				}
			}
		}
	}
	void OnMouseDrag(){
	if(_isRotating){
		_mouseOffset = (Input.mousePosition - _mouseReference);

			// apply rotation
			_rotation.y = (_mouseOffset.x) * _sensitivity;
			_rotation.x = -(_mouseOffset.y) * _sensitivity;

			// rotate
			transform.Rotate(_rotation, Space.Self);

			// prevent Z axis from rotating
			Vector3 pos = Camera.main.transform.position;
			Vector3 dir = (this.transform.position - pos).normalized;

			Quaternion currentRotation = transform.rotation; 
			currentRotation = Quaternion.LookRotation(dir); 
			transform.rotation = currentRotation;

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


	void OnMouseUp()
	{
		// rotating flag
		_isRotating = false;
	}

}
