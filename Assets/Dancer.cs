using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

// handels the dancers movement and behaviour in the game 

public class Dancer : MonoBehaviour {
	Animator _myAnim;                     // future use --> change of animation 
	Transform _myTransform;

	// 
	[SerializeField] float _mySpeed;      // dancer's moving speed 
	[SerializeField] Transform _myBodyTransform;

	bool isPathFinished = true;           // is the current path finished 
	bool isMoving = false; 				  // is the dancer currently moving 
	Vector3 _curStartPos;
	Vector3 _curEndPos;
	Vector3 _curDirection;                 // moving direction --> local movement axis 
	int _curPathLength;
	int _curIndex;
	List<Vector3> _curPathLinkPos;

	// Use this for initialization
	void Awake () {
		_myTransform = gameObject.transform;
		if (_mySpeed == null) {
			_mySpeed = 10f;
		}
		//
		_curPathLinkPos = new List<Vector3>(100);
			
	}
	
	// Update is called once per frame
	void Update () {
		// TODO Inplement dragging later
		if (isMoving && !isPathFinished) {
			print ("Dancer: Keep Moving " + Vector3.Distance (_myTransform.localPosition + _mySpeed * _curDirection * Time.deltaTime, _curEndPos));
			if (Mathf.Abs (Vector3.Distance (_myTransform.localPosition + _mySpeed * _curDirection * Time.deltaTime, _curEndPos)) > 0.0001f
			    && Vector3.Normalize (_curEndPos - (_myTransform.localPosition + _mySpeed * _curDirection * Time.deltaTime)) == _curDirection) {
				_myTransform.Translate (_mySpeed * _curDirection * Time.deltaTime, Space.Self);
			} else {
				isMoving = false;
				_myTransform.localPosition = _curEndPos;
			}


			// if reaches the end of the path 
			// do something 
		} else if (!isMoving && !isPathFinished) {
			if (_curIndex + 1 < _curPathLength) {
				_curIndex += 1;
				print ("Dancer move on to link No." + _curIndex);
				_curStartPos = _curPathLinkPos [_curIndex];
				if (_curIndex + 1 < _curPathLength) {
					_curEndPos = _curPathLinkPos [_curIndex + 1];
				} else {
					_curEndPos = _curStartPos;
				}
				_curDirection = Vector3.Normalize (_curEndPos - _curStartPos);
				isMoving = true;
			} else {
				isMoving = false;
				isPathFinished = true;
				print ("Dancer: reaches the end");
				Events.G.Raise (new DancerFinishPath ());
			}
		} else {
			//
		}


		
	}

	// the dancer enters the new path 
	public void SetNewPath (PathNode pn){
		// set the boolean vals 
		isMoving = true;
		isPathFinished = false;
		//_myTransform.parent = pn.gameObject.transform.parent;
		_myTransform.parent = pn.gameObject.transform;
		Quaternion tempRot = pn.gameObject.transform.rotation;

		// get the positions info
		Vector3[] TempPos = pn.readNodeInfo ().pathPositions;
		_curPathLinkPos.Clear ();
		for (int i= 0; i < TempPos.Length; i++) {
			_curPathLinkPos.Add (TempPos [i]);
		}
		//print ("Check: " + _curPathLinkPos);
		_curPathLength = _curPathLinkPos.Count;
		_curIndex = 0;
		_curStartPos = _curPathLinkPos [_curIndex];
		if (_curIndex + 1 < _curPathLength) {
			_curEndPos = _curPathLinkPos [_curIndex + 1];
		} else {
			_curEndPos = _curStartPos;
		}

		_curDirection = Vector3.Normalize (_curEndPos - _curStartPos);
		_myTransform.localPosition = _curStartPos;
		tempRot = new Quaternion (0, 0, 0, 0);
		tempRot = pn.gameObject.transform.rotation;
		_myTransform.rotation = tempRot;
		//_myBodyTransform.LookAt (_curEndPos);	

		// face along the path 
		//_myTransform.LookAt(_curEndPos);

		/*
		//tempRot = new Quaternion (0, 0, 0, 0);
		_myTransform.rotation = tempRot;
	
		// get the direction of moving 
		//_curDirection = -Vector3.Normalize(pn.readNodeInfo().endPos - pn.readNodeInfo().startPos);
		_curDirection = new Vector3 (-1f, 0, 0);
		// get the start point 
		//Vector3 n = new Vector3 (-0.5f, -0.5f, -0.5f);	
		//_curStartPos = Vector3.Scale (n, _curDirection);
		_curStartPos = new Vector3 (0.5f, 0, 0);
		// get the end point 
		//Vector3 m = new Vector3 (0.5f, 0.5f, 0.5f);
		//_curEndPos = Vector3.Scale (m, _curDirection);

		_curEndPos = new Vector3 (-0.5f, 0, 0);

		// set the dancer position 
	
		_myTransform.localPosition = _curStartPos;
		*/



		
	}

	// TODO: set the dancer behavior --> what the dancer's moves are 



}
