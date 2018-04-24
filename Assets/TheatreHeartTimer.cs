using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreHeartTimer : MonoBehaviour {

	enum axisToRotate {
		xAxis, yAxis, zAxis, drivenByMain
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
	[SerializeField] bool _isNotUsingMainCamera = false;
	[SerializeField] Camera _mainCamera;
	[SerializeField] Camera _nonMainCameraForRayCast;
	[SerializeField] LayerMask _whichLayerMask;

	Plane circlePlane;
	List<float> snapToAngle;

	[SerializeField] axisToRotate _mainAxis;
	[Header("Same Direction Rotate Transforms")]
	[SerializeField] bool _hasOtherRotations = false;
	[SerializeField] bool _rightHeart;
	[SerializeField] Transform[] _sameRotation;

	[Header("Reverse Rotation")]
	[SerializeField] Transform[] _reverseRotation;
	[SerializeField] float[] _reverseRelativeSpeed;
	[SerializeField] axisToRotate[] _reverseAxisToRotate;
	[SerializeField] bool _flipDirection = false;
	[SerializeField] AnimationCurve _curve;
	float _directionFlip = 1.0f;
	Vector3 curMousePos;

	int _thisInstanceID;

	[SerializeField] TheatreHeartTimer _theOtherHalfHeartTimer;

	[SerializeField] Quaternion[] _timerTicks;

	bool _heartIsActive = false;


	[SerializeField] AltTheatre _myTheatre;

	[SerializeField] SpriteFade _spriteFade;

	public void ActivateHeart(){
		_spriteFade.CallFadeSpriteIn (0f);
		_heartIsActive = true;
		_thisInstanceID = transform.GetInstanceID ();

		snapToAngle = new List<float> (10);

		if (_flipDirection) {
			_directionFlip = -1.0f;
		}

		Vector3 p = transform.position;

		// rotate around Y 
		if (_mainAxis == axisToRotate.yAxis) {
			p = p + transform.right;
			circlePlane = new Plane (transform.up, p);
			//print ("init debug: " + p);
		} else if (_mainAxis == axisToRotate.zAxis) {
			p = p + transform.right;
			circlePlane = new Plane (transform.forward, p);
			//print ("init debug: " + p);
		} else if (_mainAxis == axisToRotate.xAxis) {
			p = p + transform.up;
			circlePlane = new Plane (transform.right, p);
			//print ("init debug: " + p);
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(dragStartPos, 0.01f);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, dragStartPos);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(curMousePos, 0.01f);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, curMousePos);
	}


	void Update(){
//		if (Input.GetKeyDown (KeyCode.S)) {
//			ActivateHeart ();
//		}
//
		if (_heartIsActive) {
			RotateWithMouse ();

			float forwardX = transform.forward.x;

			if (_rightHeart) {
				if (forwardX >= 0.98f) {
					_heartIsActive = false;
					SnapTickerIntoStart ();
					_theOtherHalfHeartTimer.enabled = true;
					_myTheatre.MoveToNext ();
					_theOtherHalfHeartTimer.SnapTickerIntoStart ();
				}
			} else {
				if (forwardX <= -0.98f) {
					_heartIsActive = false;
					SnapTickerIntoStart ();
					_theOtherHalfHeartTimer.enabled = true;
					_theOtherHalfHeartTimer.SnapTickerIntoStart ();

				}
			}
		}
	}

	public void SnapTickerIntoStart(){
		_spriteFade.TurnItOffForGood ();

		_theOtherHalfHeartTimer.enabled = true;
		isDragStart = false;
		isRotating = false;

		transform.localRotation = _timerTicks [3];
		StartCoroutine (SnapTickerToStart ());
	}



	IEnumerator SnapTickerToStart(){
		yield return new WaitForSeconds (1f);
		float timer = 0f;
		float duration = 1f;
		while (duration > timer) {
			transform.localRotation = Quaternion.Lerp (_timerTicks[3], _timerTicks [2], _curve.Evaluate(timer / duration));
			timer += Time.deltaTime;
			yield return null;
		}
		transform.localRotation = _timerTicks [2];
		yield return new WaitForSeconds (1f);

		timer = 0f;
		while (duration > timer) {
			transform.localRotation = Quaternion.Lerp (_timerTicks[2], _timerTicks [1], _curve.Evaluate(timer / duration));
			timer += Time.deltaTime;
			yield return null;
		}
		transform.localRotation = _timerTicks [1];
		yield return new WaitForSeconds (1f);

		timer = 0f;
		while (duration > timer) {
			transform.localRotation = Quaternion.Lerp (_timerTicks[1], _timerTicks [0], _curve.Evaluate(timer / duration));
			timer += Time.deltaTime;
			yield return null;
		}
		transform.localRotation = _timerTicks [0];
		yield return null;
	}

	public bool GetIsRotating(){
		return isRotating;
	}



	void RotateWithMouse(){


		// start dragging
		if(Input.GetMouseButtonDown(0)){

				Ray mousePositionRay;
				if (!_isNotUsingMainCamera) {
					mousePositionRay = _mainCamera.ScreenPointToRay (Input.mousePosition);
				} else {
					mousePositionRay = _nonMainCameraForRayCast.ScreenPointToRay (Input.mousePosition);
				}
				RaycastHit hit;
				dragPreviousMousePos = Input.mousePosition;
				bool isHit;
				if (!_isNotUsingMainCamera) {
					isHit = Physics.Raycast (mousePositionRay, out hit, Mathf.Infinity, _whichLayerMask);
					Debug.Log (isHit);
				} else {
					isHit = Physics.Raycast (mousePositionRay, out hit, Mathf.Infinity, _whichLayerMask);
				}
				if (isHit && hit.collider.gameObject.tag == "DragRotation") {
					if (hit.transform.GetInstanceID() == _thisInstanceID) {
						isDragStart = true;
						_theOtherHalfHeartTimer.enabled = false;
						//dragStartPos = hit.point;
						float rayDistance;
						if (circlePlane.Raycast (mousePositionRay, out rayDistance)) {
							dragStartPos = mousePositionRay.GetPoint (rayDistance);
						}
						//print ("hit point :" + hit.point);
						//hitDist = rayDistance;
						// create a plane ;
						//circlePlane = new Plane (transform.up, hit.point);
						preAxis = new Vector3 (0, 0, 0);
						preChangingTime = -1;
					}
				}

		}



		// end dragging: angle snap
		if (Input.GetMouseButtonUp (0)) {
			if (isDragStart) {
				_theOtherHalfHeartTimer.enabled = true;
				isDragStart = false;
				//hitDist = 0;
				accAngle = 0;
				isRotating = false;
				//Events.G.Raise (new MBNodeRotate (0, false, 0));
			}
		}

		if (isDragStart) {

			curMousePos = Vector3.zero;

			Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
			float rayDistance;
			if (circlePlane.Raycast (ray, out rayDistance)) {
				curMousePos = ray.GetPoint(rayDistance);
			}


			// TODO: z needs to be switched to any customized axis
			Vector3 va = Vector3.Normalize(dragStartPos - gameObject.transform.position);
			//va.z = 0;
			Vector3 vb = Vector3.Normalize (curMousePos - gameObject.transform.position);
			//vb.z = 0;
			//print ("z pos chack: " + (va.z - vb.z));
			//rotate from b to a
			rotateAxis = Vector3.Normalize(Vector3.Cross (vb, va));
			print ("Debug: rotate axis " + rotateAxis);

			// remove rotate jitter (changing axis direction abruptly)
			if(rotateAxis != preAxis){
				float deltaChangeTime = Time.time - preChangingTime;

				if (deltaChangeTime <= 0.02f) {
					//					print("############# Axis Jitter !!!!!");
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
				gameObject.transform.Rotate (-accAngle * _directionFlip * rotateAxis * 0.5f, Space.World);



				bool positiveDirection;
				if (rotateAxis.z > 0f) {
					positiveDirection = true;
				} else {
					positiveDirection = false;
				}

				if (rotateAxis.z == 0) {
					rotateAxis.z = 1f;
				}

				if (_hasOtherRotations) {
					for (int i = 0; i < _sameRotation.Length; i++) {
						if (_rightHeart) {
							if (positiveDirection) {
								_sameRotation [i].Rotate (-accAngle * -_directionFlip * _sameRotation [i].right * 0.5f, Space.World);
							} else {
								_sameRotation [i].Rotate (-accAngle * _directionFlip * _sameRotation [i].right * 0.5f, Space.World);
							}
						} else {
							if (positiveDirection) {
								_sameRotation [i].Rotate (-accAngle * _directionFlip * _sameRotation [i].right * 0.5f, Space.World);
							} else {
								_sameRotation [i].Rotate (-accAngle * -_directionFlip * _sameRotation [i].right * 0.5f, Space.World);
							}
						}
					}
				}

				for (int i = 0; i < _reverseRotation.Length; i++) {
					if (_reverseAxisToRotate [i] == axisToRotate.drivenByMain) {
						_reverseRotation [i].Rotate ((-accAngle * _directionFlip * rotateAxis * 0.5f * _reverseRelativeSpeed [i]), Space.World);
					}
					else if (positiveDirection) {
						if (_reverseAxisToRotate [i] == axisToRotate.yAxis) {
							_reverseRotation [i].Rotate ((-accAngle * _directionFlip * _reverseRotation [i].up * 0.5f * _reverseRelativeSpeed [i]), Space.World);
						} else if (_reverseAxisToRotate [i] == axisToRotate.zAxis) {
							_reverseRotation [i].Rotate ((-accAngle * _directionFlip * _reverseRotation [i].forward * 0.5f * _reverseRelativeSpeed [i]), Space.World);
						} else if (_reverseAxisToRotate [i] == axisToRotate.xAxis) {
							_reverseRotation [i].Rotate ((-accAngle * _directionFlip * _reverseRotation [i].right * 0.5f * _reverseRelativeSpeed [i]), Space.World);
						} else {
							return;
						}
					} else {
						if (_reverseAxisToRotate [i] == axisToRotate.yAxis) {
							_reverseRotation [i].Rotate ((-accAngle * _directionFlip * -_reverseRotation [i].forward * 0.5f * _reverseRelativeSpeed [i]), Space.World);
						} else if (_reverseAxisToRotate [i] == axisToRotate.zAxis) {
							_reverseRotation [i].Rotate ((-accAngle * _directionFlip * -_reverseRotation [i].forward * 0.5f * _reverseRelativeSpeed [i]), Space.World);
						} else if (_reverseAxisToRotate [i] == axisToRotate.xAxis) {
							_reverseRotation [i].Rotate ((-accAngle * _directionFlip * -_reverseRotation [i].right * 0.5f * _reverseRelativeSpeed [i]), Space.World);
						} else {
							return;
						}
					}
				}
				dragStartPos = curMousePos;
				accAngle = 0;
				if (_oneDirectional == 1) {
					positiveDirection = !positiveDirection;
				} else if (_oneDirectional == 0) {
					positiveDirection = true;
				}
				Events.G.Raise (new DragRotationEvent (true, positiveDirection));
			} else {
				isRotating = false;
			}
		}
	}
}
