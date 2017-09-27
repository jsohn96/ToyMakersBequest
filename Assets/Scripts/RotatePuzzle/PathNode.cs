using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonColor{
	Red = 0,
	Yellow,
	Brown,
	None

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
	

public class PathNode : MonoBehaviour {
	[SerializeField] ButtonColor _ControlColor;      // the color of the button that controls the node 
	float _rotateAngle;
	[SerializeField] int _nodeIndex;
	Transform _nodeTransform;
	[SerializeField] float[] _assignedDegree;          // the correct rotation  
	[SerializeField] Material _GreenMat;
	[SerializeField] Renderer _cylinderRenderer;
	Material _originMat;
	//
	[SerializeField] AdjacentNode[] _adjacentNode;              // the adjacent node index and angle
	bool _isDancerOnBoard = false;                                      // 1 - on, 0-off    
	 
	Vector3 _startPos;
	Vector3 _endPos;
	public bool _isCorrectConnection = false;               // if the path is correctly connected 
	bool _isInterLocked = false;                            // if the path node has dependency on others 

	PathNetwork _myNetWork;
	NodeInfo _myNodeInfo;
	//Vector3[] _myPathPos;
	//Path[] _myPaths;
	BezierSpline[] _mySplines;
	int _segCount;
	int _curSegIdx = 0;
	//float _duration = 10f;
	//float progress = 0;


	// mouse drag rotate 
	bool isDragStart = false;
	Vector3 dragStartPos;
	Vector3 dragPreviousMousePos;
	float hitDist;
	float accAngle = 0; // accumulated angle
	int circleDivision = 12; // default as the clock 
	bool ischeckingConnection = false;


	void OnEnable(){
		Events.G.AddListener<SetPathNodeEvent> (SetPathEventHandle);
		Events.G.AddListener<DancerFinishPath> (DancerFinishPathHandle);
		Events.G.AddListener<DancerOnBoard> (DancerOnBoardHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<SetPathNodeEvent> (SetPathEventHandle);
		Events.G.RemoveListener<DancerFinishPath> (DancerFinishPathHandle);
		Events.G.RemoveListener<DancerOnBoard> (DancerOnBoardHandle);
	}


	// Use this for initialization
	void Awake () {
		_myNetWork = GetComponentInParent<PathNetwork> ();
		_nodeTransform = gameObject.transform;
		if (_ControlColor != ButtonColor.None) {
			_originMat = _cylinderRenderer.GetComponent<Renderer> ().material;
		}


		// static node 
		if (_ControlColor == ButtonColor.None) {
			_isCorrectConnection = true;
		} else {
			_isCorrectConnection = false;
		}
		// 
		initNodePathInfo ();

	}

	void initNodePathInfo(){
		//get the positions of the path links 	

		// TODO store the position value for different path segments 
		if (GetComponentsInChildren<BezierSpline> () != null) {
			_mySplines = GetComponentsInChildren<BezierSpline> ();
			_segCount = _mySplines.Length;
			print ("Check: path number in total " + _segCount);
		} else {
			print ("ERROR: need to construct path first");
		}

		if (_ControlColor != ButtonColor.None && _adjacentNode.Length != _segCount * 2) {
			print ("ERROR: wrong assigned angles numbers, \ncheck Path No. " + _nodeIndex);
		} else {
			_myNodeInfo = new NodeInfo(_mySplines, _nodeIndex, _isCorrectConnection, _curSegIdx, _adjacentNode, _ControlColor);
		}


		//print ("Path position chaeck " + _myNodeInfo.paths[0].ToString());

	}
	
	// Update is called once per frame
	void Update () {
		// Controls for the circle rotation
		ButtonControlListener ();
		RotateWithMouse ();

		
	}

	void FixedUpdate(){
		CheckNodeConnection ();
//		if (ischeckingConnection) {
//			CheckNodeConnection ();
//		}

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
		float tempRotDegree = transform.rotation.eulerAngles.z;
		transform.Rotate(0,0,-90);

	}


	void CheckNodeConnection(){
		if (_ControlColor != ButtonColor.None) {
			float tempRotDegree = transform.localRotation.eulerAngles.z;
			int curCheckIdx = 0;
			if (!_isDancerOnBoard) {
				//print ("Check In ");
				curCheckIdx = _curSegIdx * 2;
			} else {
				curCheckIdx = _curSegIdx * 2 + 1;
				//print ("Check Out ");
			}

			//print("Current Node " + _nodeIndex + "adj Node " + _adjacentNode [curCheckIdx].adjNodeIdx + " ," +  _adjacentNode [curCheckIdx].relativeAngle);

			// update current checking index

			if (_adjacentNode [curCheckIdx].adjNodeIdx >= 0) {
				// if the node has dependency on other nodes

				// check relative angle && get the next node information 
				PathNode adjNode = _myNetWork.FindNodeWithIndex (_adjacentNode [curCheckIdx].adjNodeIdx);
				print("Current Node " + _nodeIndex + "Current Angle: " + DampAngle(tempRotDegree - adjNode.gameObject.transform.localRotation.eulerAngles.z));
				if (DampAngle(tempRotDegree - adjNode.gameObject.transform.localRotation.eulerAngles.z) 
					== DampAngle(_adjacentNode [curCheckIdx].relativeAngle)) {
					_isCorrectConnection = true;
					//adjNode._isCorrectConnection = true;
					if (_cylinderRenderer) {
						_cylinderRenderer.GetComponent<Renderer> ().material = _GreenMat;
					}
				} else {
					_isCorrectConnection = false;
					if (_cylinderRenderer) {
						_cylinderRenderer.GetComponent<Renderer> ().material = _originMat;
					}

				}
			} else {
				if ( DampAngle(tempRotDegree) ==  DampAngle(_adjacentNode [curCheckIdx].relativeAngle)) {
					_isCorrectConnection = true;
					//_isDancerOnBoard = true;
					if (_cylinderRenderer) {
						_cylinderRenderer.GetComponent<Renderer> ().material = _GreenMat;
					}
				} else {
					_isCorrectConnection = false;
					if (_cylinderRenderer) {
						_cylinderRenderer.GetComponent<Renderer> ().material = _originMat;
					}

				}
			}

			//update node info 
			_myNodeInfo.isConnected = _isCorrectConnection;
			//print ("Current Angle " + _ControlColor.ToString () +":" + tempRotDegree);
		
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
			if (_curSegIdx + 1 < _segCount) {
				_curSegIdx += 1;
				// update node info --> move on to the next spline
				_myNodeInfo.activeSegIdx = _curSegIdx;

				// reset connection status
				if (_assignedDegree [_curSegIdx] != _assignedDegree [_curSegIdx - 1]) {
					_isCorrectConnection = false;
					_myNodeInfo.isConnected = _isCorrectConnection;
					if (_cylinderRenderer != null) {
						_cylinderRenderer.material = _originMat;
					}
				}
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
		
	// TODO: put into camera selection manager or something 
	// 需要把代码整理到一个脚本里面，否则每个node要求做一个raycast消费太大
	void RotateWithMouse(){
		//print ("Mouse Axis Check: " + Input.GetAxis ("Mouse Y") + "," + Input.GetAxis ("Mouse X"));
		//float v = Input.GetAxis ("Mouse Y");
		//float h = Input.GetAxis ("Mouse X");

		Vector3 forward = Camera.main.transform.TransformDirection (Vector3.forward);
		//Plane playerPlane = 



		// get the mouse input position
		if(Input.GetMouseButtonDown(0)){
			Ray mousePositionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			dragPreviousMousePos = Input.mousePosition;
			bool isHit = Physics.Raycast (mousePositionRay, out hit);
			if (isHit && hit.collider.gameObject.tag == "RotateCircle") {
				//print ("git the circle: " + hit.point);
				if(hit.collider.gameObject.GetComponentInParent<PathNode>()._nodeIndex == _nodeIndex){
					isDragStart = true;
					dragStartPos = hit.point;
					hitDist = hit.distance;
				}

			}

			//print ("Mouse Position Check: " + mouseInWorldPos);
		}

		if (Input.GetMouseButtonUp (0)) {
			if (isDragStart) {
				isDragStart = false;
				hitDist = 0;
				accAngle = 0;
				//
				// check if angle correct 

				// get the final rotation and then turn? 
				float tempAngle = gameObject.transform.rotation.eulerAngles.z;
				// dont need this step 
				tempAngle = DampAngle (tempAngle);
				float targetAngle = AngleSnap (tempAngle);
				print("Final angle check: " + tempAngle + "," + targetAngle);

				if (targetAngle == _assignedDegree[_curSegIdx]) {
					_isCorrectConnection = true;
					if (_cylinderRenderer) {
						_cylinderRenderer.GetComponent<Renderer> ().material = _GreenMat;
					}
				} else {
					_isCorrectConnection = false;
					if (_cylinderRenderer) {
						_cylinderRenderer.GetComponent<Renderer> ().material = _originMat;
					}

				}
				//update node info 
				_myNodeInfo.isConnected = _isCorrectConnection;
			}
		}

		if (isDragStart) {
			Vector3 curMousePos;
			curMousePos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, hitDist));
			// TODO: z needs to be switched to any customized axis
			Vector3 va = Vector3.Normalize(dragStartPos - gameObject.transform.position);
			va.z = 0;
			Vector3 vb = Vector3.Normalize (curMousePos - gameObject.transform.position);
			vb.z = 0;
			//print ("z pos chack: " + (va.z - vb.z));
			//rotate from b to a
			Vector3 rotateAxis = Vector3.Normalize(Vector3.Cross (vb, va));

			// get the angle 
			float angle = Mathf.Acos(Vector3.Dot(va, vb))*Mathf.Rad2Deg;
			accAngle += angle;
			//print ("Angle Check: " + accAngle);
			if (accAngle >= 0f) {
				Quaternion tempRot = Quaternion.Euler (-accAngle*rotateAxis);
				print ("Temp rot: " + tempRot);
				Quaternion curRot = gameObject.transform.rotation;
				//curRot = curRot + tempRot;
				gameObject.transform.Rotate(-accAngle*rotateAxis*0.5f, Space.World);
				dragStartPos = curMousePos;
				accAngle = 0;
			}
				
		}


	}

	// snap rotation angle to the clock --> use in the clock puzzle 
	float AngleSnap(float angle){
		float subDeg = 360/12;
		// round the angle to the next subdivision point 
		float remainder = angle%subDeg;
		if (remainder >= subDeg / 2) {
			return angle + subDeg - remainder;
		} else {
			return angle - remainder;
		}

		return angle;
			
	}

	// map angle to [0,2*PI)
	float DampAngle(float angle){
		if (angle >= 360) {
			angle -= 360;
			DampAngle (angle);
		} else if (angle < 0) {
			angle += 360;
			DampAngle (angle);
		} else {
			return angle;
			//break;
		}

		return angle;
	}



}
