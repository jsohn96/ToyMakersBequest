using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonColor{
	Red = 0,
	Yellow,
	Brown,
	None
};

// passing data to the path network 
public struct NodeInfo{
	public Path[] paths;
	public int index;
	public bool isConnected;
	public int activeSegIdx;
	public NodeInfo(Path[] p, int i, bool iscnnt, int actseg){
		paths = p;
		index = i;
		isConnected = iscnnt;
		activeSegIdx = actseg;
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

	Vector3 _startPos;
	Vector3 _endPos;
	public bool _isCorrectConnection = false;               // if the path is correctly connected 



	NodeInfo _myNodeInfo;
	//Vector3[] _myPathPos;
	Path[] _myPaths;
	int _segCount;
	int _curSegIdx = 0;

	// mouse drag rotate 
	bool isDragStart = false;
	Vector3 dragStartPos;
	Vector3 dragPreviousMousePos;
	float hitDist;
	float accAngle = 0; // accumulated angle
	int circleDivision = 12; // default as the clock 

	void OnEnable(){
		Events.G.AddListener<SetPathNodeEvent> (SetPathEventHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<SetPathNodeEvent> (SetPathEventHandle);
	}


	// Use this for initialization
	void Awake () {
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

		// debug check 
//		print("Angle check 1: " + DampAngle(540));
//		print("Angle check 2: " + DampAngle(-270));
//		print ("Angle check 3: " + DampAngle (180));


	}

	void initNodePathInfo(){
		//get the positions of the path links 	

		// TODO store the position value for different path segments 
		if (GetComponentsInChildren<Path> () != null) {
			_myPaths = GetComponentsInChildren<Path> ();
			_segCount = _myPaths.Length;
			print ("Check: path number in total " + _segCount);
		} else {
			print ("ERROR: need to construct path first");
		}

		if (_assignedDegree.Length != _segCount) {
			print ("ERROR: wrong assigned angles numbers, \ncheck Path No. " + _nodeIndex);
		}

		_myNodeInfo = new NodeInfo(_myPaths, _nodeIndex, _isCorrectConnection, _curSegIdx);
		//print ("Path position chaeck " + _myNodeInfo.paths[0].ToString());

	}
	
	// Update is called once per frame
	void Update () {
		// Controls for the circle rotation
		ButtonControlListener ();
		RotateWithMouse ();
		
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
		tempRotDegree = transform.rotation.eulerAngles.z;
		if (tempRotDegree == _assignedDegree[_curSegIdx]) {
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
		//print ("Current Angle " + _ControlColor.ToString () +":" + tempRotDegree);
	}

	// read functions 
	public NodeInfo readNodeInfo(){
		return _myNodeInfo;
	}

	void SetPathEventHandle(SetPathNodeEvent e){
		if (e.NodeIdx == _nodeIndex) {
			// set next path active 
			if (_curSegIdx + 1 < _segCount) {
				
				_curSegIdx += 1;
				// update node info
				_myNodeInfo.activeSegIdx = _curSegIdx;

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
