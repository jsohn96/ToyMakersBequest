using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRotation : MonoBehaviour {

	enum axisToRotate {
		xAxis, yAxis, zAxis
	}
	[Header("0: for both directions, 1: left, 2: right")] 
	[SerializeField] int _oneDirectional = 0;

	// mouse drag rotate 
	public bool isDragStart = false;
	Vector3 dragStartPos;
	Vector3 dragPreviousMousePos;
	float dragSensitivity = 0.05f;
	float hitDist;
	float accAngle = 0; // accumulated angle
	int circleDivision = 12; // default as the clock 
	bool ischeckingConnection = false;

	// lerp node rotation 
	float rotateSpeed = 4f;
	Quaternion finalAngle;
	Quaternion originAngle;
	bool isRotating = false;
	float errorVal = 0.2f;
	Vector3 preAxis;
	Vector3 rotateAxis;
	Vector3 zRotateAxis;
	float preChangingTime;


	[SerializeField] float _dragSensitivity = 10f;
	[SerializeField] bool _isNotebook = false;
	Camera _mainCamera;
	[SerializeField] Camera _nonMainCameraForRayCast;
	int _3DBookLayerMask = 1 << 15;

	Plane circlePlane;
	List<float> snapToAngle;

	[SerializeField] Transform[] _reverseRotation;
	[SerializeField] float[] _reverseRelativeSpeed;
	[SerializeField] axisToRotate[] _reverseAxisToRotate;
	[SerializeField] bool _useZRotateAxisInstead = false;
	[SerializeField] bool _flipDirection = false;
	float _directionFlip = 1.0f;

	[SerializeField] bool _isTopDown = false;

	void Start(){
		snapToAngle = new List<float> (10);
		_mainCamera = Camera.main;

		if (_flipDirection) {
			_directionFlip = -1.0f;
		}
	}


	void Update(){
		RotateWithMouse ();
	}

	public bool GetIsRotating(){
		return isRotating;
	}


	void RotateWithMouse(){


		// start dragging
		if(Input.GetMouseButtonDown(0)){
			Ray mousePositionRay;
			if (!_isNotebook) {
				mousePositionRay = _mainCamera.ScreenPointToRay (Input.mousePosition);
			} else {
				mousePositionRay = _nonMainCameraForRayCast.ScreenPointToRay (Input.mousePosition);
			}
			RaycastHit hit;
			dragPreviousMousePos = Input.mousePosition;
			bool isHit;
			if (!_isNotebook) {
				isHit = Physics.Raycast (mousePositionRay, out hit);
			} else {
				isHit = Physics.Raycast (mousePositionRay, out hit, _3DBookLayerMask);
			}
			if (isHit && hit.collider.gameObject.tag == "RotateCircle") {
				isDragStart = true;
				dragStartPos = hit.point;
				//print ("hit point :" + hit.point);
				hitDist = hit.distance;
				// create a plane ;
				if (_isTopDown) {
					circlePlane = new Plane (Vector3.up, hit.point);
				} else {
					circlePlane = new Plane (Vector3.forward, hit.point);
				}
				preAxis = new Vector3 (0, 0, 0);
				preChangingTime = -1;
			}
		}



		// end dragging: angle snap
		if (Input.GetMouseButtonUp (0)) {
			if (isDragStart) {
				isDragStart = false;
				//hitDist = 0;
				accAngle = 0;
				isRotating = false;
				//Events.G.Raise (new MBNodeRotate (0, false, 0));
			}
		}

		if (isDragStart) {

			Vector3 curMousePos = Vector3.zero;

			Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
			float rayDistance;
			if (circlePlane.Raycast (ray, out rayDistance)) {
				curMousePos = ray.GetPoint(rayDistance);
			}


			// TODO: z needs to be switched to any customized axis
			Vector3 va = Vector3.Normalize(dragStartPos - gameObject.transform.position);
			if (_isTopDown) {
				va.y = 0;
			} else {
				va.z = 0;
			}
			Vector3 vb = Vector3.Normalize (curMousePos - gameObject.transform.position);
			if (_isTopDown) {
				vb.y = 0;
			} else {
				vb.z = 0;
			}
			//print ("z pos chack: " + (va.z - vb.z));
			//rotate from b to a
			rotateAxis = Vector3.Normalize(Vector3.Cross (vb, va));
			print ("Debug: rotate axis " + rotateAxis);

			// remove rotate jitter (changing axis direction abruptly)
			if(rotateAxis != preAxis){
				float deltaChangeTime = Time.time - preChangingTime;

				if (deltaChangeTime <= 0.2f) {
					print("############# Axis Jitter !!!!!");
					return;

				} else {
					preAxis = rotateAxis;
					preChangingTime = Time.time;
				}
			}

			// determine the angle with the mouse position offset 
			float dragDistance = Vector3.Distance(Input.mousePosition, dragPreviousMousePos);
			//print ("Jitter test drag distance: " + dragDistance);
			float distanceToCenter = Vector3.Distance (dragStartPos, transform.position);
			float angle = dragDistance * dragSensitivity * Mathf.Rad2Deg * (5/distanceToCenter);

			dragPreviousMousePos = Input.mousePosition;

			// get the angle 
			//
			//float angle = Mathf.Acos(Vector3.Dot(va, vb))*Mathf.Rad2Deg;
			accAngle += angle;
			//print ("Angle Check: " + accAngle);
			if (accAngle >= _dragSensitivity) {
				isRotating = true;
				accAngle = _dragSensitivity;
//				Quaternion tempRot = Quaternion.Euler (-accAngle * rotateAxis);
//				//print ("Temp rot: " + tempRot);
//				Quaternion curRot = gameObject.transform.rotation;
//				//curRot = curRot + tempRot;
				if (rotateAxis.z > 0f) {
					zRotateAxis =- transform.forward;
				} else {
					zRotateAxis = transform.forward;
				}
				if (_useZRotateAxisInstead) {
					gameObject.transform.Rotate (-accAngle * _directionFlip * zRotateAxis * 0.5f, Space.Self);
				} else {
					gameObject.transform.Rotate (-accAngle * _directionFlip * rotateAxis * 0.5f, Space.Self);
				}
				for (int i = 0; i < _reverseRotation.Length; i++) {
					if (_reverseAxisToRotate [i] == axisToRotate.yAxis) {
						_reverseRotation [i].Rotate ((accAngle * rotateAxis * 0.5f * _reverseRelativeSpeed [i]), Space.Self);
					} else if (_reverseAxisToRotate [i] == axisToRotate.zAxis) {
						_reverseRotation [i].Rotate ((accAngle * zRotateAxis * 0.5f * _reverseRelativeSpeed [i]), Space.Self);
					}
				}
				dragStartPos = curMousePos;
				accAngle = 0;
				Events.G.Raise (new MBNodeRotate (0, true, 0));
			} else {
				isRotating = false;
			}
		}
	}
}
