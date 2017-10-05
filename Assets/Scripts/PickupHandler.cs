using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour {
	Camera _mainCamera;
	bool _carrying;
	public bool carrying
	{
		get {return _carrying; }
		set {_carrying = value; }
	}

	bool _dropping = false;
	bool _isDropZoneDrop = false;
	DropZone _dropZoneReference = null;

	GameObject _carriedObject;
	GameObject _droppingObject;
	[SerializeField] float _distance;
	[SerializeField] float _smooth;

	Timer _pickUpTimer;
	[SerializeField] float _pickUpDuration = 0.5f;
	Timer _dropOffTimer;
	[SerializeField] float _dropOffDuration = 0.5f;
	Vector3 _forwardRotation = new Vector3 (90.0f, 0.0f, 0.0f);
	Vector3 _dropZoneRotation = new Vector3 (180.0f, -90.0f, 0.0f);
	Quaternion _tempOriginRotation;
	Vector3 _tempOriginPosition;

	Quaternion _tempDropOriginRotation;
	Vector3 _tempDropOriginPosition;
	Quaternion _tempDropGoalRotation;
	Vector3 _tempDropGoalPosition;

	Collider _droppingCollider;

	[SerializeField] Transform _cameraNormalizer;
	[SerializeField] Camera _uiCamera;

	// Set Layer Mask to Traversal
	int _traversalExclusionLayerMask = 1 << 8;
	int _uiDrawerLayerMask = 1 << 11;
	int _DropZoneLayerMask = 1 << 12;
	int _pickupableInBoxLayerMask = 1 << 13;

	int _uiDropZoneLayerMask;

	//Double Click Related Variables
	//bool _oneClick = false;
	//Timer _doubleClickTimer;
	//float _doubleClickDelay = 0.25f;
	//Pickupable _cachedPickupable = null;

	void Awake () {
		_uiDropZoneLayerMask = _uiDrawerLayerMask | _DropZoneLayerMask;

		_mainCamera = Camera.main;
		_pickUpTimer = new Timer (_pickUpDuration);
		_dropOffTimer = new Timer (_dropOffDuration);
		//include all but Traversal Layer
		_traversalExclusionLayerMask = ~_traversalExclusionLayerMask;

		//_doubleClickTimer = new Timer (_doubleClickDelay);
	}
	
	// Update is called once per frame
	void Update () {
		//DoubleClickReset ();

		if(_carrying){
			Carry(_carriedObject);
			CheckDrop();
		} else {
			if(_dropping){
				if(!_dropOffTimer.IsOffCooldown){
					if (_isDropZoneDrop) {
						_tempDropGoalPosition = _dropZoneReference.position;
						_tempDropGoalRotation = Quaternion.Euler(_dropZoneReference.eulerRotation);
					}
					Vector3 tempPos = Vector3.Lerp (_tempDropOriginPosition, _tempDropGoalPosition, _dropOffTimer.PercentTimePassed);
					Quaternion tempRot = Quaternion.Lerp (_tempDropOriginRotation, _tempDropGoalRotation, _dropOffTimer.PercentTimePassed);
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
		carriedObject.transform.position = Vector3.Lerp(_tempOriginPosition, _cameraNormalizer.position + _cameraNormalizer.forward * _distance, _pickUpTimer.PercentTimePassed);
		// have this stop after completion
		if(!_pickUpTimer.IsOffCooldown){
			MaintainRotation(carriedObject);
		}
	}

	// Keeps the rotation of the picked up object consistent during Camera Rotation
	void MaintainRotation(GameObject carriedObject){
		Vector3 adjustedForwardRotation = _mainCamera.transform.rotation.eulerAngles + _forwardRotation;
		carriedObject.transform.rotation = Quaternion.Lerp (_tempOriginRotation, Quaternion.Euler(_forwardRotation), _pickUpTimer.PercentTimePassed);

		//carriedObject.transform.rotation = Quaternion.Lerp (_tempOriginRotation, Quaternion.Euler (adjustedForwardRotation), _pickUpTimer.PercentTimePassed);
	}

	void Pickup(){
		if(Input.GetMouseButtonDown(0)){
			Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Ray uiRay = _uiCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit uiHit;

			if(Physics.Raycast(ray, out hit, Mathf.Infinity, _traversalExclusionLayerMask)){
				Pickupable p = hit.collider.GetComponent<Pickupable> ();
				//if (_cachedPickupable == null) {
				//	_cachedPickupable = p;
				//	_oneClick = true;
				//	_doubleClickTimer.Reset ();
				//} else if (_oneClick && (_cachedPickupable == p)) {
					if (p != null) {
					//	DoubleClickReset ();

						FinishDrop ();
						
					p.isPickedUp = true;
						_pickUpTimer.Reset ();
						_carrying = true;
						_carriedObject = p.gameObject;
						_tempOriginPosition = _carriedObject.transform.position;
						_tempOriginRotation = _carriedObject.transform.rotation;
						hit.collider.isTrigger = true;
						p.GetComponent<Rigidbody> ().isKinematic = true;
					}
				//}
			}
			// pick up pickupables in Box
			else if (Physics.Raycast (uiRay, out uiHit, Mathf.Infinity, _pickupableInBoxLayerMask)) {
				Pickupable p = uiHit.collider.GetComponent<Pickupable> ();
				if (p != null && p.isInBox) {
					FinishDrop ();

					p.isPickedUp = true;
					_pickUpTimer.Reset ();
					_carrying = true;
					_carriedObject = p.gameObject;
					_tempOriginPosition = _carriedObject.transform.position;
					_tempOriginRotation = _carriedObject.transform.rotation;
					uiHit.collider.isTrigger = true;
					p.GetComponent<Rigidbody> ().isKinematic = true;
				} 	
					
			
			}
		}
	}

	void CheckDrop(){
		if(Input.GetMouseButtonDown(0)){
			Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, _traversalExclusionLayerMask)) {
				Pickupable p = hit.collider.GetComponent<Pickupable> ();
				if (p != null && p.isPickedUp) {
				}
				else if (hit.collider.gameObject.tag == "Drawer") {
					// For the drawer under table
					// do nothing

				} else if (hit.collider.gameObject.tag == "DropZone"){
					_dropZoneReference = hit.collider.gameObject.GetComponent<DropZone> ();
					if (_dropZoneReference != null && !_dropZoneReference.occupied) {
						Pickupable carriedP = _carriedObject.GetComponent<Pickupable> ();
						if (_dropZoneReference.identifier == carriedP.identifier) {
							PlaceInDropZone (carriedP, _dropZoneReference);
						} else {
							DropObject ();
						}
					} else {
						//drop in box drawer
						PutInDrawer (hit.collider.gameObject.transform.position);
					}
				}
				else {
					DropObject ();
				}
			} else {
				Ray uiRay = _uiCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit uiHit;
				if (Physics.Raycast (uiRay, out uiHit, Mathf.Infinity, _uiDropZoneLayerMask)) {
					if (uiHit.collider.gameObject.tag == "Drawer") {
					} else if (uiHit.collider.gameObject.tag == "DropZone"){
						//drop in box drawer
						PutInDrawer (uiHit.collider.gameObject.transform.position);
					} else {
						DropObject ();
					}
				} else {
					DropObject ();
				}
			}
			//if (!_oneClick) {
			//	_doubleClickTimer.Reset ();
			//	_oneClick = true;
			//} else {
				
			//	DoubleClickReset ();
			//}
		}
	}

	void FinishDrop(){
		if(_dropping){
			
			if (!_isDropZoneDrop) {
				_droppingCollider.isTrigger = false;
				_droppingObject.GetComponent<Rigidbody> ().isKinematic = false;
			} else {
				_tempDropGoalPosition = _dropZoneReference.position;
				_tempDropGoalRotation = Quaternion.Euler(_dropZoneReference.eulerRotation);
				_dropZoneReference.occupied = true;
				_dropZoneReference = null;
			}
			_droppingObject.transform.SetPositionAndRotation (_tempDropGoalPosition, _tempDropGoalRotation);
			_droppingCollider.enabled = true;
			_dropping = false;
			_isDropZoneDrop = false;
		}
	}

	void DropObject(){
		_tempDropGoalRotation = _tempOriginRotation;
		_tempDropGoalPosition = _tempOriginPosition;

		_droppingObject = _carriedObject;
		_tempDropOriginPosition = _droppingObject.transform.position;
		_tempDropOriginRotation = _droppingObject.transform.rotation;

		_droppingCollider =  _droppingObject.GetComponent<Collider>();
		//set pickupable false
		_droppingObject.GetComponent<Pickupable>().isPickedUp = false;

		_droppingCollider.enabled = false;
		_carrying =false;
		_dropping = true;
		_dropOffTimer.Reset();
		//_carriedObject.GetComponent<Rigidbody>().isKinematic = false;
		_carriedObject = null;
	}

	//void DoubleClickReset(){
	//	if (_doubleClickTimer.IsOffCooldown) {
	//		_oneClick = false;
	//		_cachedPickupable = null;
	//	}
	//}

	void PlaceInDropZone(Pickupable p, DropZone d){
		_tempDropGoalRotation = Quaternion.Euler(d.eulerRotation);
		_tempDropGoalPosition = d.position;

		_droppingObject = _carriedObject;
		//set object to pickupable
		_droppingObject.layer = 9;

		_tempDropOriginPosition = _droppingObject.transform.position;
		_tempDropOriginRotation = _droppingObject.transform.rotation;

		_droppingCollider =  _droppingObject.GetComponent<Collider>();

		//put the object under dropzone parent
		_droppingObject.transform.parent = d.parent;

		p.isPickedUp = false;
		p.isInBox = false;
		p.isInDropZone = true;

		_droppingCollider.enabled = false;
		_carrying = false;
		_dropping = true;

		_isDropZoneDrop = true;
		_dropOffTimer.Reset ();
		_carriedObject = null;
	}

	void PutInDrawer(Vector3 dropZonePosition){
		_tempDropGoalRotation = Quaternion.Euler(_dropZoneRotation);
		_tempDropGoalPosition = dropZonePosition;

		_droppingObject = _carriedObject;
		//set pickupable object to UI box layer
		_droppingObject.layer = 13;

		_tempDropOriginPosition = _droppingObject.transform.position;
		_tempDropOriginRotation = _droppingObject.transform.rotation;

		_droppingCollider =  _droppingObject.GetComponent<Collider>();
		//set pickupable false
		Pickupable pickupableScript = _droppingObject.GetComponent<Pickupable>();
		pickupableScript.isPickedUp = false;
		pickupableScript.isInBox = true;

		_droppingCollider.enabled = false;
		_carrying = false;
		_dropping = true;
		_dropOffTimer.Reset ();
		_carriedObject = null;
	}
}
