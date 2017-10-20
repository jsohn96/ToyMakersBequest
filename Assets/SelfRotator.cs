using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotator : MonoBehaviour 
{

	[SerializeField] float _sensitivity;
	Vector3 _mouseReference;
	Vector3 _mouseOffset;
	Vector3 _rotation;
	bool _isRotating;
	[SerializeField] Transform _cameraNormalizer;

	Vector3 _tempRot;
	// Set Layer Mask to Traversal
	int _traversalLayerMask = 1 << 8;
	int _pickUpLayerMask = 1 << 9;
	int _combinedLayerMask;

	void Awake(){
		_combinedLayerMask = _traversalLayerMask | _pickUpLayerMask;
	}

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
			if(Physics.Raycast(ray, out hit,  Mathf.Infinity,  _combinedLayerMask)){
				if(hit.collider.gameObject.tag == "Traversal"){
					_isRotating = true;
					_mouseReference = Input.mousePosition;
				}
			}
		} else if(Input.GetMouseButtonUp(0)){
			// rotating flag
			_isRotating = false;
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
			//Vector3 pos = Camera.main.transform.position;
			//Debug.Log(pos);
			//Vector3 pos = _cameraNormalizer.transform.position;
			//Vector3 dir = (this.transform.position - pos).normalized;

			//Quaternion currentRotation = transform.rotation;
			//currentRotation = Quaternion.LookRotation(dir); 
			//transform.rotation = currentRotation;

			//clamp Rotation
			_tempRot = transform.rotation.eulerAngles;
			if (_tempRot.x > 80.0f && _tempRot.x < 270.0f) {
				_tempRot.x = 80.0f;
			} else if (_tempRot.x < 300.0f && _tempRot.x > 90.0f) {
				_tempRot.x = 300.0f;
			}
			_tempRot.z = 0.0f;
			transform.rotation = Quaternion.Euler (_tempRot);

			// store mouse
			_mouseReference = Input.mousePosition;
		}
	}
}
