using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour {
	Camera _mainCamera;
	bool _carrying;
	GameObject _carriedObject;
	[SerializeField] float _distance;
	[SerializeField] float _smooth;

	Timer _pickUpTimer;
	[SerializeField] float _pickUpDuration = 2.0f;

	Vector3 _forwardRotation = new Vector3 (90.0f, 0.0f, 0.0f);
	Quaternion _tempOriginRotation;
	Vector3 _tempOriginPosition;

	// Set Layer Mask to Traversal
	int _traversalExclusionLayerMask = 1 << 8;

	void Awake () {
			_mainCamera = Camera.main;
			_pickUpTimer = new Timer (_pickUpDuration);

			//include all but Traversal Layer
		_traversalExclusionLayerMask = ~_traversalExclusionLayerMask;
	}
	
	// Update is called once per frame
	void Update () {
		if(_carrying){
			Carry(_carriedObject);
			CheckDrop();
		} else {
			Pickup();
		}
	}

	void RotateObject(){
		_carriedObject.transform.Rotate(5,10,15);
	}

	void Carry(GameObject carriedObject){
		carriedObject.transform.position = Vector3.Lerp(_tempOriginPosition, _mainCamera.transform.position + _mainCamera.transform.forward * _distance, _pickUpTimer.PercentTimePassed);
		// have this stop after completion
		if(!_pickUpTimer.IsOffCooldown){
			MaintainRotation(carriedObject);
		}
	}

	// Keeps the rotation of the picked up object consistent during Camera Rotation
	void MaintainRotation(GameObject carriedObject){
		Vector3 adjustedForwardRotation = _mainCamera.transform.rotation.eulerAngles + _forwardRotation;
		carriedObject.transform.rotation = Quaternion.Lerp (_tempOriginRotation, Quaternion.Euler (adjustedForwardRotation), _pickUpTimer.PercentTimePassed);
	}

	void Pickup(){
		if(Input.GetMouseButtonDown(0)){
			Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, Mathf.Infinity, _traversalExclusionLayerMask)){
				Pickupable p = hit.collider.GetComponent<Pickupable>();
				if(p != null){
					_pickUpTimer.Reset();
					_carrying = true;
					_carriedObject = p.gameObject;
					_tempOriginPosition = _carriedObject.transform.localPosition;
					_tempOriginRotation = _carriedObject.transform.rotation;
					p.GetComponent<Rigidbody>().isKinematic = true;
				}
			}
		}
	}

	void CheckDrop(){
		if(Input.GetMouseButtonDown(0)){
			DropObject();
		}
	}

	void DropObject(){
		_carrying =false;
		_carriedObject.GetComponent<Rigidbody>().isKinematic = false;
		_carriedObject = null;
	}
}
