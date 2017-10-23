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

	bool isPathFinished = true;           // is the current path finished 
	bool isMoving = false; 				  // is the dancer currently moving 
	Vector3 _curStartPos;
	Vector3 _curEndPos;
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

	// playmode 
	PlayMode _myPlayMode;
	bool _isOnStaticTrack = false;

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
		Events.G.AddListener<MBPlayModeEvent> (PlayModeHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<DancerChangeMoveEvent> (DancerChangeMoveHandel);
		Events.G.RemoveListener<PathStateManagerEvent> (DancerHoldHandEvent);
		Events.G.RemoveListener<MBPlayModeEvent> (PlayModeHandle);

	}
	
	// Update is called once per frame
	void Update () {
		// spline walker adapt 
		if (_myPlayMode == PlayMode.MBPrototype_With_Path) {
			if (isMoving && !isPathFinished) {
				_progress += Time.deltaTime / _duration;
				if (_progress >= 1f) {
					_progress = 1f;
					isMoving = false;
					isPathFinished = true;
					print ("Dancer: reaches the end");
					Events.G.Raise (new DancerFinishPath (_curPathIndex));
				}


				// playing the music
				if (!_myAudio.isPlaying) {
					_myAudio.Play ();
					print ("Dancer Player Music");
				}
				Vector3 position = _activeSpline.GetPoint (_progress);
				transform.position = position;
				if (isMoving) {
					transform.LookAt (position + _activeSpline.GetDirection (_progress));
				}

			} else if (isPathFinished) {
				if (_myAudio.isPlaying) {
					_myAudio.Pause ();
				}
			}
		} else if (_myPlayMode == PlayMode.MBPrototype2_Without_Path) {
			// adjust the dancer rotation 
			if (isMoving &&!isPathFinished) {
				// if the dancer is on the static track
				if (_isOnStaticTrack ) {
					_progress += Time.deltaTime / _duration;
					if (_progress >= 1f) {
						_progress = 1f;
						isMoving = false;
						isPathFinished = true;
						print ("Dancer: reaches the end");
						Events.G.Raise (new DancerFinishPath (_curPathIndex));

					}


					Vector3 position = _activeSpline.GetPoint (_progress);
					transform.position = position;
					if (isMoving) {
						transform.LookAt (position + _activeSpline.GetDirection (_progress));
					}
				} else {
					print ("Dancer: dancer onboard the circle");
					//Events.G.Raise (new DancerFinishPath (_curPathIndex));
					
				}


				// playing the music
				if (!_myAudio.isPlaying) {
					_myAudio.Play ();
					print ("Dancer Player Music");
				}

				if (isPathFinished) {
					if (_isOnStaticTrack) {
						_isOnStaticTrack = false;
					}
				}


			} else if (isPathFinished) {
				if (_myAudio.isPlaying) {
					_myAudio.Pause ();
				}


			}

		}


		if (_isDancerRotate) {
			_myBodyTransform.Rotate (0, -1, 0);
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
		isMoving = true;
		isPathFinished = false;

	

		//_myTransform.parent = pn.gameObject.transform.parent;
		_myTransform.parent = pn.gameObject.transform;
		Quaternion tempRot = pn.gameObject.transform.rotation;
		//_myTransform.rotation = tempRot;


		// get the positions info
		_curPathIndex = pn.readNodeInfo().index;
		int activePath = pn.readNodeInfo().activeSegIdx;
		//TODO: add code for p2 
		if (pn.readNodeInfo ().paths != null) {
			_progress = 0f;
			_isOnStaticTrack = true;
			_activeSpline = pn.readNodeInfo ().paths [activePath];
			//print ("Check Active Path" + activePath);
			_curStartPos = _activeSpline.GetPoint (0);

			float pathLength = _activeSpline.GetSplineDuration ();
			//print ("Current Spline Length: " + pathLength);
			_duration = pathLength * _DurationSensitivity;
		} else {
			_isOnStaticTrack = false;
			print ("Enter Node");
			//_activeSpline == null;
		}


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


	void PlayModeHandle(MBPlayModeEvent e){
		_myPlayMode = e.activePlayMode;
	}


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
