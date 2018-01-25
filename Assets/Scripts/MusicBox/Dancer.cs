using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

// handels the dancers movement and behaviour in the game 
// add music control for the music box 
// dancer movement --> different gestures according to the activated event 

public enum DancerMove{
	none = 0,
	idleDance
}

public class Dancer : MonoBehaviour {
	Animator _myAnim;                     // future use --> change of animation 
	Transform _myTransform;
	AudioSource _myAudio;
	// 
	[SerializeField] float _DurationSensitivity;      // dancer's moving speed 
	[SerializeField] Transform _myBodyTransform;
	[SerializeField] GameObject _underFootIntersection;
	bool isOnIntersection;
	int correctIdx = -1; // the node that the dancer is supposed to be on
	int shifedIdx = -1;  // the parent of the intersection 

	bool isPathFinished = true;           // is the current path finished 
	bool isMoving = false; 				  // is the dancer currently moving 
	Vector3 _curStartPos;
	Vector3 _curDirection;                 // moving direction --> local movement axis 
	int _curPathLength;
	int _curIndex;                         // link index 
	List<Vector3> _curPathLinkPos;
	int _curPathIndex;                     // path index
	bool _isDancerRotate = false;

	// traverse the spline 
	float _duration = 2f;
	float _progress = 0;
	BezierSpline _activeSpline;
	[SerializeField] SplineWalkerMode _mode;

	bool _isNodeWithPath = false;
	bool _isPreNodeWithPath = false;
	Vector3 _tempVector3KeepingDancerLevel;

	// Use this for initialization
	void Awake () {
		_myTransform = gameObject.transform;
		_myAudio = gameObject.GetComponent<AudioSource> ();
		if (_DurationSensitivity == null) {
			_DurationSensitivity = 10f;
		}
		//
		_curPathLinkPos = new List<Vector3>(100);

			
	}

	void OnEnable(){
		Events.G.AddListener<DancerChangeMoveEvent> (DancerChangeMoveHandel);
		Events.G.AddListener<PathStateManagerEvent> (DancerHoldHandEvent);

	}

	void OnDisable(){
		Events.G.RemoveListener<DancerChangeMoveEvent> (DancerChangeMoveHandel);
		Events.G.RemoveListener<PathStateManagerEvent> (DancerHoldHandEvent);

	}
	
	// Update is called once per frame
	void Update () {
		// spline walker adapt 

		if (isMoving && !isPathFinished) {
			_progress += Time.deltaTime / _duration;
			if (_progress >= 1f) {
				_progress = 1f;
				isMoving = false;
				isPathFinished = true;
				print ("Dancer: reaches the end");
				Events.G.Raise (new DancerFinishPath (_curPathIndex));
				detectDancerExactPosition ();
			}
			if (_activeSpline != null) {
				Vector3 position = _activeSpline.GetPoint (_progress);
				transform.position = position;
				if (isMoving) {
					_tempVector3KeepingDancerLevel = position + _activeSpline.GetDirection (_progress);
					_tempVector3KeepingDancerLevel.y = transform.position.y;
					transform.LookAt (_tempVector3KeepingDancerLevel);
				}
			}
				

		}
		 

		CheckShiftedNodeIdx ();


		if (_isDancerRotate) {
			_myBodyTransform.Rotate (0, -1, 0);
		}



	
	}

	void detectDancerExactPosition(){
		Ray FootRay = new Ray (gameObject.transform.position, -gameObject.transform.up);
		RaycastHit hit;
		int layerMask = 1 << 19;
		if (Physics.Raycast (FootRay, out hit, 10f, layerMask)) {
			if (hit.collider.tag == "intersect") {
				//Debug.Log ("under dance : " + hit.collider.gameObject.name);
				_underFootIntersection = hit.collider.gameObject;
				gameObject.transform.parent = _underFootIntersection.transform;
				isOnIntersection = true;

			} 

		} else {
			isOnIntersection = false;
		}
		
	}

	void CheckShiftedNodeIdx(){
		if (isOnIntersection) {
			int tempIdx = _underFootIntersection.GetComponentInParent<PathNode> ().readNodeInfo ().index;
			if (tempIdx != shifedIdx) {
				shifedIdx = tempIdx;
				if (shifedIdx != correctIdx) {
					Debug.Log("AHHHHHH DERAILED !!!! " + shifedIdx);
				}	
				// send out an event to imform the pathnetwork that the dancer is derailed 

			}
		}
	}

	void RotateForward(){
		//rotation can't reley on the local position --> need to rotate the _currentdirection 
		//_myBodyTransform.up = _curDirection;
		//print ("Direction Check: " + _myBodyTransform.forward + " " + _myBodyTransform.up + " " + _myBodyTransform.right);
		//print ("curDir Check: " + _curDirection);
		// calculate the angles 
		// dot product
		float angle = Mathf.Acos(Vector3.Dot(_curDirection, new Vector3(0,1,0)));
		angle *= Mathf.Rad2Deg;
		//print ("Dancer check dot product: " + angle);

		// cross product 
		Vector3 eularAngle = angle * Vector3.Normalize(Vector3.Cross(new Vector3(0,1,0), _curDirection));

		Quaternion tempRot = Quaternion.Euler (eularAngle);
		//print ("Dancer check rotating: " + eularAngle + " Angle: " + angle + "Quaternion: " + tempRot);

	}

	// the dancer enters the new path 
	public void SetNewPath (PathNode pn){
		// set New Path --> get the current active path
		// set the boolean vals 
		correctIdx = pn.readNodeInfo ().index;
		print("Place dancer on node " + correctIdx);
		isPathFinished = false;

		_curStartPos = transform.position;
		_myTransform.parent = pn.gameObject.transform;
	
		// get the positions info
		_curPathIndex = pn.readNodeInfo().index;
		int activePath = pn.readNodeInfo().activeSegIdx;
		//TODO: add code for p2 
		if (pn.readNodeInfo ().paths!= null && pn.readNodeInfo ().paths.Length > 0) {
			_isNodeWithPath = true;
			isMoving = true;
			_progress = 0f;

			_activeSpline = pn.readNodeInfo ().paths [activePath];
			//print ("Check Active Path" + activePath);
			_curStartPos = _activeSpline.GetPoint (0);

			float pathLength = _activeSpline.GetSplineDuration ();
			if (pathLength == -1f) {
				_duration = _activeSpline.GetPresetDuration ();
			} else {
				//print ("Current Spline Length: " + pathLength);
				_duration = pathLength * _DurationSensitivity;
				//detectDancerExactPosition ();
			}


		} else {
			_isNodeWithPath = false;
			transform.position = _curStartPos;
			//print ("Enter Node");
			_activeSpline = null;
			isMoving = true;
			_progress = 1f;
			//Events.G.Raise (new DancerFinishPath (_curPathIndex));
		}
			
		//* Activate isPathFinished at end of coroutine:  isPathFinished = false;\
		if (_isNodeWithPath && _isPreNodeWithPath) {
			StartCoroutine (LerpBetweenPaths (pn._betweenTransitionLerpDuration));
		} 

		_isPreNodeWithPath = _isNodeWithPath;


	}

	IEnumerator LerpBetweenPaths(float duration){
		float timer = 0.0f;
		Vector3 tempOriginPos = transform.position;
		isMoving = false;
		while (timer < duration) {
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp (tempOriginPos, _curStartPos, timer / duration);
			yield return null;
		}
		transform.position = _curStartPos;
		isPathFinished = false;
		isMoving = true;
		yield return null;

	}

	// TODO: set the dancer behavior --> what the dancer's moves are 
	void DancerChangeMove(DancerMove _dm){
		
		switch(_dm){
		case DancerMove.none:
			_isDancerRotate = false;
			break;
		case DancerMove.idleDance:
			_isDancerRotate = true;
			break;

		}
	}

	void DancerChangeMoveHandel(DancerChangeMoveEvent e){
		DancerChangeMove (e.Move);
	}

	void DancerHoldHandEvent(PathStateManagerEvent e){
		if (e.activeEvent == PathState.hold_hand_with_TM) {
			print ("Dancer start");
			DancerChangeMove (DancerMove.idleDance);
		}
	}
}

