using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonColor{
	Red = 0,
	Yellow,
	Brown
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

	/*
	public Vector3 startPos;
	public Vector3 endPos;
	//public Vector3[] paths;
	public int index;
	public bool isConnected;
	public Vector3 localDirection;
	public NodeInfo(Vector3 dir, Vector3 va, Vector3 vb, int i, bool isconnected){
		localDirection = dir;
		startPos = va;
		endPos = vb;
		index = i;
		isConnected = isconnected;
	}
	*/


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

	void OnEnable(){
		Events.G.AddListener<SetPathNodeEvent> (SetPathEventHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<SetPathNodeEvent> (SetPathEventHandle);
	}


	// Use this for initialization
	void Awake () {
		_nodeTransform = gameObject.transform;
		_originMat = _cylinderRenderer.GetComponent<Renderer> ().material;

		// 
		initNodePathInfo ();
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
		// register the start and end 
//		Vector3 tempPos = transform.position;
//		float length = transform.localScale.x;
//		tempPos.x += length / 2;
//		_startPos = tempPos;
//		tempPos.x -= length/2;
//		_endPos = tempPos;
//		Vector3 tempDir = new Vector3(1, 1, 1);
		//tempDir = Vector3.Normalize ();
		//_myNodeInfo = new NodeInfo(tempDir, _startPos, _endPos, _nodeIndex, false);
		_myNodeInfo = new NodeInfo(_myPaths, _nodeIndex, false, _curSegIdx);
		//print ("Path position chaeck " + _myNodeInfo.paths[0].ToString());

	}
	
	// Update is called once per frame
	void Update () {
		ButtonControlListener ();
		
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
			_cylinderRenderer.GetComponent<Renderer> ().material = _GreenMat;
		} else {
			_isCorrectConnection = false;
			_cylinderRenderer.GetComponent<Renderer> ().material = _originMat;
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
					_cylinderRenderer.material = _originMat;

				}
			}

		}
	}

}
