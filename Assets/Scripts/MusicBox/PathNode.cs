using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonColor{
	Red = 0,
	Yellow,
	Brown,
	None,
	RotatableObject,
	Branch,
	Descend

};

[System.Serializable]
public struct AdjacentNode{
	public int adjNodeIdx;
	public float relativeAngle;
	AdjacentNode(int id, float angle){
		adjNodeIdx = id;
		relativeAngle = angle;
	}
}

[System.Serializable]
public struct InterlockNode{
	public int sendToIdx;
	public float lockAngle;
	public GameObject intersection;
	InterlockNode(int id, float angle, GameObject itc){
		sendToIdx = id;
		lockAngle = angle;
		intersection = itc;
	}
}

[System.Serializable]
public struct BranchNode{
	public int jumpToNodeIdx;
	public float connectAngle;
	BranchNode(int id, float angle){
		jumpToNodeIdx = id;
		connectAngle = angle;
	}
}


// passing data to the path network 
public struct NodeInfo{
	public BezierSpline[] paths;
	public int index;
	public bool isConnected;
	public int activeSegIdx;
	public AdjacentNode[] adjNodes;
	public ButtonColor controlColor;
	public NodeInfo(BezierSpline[] p, int i, bool iscnnt, int actseg, AdjacentNode[] adj, ButtonColor btn){
		paths = p;
		index = i;
		isConnected = iscnnt;
		activeSegIdx = actseg;
		adjNodes = adj;
		controlColor = btn;
	}	

}


[System.Serializable]
public class InterlockStruct{
	public int relativeNodeIdx;
	public float unlockAngle;
	public int unlockNodeIdx;
	public GameObject intersection;
	public bool isActive;
}

[System.Serializable]
public class InterClockNode{
	public List<InterlockStruct> _interlockMembers;
	// check for each interlockmember, if all parts are collocted 
	public bool checkActivity(){
		bool isfoundmatch = false;
		foreach (InterlockStruct its in _interlockMembers) {
			if (its.isActive == true) {
				isfoundmatch = true;
			} else {
				isfoundmatch = false;
				break;
			}
		}

		return isfoundmatch;
	}
}




public class PathNode : MonoBehaviour {
	[SerializeField] ButtonColor _ControlColor;      // the color of the button that controls the node 
	float _rotateAngle;
	[SerializeField] int _nodeIndex;
	Transform _nodeTransform;
	//[SerializeField] float[] _assignedDegree;          // the correct rotation  
	[SerializeField] Material _GreenMat;
	[SerializeField] Renderer _cylinderRenderer;
	Material _originMat;
	//
	[SerializeField] AdjacentNode[] _adjacentNode;              // the adjacent node index and angle
	bool _isDancerOnBoard = false;                                      // 1 - on, 0-off    

	Vector3 _startPos;
	Vector3 _endPos;
	public bool _isCorrectConnection = false;               // if the path is correctly connected 
	                          // if the path node has dependency on others 

	PathNetwork _myNetWork;
	NodeInfo _myNodeInfo;
	//Vector3[] _myPathPos;
	//Path[] _myPaths;
	BezierSpline[] _mySplines;
	[SerializeField] int _segCount;
	[SerializeField] int _curSegIdx = 0;
	//float _duration = 10f;
	//float progress = 0;

	//bool _isWinded = false;


	// mouse drag rotate 
	bool isDragStart = false;
	Vector3 dragStartPos;
	Vector3 dragPreviousMousePos;
	float dragSensitivity = 0.05f;
	float hitDist;
	float accAngle = 0; // accumulated angle
	//int circleDivision = 12; // default as the clock 
	//bool ischeckingConnection = false;

	// lerp node rotation 
	float rotateSpeed = 4f;
	Quaternion finalAngle;
	Quaternion originAngle;
	Quaternion lastRestAngle;   // when the circle was static last time 
	bool isRotating = false;
	bool isShaking = false;
	float shakeDecay = 5f;
	float errorVal = 0.2f;
	Vector3 preAxis;
	Vector3 rotateAxis;
	float preChangingTime;

	List<float> snapToAngle;
	Timer disableRotateTimer;
	bool isTempDisableActive = true;
	bool isTempDisable = false;

	// playmode 
	//PlayMode _myPlayMode = PlayMode.MBPrototype2_Without_Path;

	Vector3 debugPos1;
	Vector3 debugPos2;

	Plane circlePlane;

	// for interlocked nodes
	[SerializeField] bool _isInterLocked = false;  
	[SerializeField] bool _isControlActive = true ;
	[SerializeField] bool _isCheckConnectionActive = true;
	[SerializeField] GameObject _intersectionPart;

	[SerializeField] InterlockNode[] _interlockNodes;
	[SerializeField] BranchNode[] _branchNodes;
	[SerializeField] bool _isSnapEnable = true;
	[SerializeField] bool _isCheckSameAsControl = false;


	// for clock maze structure
	[SerializeField] List<GameObject> _activeIntersections;

	// descend node --> for stairs 
	bool _isDescended = false;
	bool _isDescending = false;
	Vector3 _finalDescendPos;
 
	[SerializeField] float _dragSensitivity = 10f;
	[SerializeField] bool _isNotebook = false;
	[SerializeField] Camera _nonMainCameraForRayCast;
	int _3DBookLayerMask = 1 << 15;

	bool _glowInfoSent = false;
	[SerializeField] shaderGlowCustom _shaderGlowCustom;

	public float _betweenTransitionLerpDuration = 0.25f;

	void OnEnable(){
		Events.G.AddListener<SetPathNodeEvent> (SetPathEventHandle);
		Events.G.AddListener<DancerOnBoard> (DancerOnBoardHandle);
		Events.G.AddListener<MBTurnColorCircle> (TurnColorCircleHandle);
		Events.G.AddListener<InterlockNodeStateEvent> (InterlockNodeStateHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<SetPathNodeEvent> (SetPathEventHandle);
		Events.G.RemoveListener<DancerOnBoard> (DancerOnBoardHandle);
		Events.G.RemoveListener<MBTurnColorCircle> (TurnColorCircleHandle);
		Events.G.RemoveListener<InterlockNodeStateEvent> (InterlockNodeStateHandle);
	}


	// Use this for initialization
	void Awake () {
		_activeIntersections = new List<GameObject> ();
		_myNetWork = GetComponentInParent<PathNetwork> ();
		_nodeTransform = gameObject.transform;
		if (_ControlColor != ButtonColor.None) {
			if (_cylinderRenderer!= null && _cylinderRenderer.GetComponent<Renderer> () ) {
				_originMat = _cylinderRenderer.GetComponent<Renderer> ().material;
			}
		
		}


		// static node 
		if (_ControlColor == ButtonColor.None) {
			_isCorrectConnection = true;
		} else {
			_isCorrectConnection = false;
		}
		// 

		if (_isInterLocked && _intersectionPart == null) {
			//print("Check interlock section");
			
		}

		if (!_isInterLocked && !_isControlActive) {
			print ("ERROR: non interlock nodes should be active : " +_nodeIndex);
			_isControlActive = true;
		}



	}



	void Start(){
		initNodePathInfo ();
		snapToAngle = new List<float> (10);
		disableRotateTimer = new Timer (1);
		if (_isCheckSameAsControl) {
			_isCheckConnectionActive = _isControlActive;
		}
	}

	void initNodePathInfo(){
		//get the positions of the path links 	
		_mySplines = GetComponentsInChildren<BezierSpline> ();
		//print ("Debug " + _nodeIndex + " : has spline: " + _mySplines.Length);
		// TODO store the position value for different path segments 

		if (_mySplines.Length > 0) {
			if (_mySplines.Length <= (_adjacentNode.Length / 2)) {
				_segCount = _mySplines.Length;
			} else {
				_segCount = _adjacentNode.Length / 2;
			} 
			//_segCount = _mySplines.Length;
			//print ("Check: path number in total " + _segCount + " " + _nodeIndex);
			_myNodeInfo = new NodeInfo (_mySplines, _nodeIndex, _isCorrectConnection, _curSegIdx, _adjacentNode, _ControlColor);
		} else {
			//print ("ERROR: need to construct path first");
			_myNodeInfo = new NodeInfo (null, _nodeIndex, _isCorrectConnection, _curSegIdx, _adjacentNode, _ControlColor);
		}
		

	}

	public void SetCheckClockInterlock(bool clockNodeActive, List<GameObject> activeIntersections){
		_isControlActive = clockNodeActive;
		_activeIntersections = activeIntersections;
		if (_isCheckSameAsControl) {
			_isCheckConnectionActive = _isControlActive;
		}
	}
	public void SetCheckConnection(bool checkConnectionActive, bool isCheckIn){
		if (isCheckIn == !_isDancerOnBoard) {
			_isCheckConnectionActive = checkConnectionActive;
		}
			
	}

	// Update is called once per frame
	void Update () {
		// Rotate Circles
		RotateWithMouse();	

		//print ("debug check temp rotate disabled: " + isTempDisable);

		if(_isControlActive){
			//ClickWithMouse ();_curSegIdx

			if (_isDescending) {
				DescendNode ();
			}

			if (disableRotateTimer.IsOffCooldown && !isTempDisableActive) {
				isTempDisable = false;
				//isDragStart = false;
			}
				
		}// end of checking isActive

	}


	void FixedUpdate(){
		if (_isControlActive) {
			if (isRotating) {
				if (!_glowInfoSent) {
					//do the mouse enter thing _shaderGlowCustom
					_glowInfoSent = true;
				}

				// rotate the node
				if (Mathf.Abs (Quaternion.Angle (transform.localRotation, finalAngle)) > errorVal * 20f) {
					transform.localRotation = Quaternion.Lerp (transform.localRotation, finalAngle, Time.deltaTime * rotateSpeed);
				} else {
					transform.localRotation = finalAngle;
					isRotating = false;
				}
			} else {
				if (_glowInfoSent) {
					// send the mouse out
					_glowInfoSent = false;
				}
			}

			CheckDescend ();
			//CheckNodeConnection ();
		
		}

	}

	void LateUpdate(){
		// only check connection when node is active 
		if (_isCheckConnectionActive) {
			CheckNodeConnection ();
		} else {
			//Debug.Log (_nodeIndex + "$$ set to false ");
			if (_isCorrectConnection) {
				_isCorrectConnection = false;
				_myNodeInfo.isConnected = _isCorrectConnection;
			}
		}
	}

	void TurnColorCircleHandle(MBTurnColorCircle e){
		if (_nodeIndex != e.activeIdx && _ControlColor == e.activeColor) {
			print ("check control color: " + e.activeColor);
			if (_isControlActive) {
				RotateNode ();
			}

		}
	}

	void ButtonControlListener(){
		switch (_ControlColor) {
		case ButtonColor.Red:
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				print ("Rotate Red");
				RotateNode ();
			}
			break;
		case ButtonColor.Brown:
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				print ("Rotate Brown");
				RotateNode ();
			}
			break;
		case ButtonColor.Yellow:
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				print ("Rotate Yellow");
				RotateNode ();
			}
			break;

		}
	}

	// rotate 90 degrees clockwise 
	void RotateNode(){
		// temp Rot Degree = 0, 90, 180, 270 
		//float tempRotDegree = transform.rotation.eulerAngles.z;
		if(!isRotating){
			isRotating = true;
			originAngle = transform.localRotation;
			//transform.Rotate(0,0,-90);
			finalAngle = transform.localRotation;
			finalAngle = originAngle * Quaternion.Euler (0, 0, 90);

		}


	}

	void DoNotRotate(){
		// rotate aniamtion 
		if(!isRotating){
			// 

		}else{
			Debug.Log ("is currently rotating");
		}

	}
		

	// for interlock nodes 
	void CheckInterlockNodes(float degree){
		bool isfoundMatch = false;
		if (_interlockNodes != null && _interlockNodes.Length > 0) {
			foreach (InterlockNode itn in _interlockNodes) {
				if (Mathf.Abs (DampAngle (degree) - DampAngle (itn.lockAngle)) <= errorVal  * 20f) {
					Events.G.Raise (new InterlockNodeStateEvent (true, _nodeIndex, itn.sendToIdx));
					Events.G.Raise (new MBLotusFlower (true, _nodeIndex));
					isfoundMatch = true;
					_intersectionPart = itn.intersection;
				} else {
					Events.G.Raise (new InterlockNodeStateEvent (false, _nodeIndex, itn.sendToIdx));
					Events.G.Raise (new MBLotusFlower (false, _nodeIndex));
				}
			}

			if (_adjacentNode.Length <= 0) {
				_isCorrectConnection = isfoundMatch;
				_myNodeInfo.isConnected = _isCorrectConnection;
			}
		}
	}

	void CheckBranchNodes(float degree){
		bool isConnected = false;
		if (_branchNodes != null && _branchNodes.Length > 0 ) {
			foreach (BranchNode brn in _branchNodes) {
				if (Mathf.Abs (DampAngle (degree) - DampAngle (brn.connectAngle)) <= errorVal  * 20f) {
					isConnected = true;
					_isCorrectConnection = true;
					_myNodeInfo.isConnected = _isCorrectConnection;
					if (_isDancerOnBoard) {
						print (" branching dancer on board to emit event");
						Events.G.Raise (new MBPathIndexEvent (brn.jumpToNodeIdx));
					} 

					return;
				} 
			}
			//Events.G.Raise (new MBPathIndexEvent (-1));
			_isCorrectConnection = false;
			_myNodeInfo.isConnected = _isCorrectConnection;
		}

	}

	void CheckSnapToAngle(int curCheckSegmentIdx){
		snapToAngle.Clear ();
		//
		if (_isInterLocked ) {
			for (int i = 0; i < _interlockNodes.Length; i++) {
				snapToAngle.Add ( _interlockNodes [i].lockAngle);
			}
		}

		//
		if (_ControlColor == ButtonColor.Branch) {
			for (int i = 0; i < _branchNodes.Length; i++) {
				float angle = _branchNodes [i].connectAngle;
				if (!snapToAngle.Contains (angle)) {
					snapToAngle.Add (angle);
				}

			}
		} 

		if (_adjacentNode.Length > 0) {
			//print ("debug check segment id " + curCheckSegmentIdx);
			float tempangle = _adjacentNode [curCheckSegmentIdx].relativeAngle;
			if (!snapToAngle.Contains (tempangle)) {
				snapToAngle.Add (tempangle);
			}
		}


		return;

	}


	void CheckNodeConnection(){
		if (_ControlColor != ButtonColor.None) {
			float tempRotDegree = transform.localRotation.eulerAngles.z;
			int curCheckIdx = 0;
			if (!_isDancerOnBoard) {
				//print (_nodeIndex + ": Check In " + curCheckIdx);
				curCheckIdx = _curSegIdx * 2;
			} else {
				curCheckIdx = _curSegIdx * 2 + 1;
				//print (_nodeIndex + ": Check Out "+ curCheckIdx);
			}

			// check the correct snap-to-angle 
			CheckSnapToAngle(curCheckIdx);

			//print("Current Node " + _nodeIndex + "adj Node " + _adjacentNode [curCheckIdx].adjNodeIdx + " ," +  _adjacentNode [curCheckIdx].relativeAngle);

			// update current checking index

			// when the node is interlocked check for the interlock logic 
			if (_isInterLocked) {
				CheckInterlockNodes (tempRotDegree);

			}

			if (_ControlColor == ButtonColor.Branch) {
				CheckBranchNodes (tempRotDegree);
			} else {
				if (!_isDescending) {
					// check for connection (relative angle + defnite ange)
					if (_adjacentNode.Length > 0) {
						if (_adjacentNode [curCheckIdx].adjNodeIdx >= 0) {
							// if the node has dependency on other nodes

							// check relative angle && get the next node information 
							PathNode adjNode = _myNetWork.FindNodeWithIndex (_adjacentNode [curCheckIdx].adjNodeIdx);
							//print("Current Node " + _nodeIndex + "Current Angle: " + DampAngle(tempRotDegree - adjNode.gameObject.transform.localRotation.eulerAngles.z));
							if (Mathf.Abs (DampAngle (tempRotDegree - adjNode.gameObject.transform.localRotation.eulerAngles.z)
								- DampAngle (_adjacentNode [curCheckIdx].relativeAngle)) <= errorVal * 20f) {
								if (!_isCorrectConnection) {
									//print ("###raise sound effect");
									Events.G.Raise (new PathConnectedEvent ());
								}
								_isCorrectConnection = true;
								//adjNode._isCorrectConnection = true;
								if (_shaderGlowCustom == null) {
									if (_cylinderRenderer) {
										_cylinderRenderer.GetComponent<Renderer> ().material = _GreenMat;
									}
								} else {
									_shaderGlowCustom.ConnectionIsTrue (_isCorrectConnection);
								}
							} else {
								_isCorrectConnection = false;
								if (_shaderGlowCustom != null) {
									_shaderGlowCustom.ConnectionIsTrue (_isCorrectConnection);
								}
								if (_cylinderRenderer) {
									_cylinderRenderer.GetComponent<Renderer> ().material = _originMat;
								}

							}
						} else {
							//print("node angle check: " + _nodeIndex + " " +DampAngle(DampAngle(tempRotDegree) - DampAngle(_adjacentNode [curCheckIdx].relativeAngle)));
							if (Mathf.Abs (DampAngle(tempRotDegree) - DampAngle(_adjacentNode [curCheckIdx].relativeAngle)) <= errorVal * 20f) {
								if (!_isCorrectConnection) {
									Events.G.Raise (new PathConnectedEvent ());
									//Debug.Log ("YOU");
								}
								_isCorrectConnection = true;
								if (_shaderGlowCustom == null) {
									if (_cylinderRenderer) {
										_cylinderRenderer.GetComponent<Renderer> ().material = _GreenMat;
									}
								} else {
									_shaderGlowCustom.ConnectionIsTrue (_isCorrectConnection);
								}
							} else {
								_isCorrectConnection = false;
								if (_shaderGlowCustom != null) {
									_shaderGlowCustom.ConnectionIsTrue (_isCorrectConnection);
								}
								if (_cylinderRenderer) {
									_cylinderRenderer.GetComponent<Renderer> ().material = _originMat;
								}
							}
						}
						_myNodeInfo.isConnected = _isCorrectConnection;
					}
				
				}
			}

			// temp disable turning when dragging 
			if (_isCorrectConnection) {

				if (isDragStart && isTempDisableActive && !isTempDisable) {
					// call disable rotating for this node 
					DisableRotate (0.4f);
					Events.G.Raise (new MBNodeConnect (_nodeIndex));
				}


				// when correctly connected snap to that angle + disable drag for 1 sec 


			} else {
				// when disconnected from the correct angle, enable temp disable 
				if (!isTempDisableActive && disableRotateTimer.IsOffCooldown && !isTempDisable) {
					isTempDisableActive = true;
				}

				// when hop on to node that in and out angles are different s
				if(isTempDisable && !isTempDisableActive){
					isTempDisable = false;
					//disableRotateTimer.IsOffCooldown = true;
					isTempDisableActive = true;

					
				}
			}

		}

	}

	public void InterlockNodeStateHandle(InterlockNodeStateEvent e){
		//print ("Interlock State Check: " + _nodeIndex + " " + e.Unlock);
		// TODO: for one with multiple index
		if (e.SendFrom != _nodeIndex && e.SendTo == _nodeIndex) {
			_isControlActive = e.Unlock;
		}

	}

	// read functions 
	public NodeInfo readNodeInfo(){
		return _myNodeInfo;
	}

	void SetPathEventHandle(SetPathNodeEvent e){
		if (e.NodeIdx == _nodeIndex) {
			// set next path active 

			_isDancerOnBoard = false;
			print("seg : " + e.NodeIdx + "seg count: " +_segCount);
			if (_curSegIdx + 1 < _segCount) {
				_curSegIdx += 1;
				// update node info --> move on to the next spline
				_myNodeInfo.activeSegIdx = _curSegIdx;
			}

		}
	}

	void DancerFinishPathHandle(DancerFinishPath e){
		if (e.NodeIdx == _nodeIndex) {
			//_isCorrectConnection = false;
		}
	}


	void DancerOnBoardHandle(DancerOnBoard e){
		if (e.NodeIdx == _nodeIndex) {
			_isDancerOnBoard = true;
			//_isCorrectConnection = true;

		}
	}

	void ClickWithMouse(){
		
		//Vector3 forward = Camera.main.transform.TransformDirection (Vector3.forward);
//		if (_isInterLocked && _intersectionPart!= null && _intersectionPart.transform.parent != transform.parent) {
//			Debug.Log ("change parent");
//			_intersectionPart.transform.parent = transform.parent;
//		}
		if(Input.GetMouseButtonDown(0) && _ControlColor != ButtonColor.None){
			Ray mousePositionRay;
			mousePositionRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			//dragPreviousMousePos = Input.mousePosition;
			bool isHit = Physics.Raycast (mousePositionRay, out hit);
			if (isHit && hit.collider.gameObject.tag == "RotateCircle") {
				//print ("git the circle: " + hit.point);
				if(hit.collider.gameObject.GetComponentInParent<PathNode>()._nodeIndex == _nodeIndex){
					RotateNode();
					hitDist = hit.distance;
					//Events.G.Raise(new MBTurnColorCircle(_ControlColor, _nodeIndex));
				}
			}
		}
	}

	// TODO: put into camera selection manager or something 
	void RotateWithMouse(){
		

		// start dragging
		if(Input.GetMouseButtonDown(0)){
			
			Ray mousePositionRay;
			if (!_isNotebook) {
				mousePositionRay = Camera.main.ScreenPointToRay (Input.mousePosition);
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
			//Debug.Log (hit.collider.gameObject.name + ": this is the tag");
			if (isHit && hit.collider.gameObject.tag == "RotateCircle") {
				if(hit.collider.gameObject.GetComponentInParent<PathNode>()._nodeIndex == _nodeIndex){

					isDragStart = true;
					dragStartPos = hit.point;
					circlePlane = new Plane (Vector3.up, hit.point);
					preAxis = new Vector3 (0, 0, 0);
					preChangingTime = -1;

					if (_isControlActive) {
						if (_isInterLocked && _intersectionPart!= null ) {
							//Debug.Log ("Click on Node: " + _nodeIndex + " changing intersection");
							_intersectionPart.transform.parent = gameObject.transform;
						}

						if (_activeIntersections != null && _activeIntersections.Count > 0) {
							//Debug.Log ("Click on Node: " + _nodeIndex + " changing intersection");
							foreach(GameObject itsc in _activeIntersections){
								itsc.transform.parent = gameObject.transform;
							}
						}

						//isDragStart = true;
						if (_shaderGlowCustom != null) {
							_shaderGlowCustom.lightOn ();
						}
						Events.G.Raise (new PathGlowEvent (isDragStart));
//						dragStartPos = hit.point;
//						//print ("hit point :" + hit.point);
//						hitDist = hit.distance;
//						// create a plane ;
//						circlePlane = new Plane (Vector3.up, hit.point);
//						preAxis = new Vector3 (0, 0, 0);
//						preChangingTime = -1;
					} else {
						// lock behaviour  + sound
						// TODO: if the node is locked, after dragging larger than 5degrees, go back to the original position 
						Debug.Log("Not contorl active");
						lastRestAngle = transform.localRotation;
						isTempDisable = false;
						Events.G.Raise (new PathNodeStuckEvent ());
						isShaking = true;
					}

					//debugPos1 = hit.point;
				}

			}
		}

		// end dragging: angle snap
		if (Input.GetMouseButtonUp (0)) {
			if (isDragStart) {
				isDragStart = false;
				if (_isControlActive && !isShaking) {
					if (_shaderGlowCustom != null) {
						_shaderGlowCustom.lightOff ();
					}
					Events.G.Raise (new PathGlowEvent (isDragStart));
					//hitDist = 0;
					accAngle = 0;

					if (_isSnapEnable) {
						float tempAngle = transform.localEulerAngles.z;
						tempAngle = DampAngle (tempAngle);
						float snapAngleDelta = AngleSnapTo (tempAngle) - tempAngle;
						//print ("Release drag final angle: " + AngleSnap (tempAngle));
						isRotating = true;
						originAngle = transform.localRotation;
						finalAngle = transform.localRotation;
						finalAngle = originAngle * Quaternion.Euler (0, 0, snapAngleDelta);

					}


					Events.G.Raise (new MBNodeRotate (_nodeIndex, false, 0));
				} else {
					accAngle = 0;
					transform.localRotation = lastRestAngle;
					isShaking = false;
				}


			}
		}

		if (isDragStart && !isTempDisable) {

			Vector3 curMousePos = Vector3.zero;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float rayDistance;
			if (circlePlane.Raycast (ray, out rayDistance)) {
				curMousePos = ray.GetPoint(rayDistance);
			}

		
			// TODO: z needs to be switched to any customized axis
			Vector3 va = Vector3.Normalize(dragStartPos - gameObject.transform.position);
			va.y = 0;
			Vector3 vb = Vector3.Normalize (curMousePos - gameObject.transform.position);
			vb.y = 0;
			//print ("z pos chack: " + (va.z - vb.z));
			//rotate from b to a
			rotateAxis = Vector3.Normalize(Vector3.Cross (vb, va));
			//print ("Debug: rotate axis " + rotateAxis);

			// remove rotate jitter (changing axis direction abruptly)
			if(rotateAxis != preAxis){
				float deltaChangeTime = Time.time - preChangingTime;

				if (deltaChangeTime <= 0.4f) {
					//print("############# Axis Jitter !!!!!");
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
			print ("Angle Check: " + angle);
			if (rotateAxis.y > 0f) {
				rotateAxis = Vector3.back;
			} else {
				rotateAxis = Vector3.forward;
			}
			if (_isControlActive && !isShaking) {
				if (accAngle >= _dragSensitivity) {
					accAngle = _dragSensitivity;

					gameObject.transform.Rotate (-accAngle * rotateAxis * 0.5f, Space.Self);
					dragStartPos = curMousePos;
					accAngle = 0;

					Events.G.Raise (new MBNodeRotate (_nodeIndex, true, 0));
				} 
				
			} else {
				// when contorl is not active 
				Debug.Log("### not rotate angle check: " + angle * 0.1);
				gameObject.transform.Rotate (-angle * rotateAxis * 0.05f, Space.Self);
				dragStartPos = curMousePos;
				if(accAngle >= 50f){
					// shake node back to normal 
					isDragStart = false;
					gameObject.transform.localRotation = lastRestAngle;
					accAngle = 0;
					isShaking = false;
					
				}
			}




		}// end of dragging code 



	}



	void DisableRotate(float stopseconds){
		if (isTempDisableActive) {
			isTempDisableActive = false;
			isTempDisable = true;
			disableRotateTimer.CooldownTime = stopseconds;
			disableRotateTimer.Reset ();
			//isDragStart = false;
			//Events.G.Raise (new MBNodeConnect (_nodeIndex));
			Events.G.Raise (new MBNodeRotate (_nodeIndex, false, 0));
		}
	}


	public void SetAngleSnapWhenMouseUp(bool val){
		if (_isSnapEnable != val) {
			_isSnapEnable = val;
		}
	}
	// snap rotation angle to the clock --> use in the clock puzzle 
	float AngleSnap(float angle){
		float subDeg = 360/36;
		// round the angle to the next subdivision point 
		float remainder = angle%subDeg;
		if (remainder >= subDeg / 2) {
			return angle + subDeg - remainder;
		} else {
			return angle - remainder;
		}

		return angle;

	}

	float AngleSnapTo(float angle){
		// if the nearest snap to angle is found 
		float matchAngle = 0;
		foreach (float snp in snapToAngle) {
			if (Mathf.Abs (DampAngle (angle) - DampAngle (snp)) <= 30f) {
				matchAngle = DampAngle (snp);
				return matchAngle;
			}
		}
			
		return AngleSnap(angle);
	}

	// map angle to [0,2*PI)
	float DampAngle(float angle){
		if (angle >= 360) {
			angle -= 360;
			return DampAngle (angle);
		} else if (angle < 0) {
			angle += 360;
			return DampAngle (angle);
		} else {
			return angle;
			//break;
		}

		//return angle;
	}

//	void PlayModeHandle(MBPlayModeEvent e){
//		_myPlayMode = e.activePlayMode;
//	}

	public ButtonColor GetControlColor(){
		return _ControlColor;
	}

	public void resetAdjNodeAngle(int idx, float angle){
		_adjacentNode [idx].relativeAngle = angle;
		
	}

	// Check if node will descend 
	void CheckDescend(){
		if (_ControlColor == ButtonColor.Descend && _isDancerOnBoard && !_isDescended && !_isDescending) {
			_isDescending = true;
			_finalDescendPos = transform.localPosition;
			_finalDescendPos.z += 5f;
		}
	}

	void DescendNode(){
		if (Vector3.Distance (transform.localPosition, _finalDescendPos) > errorVal) {
			Vector3 tempPos = transform.localPosition;
			tempPos.z += 2 * Time.deltaTime;
			transform.localPosition = tempPos;
		} else {
			transform.localPosition = _finalDescendPos;
			_isDescended = true;
			_isDescending = false;
		}
	}




}
