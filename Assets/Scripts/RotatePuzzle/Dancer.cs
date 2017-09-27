using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

// handels the dancers movement and behaviour in the game 
// add music control for the music box 

public class Dancer : MonoBehaviour {
	Animator _myAnim;                     // future use --> change of animation 
	Transform _myTransform;
	AudioSource _myAudio;
	// 
	[SerializeField] float _DurationSensitivity;      // dancer's moving speed 
	[SerializeField] Transform _myBodyTransform;

	bool isPathFinished = true;           // is the current path finished 
	bool isMoving = false; 				  // is the dancer currently moving 
	Vector3 _curStartPos;
	Vector3 _curEndPos;
	Vector3 _curDirection;                 // moving direction --> local movement axis 
	int _curPathLength;
	int _curIndex;                         // link index 
	List<Vector3> _curPathLinkPos;
	int _curPathIndex;                     // path index

	// traverse the spline 
	float _duration = 2f;
	float _progress = 0;
	BezierSpline _activeSpline;
	[SerializeField] SplineWalkerMode _mode;

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
	
	// Update is called once per frame
	void Update () {
		// spline walker adapt 
		if(isMoving && !isPathFinished){
			_progress += Time.deltaTime / _duration;
			if (_progress >= 1f) {
				_progress = 1f;
				isMoving = false;
				isPathFinished = true;
				print ("Dancer: reaches the end");
				Events.G.Raise (new DancerFinishPath (_curPathIndex));
			}


			// playing the music
			if(!_myAudio.isPlaying){
				_myAudio.Play ();
			}

		}else if(isPathFinished){
			if(_myAudio.isPlaying){
				_myAudio.Pause();
			}
//			_progress -= Time.deltaTime / _duration;
//			if (_progress < 0f) {
//				_progress = -_progress;
//				isMoving = true;
//			} else {
//				
//			}
		}

		Vector3 position = _activeSpline.GetPoint(_progress);
		transform.position = position;
		if (isMoving) {
			transform.LookAt(position + _activeSpline.GetDirection(_progress));
		}

	}

	void RotateForward(){
		//rotation can't reley on the local position --> need to rotate the _currentdirection 
		//_myBodyTransform.up = _curDirection;
		//print ("Direction Check: " + _myBodyTransform.forward + " " + _myBodyTransform.up + " " + _myBodyTransform.right);
		print ("curDir Check: " + _curDirection);
		// calculate the angles 
		// dot product
		float angle = Mathf.Acos(Vector3.Dot(_curDirection, new Vector3(0,1,0)));
		angle *= Mathf.Rad2Deg;
		print ("Dancer check dot product: " + angle);

		// cross product 
		Vector3 eularAngle = angle * Vector3.Normalize(Vector3.Cross(new Vector3(0,1,0), _curDirection));

		Quaternion tempRot = Quaternion.Euler (eularAngle);
		print ("Dancer check rotating: " + eularAngle + " Angle: " + angle + "Quaternion: " + tempRot);

	}

	// the dancer enters the new path 
	public void SetNewPath (PathNode pn){
		// set New Path --> get the current active path
		// set the boolean vals 
		isMoving = true;
		isPathFinished = false;
		_progress = 0f;
	

		//_myTransform.parent = pn.gameObject.transform.parent;
		_myTransform.parent = pn.gameObject.transform;
		Quaternion tempRot = pn.gameObject.transform.rotation;
		_myTransform.rotation = tempRot;


		// get the positions info
		_curPathIndex = pn.readNodeInfo().index;
		int activePath = pn.readNodeInfo().activeSegIdx;
		_activeSpline = pn.readNodeInfo ().paths [activePath];
		print ("Check Active Path" + activePath);
		_curStartPos = _activeSpline.GetPoint (0);

		float pathLength = _activeSpline.GetSplineDuration ();
		print ("Current Spline Length: " + pathLength);
		_duration = pathLength * _DurationSensitivity;
		//Vector3[] TempPos = pn.readNodeInfo ().paths[activePath].GetPathInfo();
//		_curPathLinkPos.Clear ();
//		for (int i= 0; i < TempPos.Length; i++) {
//			_curPathLinkPos.Add (TempPos [i]);
//		}
//		//print ("Check: " + _curPathLinkPos);
//		_curPathLength = _curPathLinkPos.Count;
//		_curIndex = 0;
//		_curStartPos = _curPathLinkPos [_curIndex];
//		if (_curIndex + 1 < _curPathLength) {
//			_curEndPos = _curPathLinkPos [_curIndex + 1];
//		} else {
//			_curEndPos = _curStartPos;
//		}
//
		//_curDirection = Vector3.Normalize (_curEndPos - _curStartPos);
		//_myTransform.localPosition = _curStartPos;



		//RotateForward ();

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



// old code
// TODO Inplement dragging later --> in update 
/*
		if (isMoving && !isPathFinished) {
			//print ("Dancer: Keep Moving " + Vector3.Distance (_myTransform.localPosition + _DurationSensitivity * _curDirection * Time.deltaTime, _curEndPos));
			if (Mathf.Abs (Vector3.Distance (_myTransform.localPosition + _DurationSensitivity * _curDirection * Time.deltaTime, _curEndPos)) > 0.0001f
			    && Vector3.Normalize (_curEndPos - (_myTransform.localPosition + _DurationSensitivity * _curDirection * Time.deltaTime)) == _curDirection) {
				_myTransform.Translate (_DurationSensitivity * _curDirection * Time.deltaTime, Space.Self);
			} else {
				isMoving = false;
				_myTransform.localPosition = _curEndPos;
			}

			// playing the music
			if(!_myAudio.isPlaying){
				_myAudio.Play ();
			}

			// if reaches the end of the path 
			// do something 
		} else if (!isMoving && !isPathFinished) {
			if(_myAudio.isPlaying){
				_myAudio.Pause();
			}
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
				RotateForward ();
				isMoving = true;
			} else {
				isMoving = false;
				isPathFinished = true;
				print ("Dancer: reaches the end");
				Events.G.Raise (new DancerFinishPath (_curPathIndex));
				//Events.G.Raise (new SetPathNodeEvent (_curPathIndex));
			}
		} else {
			//
		}
	*/
