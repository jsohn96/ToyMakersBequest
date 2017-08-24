using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour {
	Camera _mainCamera;
	bool _carrying;
	bool _dropping = false;
	GameObject _carriedObject;
	GameObject _droppingObject;
	[SerializeField] float _distance;
	[SerializeField] float _smooth;

	Timer _pickUpTimer;
	[SerializeField] float _pickUpDuration = 0.5f;
	Timer _dropOffTimer;
	[SerializeField] float _dropOffDuration = 0.5f;

	Vector3 _forwardRotation = new Vector3 (90.0f, 0.0f, 0.0f);
	Quaternion _tempOriginRotation;
	Vector3 _tempOriginPosition;

	Quaternion _tempDropOriginRotation;
	Vector3 _tempDropOriginPosition;
	Quaternion _tempDropGoalRotation;
	Vector3 _tempDropGoalPosition;

	Collider _droppingCollider;

	// Set Layer Mask to Traversal
	int _traversalExclusionLayerMask = 1 << 8;

	//Double Click Related Variables
	bool _oneClick = false;
	Timer _doubleClickTimer;
	float _doubleClickDelay = 0.25f;
	Pickupable _cachedPickupable = null;

	void Awake () {
		_mainCamera = Camera.main;
		_pickUpTimer = new Timer (_pickUpDuration);
		_dropOffTimer = new Timer (_dropOffDuration);
		//include all but Traversal Layer
		_traversalExclusionLayerMask = ~_traversalExclusionLayerMask;

		_doubleClickTimer = new Timer (_doubleClickDelay);
	}
	
	// Update is called once per frame
	void Update () {
		DoubleClickReset ();

		if(_carrying){
			Carry(_carriedObject);
			CheckDrop();
		} else {
			if(_dropping){
				if(!_dropOffTimer.IsOffCooldown){
					Vector3 tempPos = Vector3.Lerp(_tempDropOriginPosition, _tempDropGoalPosition, _dropOffTimer.PercentTimePassed);
					Quaternion tempRot = Quaternion.Lerp(_tempDropOriginRotation, _tempDropGoalRotation, _dropOffTimer.PercentTimePassed);
					_droppingObject.transform.SetPositionAndRotation(tempPos, tempRot);
				} else {
					FinishDrop();
				}
			}
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
				Pickupable p = hit.collider.GetComponent<Pickupable> ();
				if (_cachedPickupable == null) {
					_cachedPickupable = p;
					_oneClick = true;
					_doubleClickTimer.Reset ();
				} else if (_oneClick && (_cachedPickupable == p)) {
					if (p != null) {
						DoubleClickReset ();

						FinishDrop ();
						_pickUpTimer.Reset ();
						_carrying = true;
						_carriedObject = p.gameObject;
						_tempOriginPosition = _carriedObject.transform.localPosition;
						_tempOriginRotation = _carriedObject.transform.rotation;
						hit.collider.isTrigger = true;
						p.GetComponent<Rigidbody> ().isKinematic = true;
					}
				}
			}
		}
	}

	void CheckDrop(){
		if(Input.GetMouseButtonDown(0)){
			if (!_oneClick) {
				_doubleClickTimer.Reset ();
				_oneClick = true;
			} else {
				DropObject();
				DoubleClickReset ();
			}
		}
	}

	void FinishDrop(){
		if(_dropping){
			_droppingObject.transform.SetPositionAndRotation(_tempDropGoalPosition, _tempDropGoalRotation);
			_droppingCollider.isTrigger = false;
			_droppingCollider.enabled = true;
			_droppingObject.GetComponent<Rigidbody>().isKinematic = false;
			_dropping = false;
		}
	}

	void DropObject(){
		_tempDropGoalRotation = _tempOriginRotation;
		_tempDropGoalPosition = _tempOriginPosition;

		_droppingObject = _carriedObject;
		_tempDropOriginPosition = _droppingObject.transform.localPosition;
		_tempDropOriginRotation = _droppingObject.transform.rotation;

		_droppingCollider =  _droppingObject.GetComponent<Collider>();
		_droppingCollider.enabled = false;
		_carrying =false;
		_dropping = true;
		_dropOffTimer.Reset();
		//_carriedObject.GetComponent<Rigidbody>().isKinematic = false;
		_carriedObject = null;
	}

	void DoubleClickReset(){
		if (_doubleClickTimer.IsOffCooldown) {
			_oneClick = false;
			_cachedPickupable = null;
		}
	}
}
