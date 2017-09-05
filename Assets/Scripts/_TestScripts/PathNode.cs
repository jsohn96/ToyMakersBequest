using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonColor{
	Red = 0,
	Yellow,
	Brown
};

public struct NodeInfo{
	public Vector3 startPos;
	public Vector3 endPos;
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

}

public class PathNode : MonoBehaviour {


	[SerializeField] ButtonColor _ControlColor;      // the color of the button that controls the node 
	float _rotateAngle;
	[SerializeField] int _nodeIndex;
	Transform _nodeTransform;
	[SerializeField] float _assignedDegree;          // the correct rotation  
	[SerializeField] Material _GreenMat;
	Material _originMat;

	// public variables for path manager  // need optimization --> struct??


	Vector3 _startPos;
	Vector3 _endPos;
	public bool _isCorrectConnection = false;               // if the path is correctly connected 

	NodeInfo _myNodeInfo;
	//



	// Use this for initialization
	void Awake () {
		_nodeTransform = gameObject.transform;
		_originMat = gameObject.GetComponent<Renderer> ().material;
		initNodePathInfo ();

	}

	void initNodePathInfo(){
		// register the start and end 
		Vector3 tempPos = transform.position;
		float length = transform.localScale.x;
		tempPos.x += length / 2;
		_startPos = tempPos;
		tempPos.x -= length/2;
		_endPos = tempPos;
		Vector3 tempDir = new Vector3(1, 1, 1);
		//tempDir = Vector3.Normalize ();
		_myNodeInfo = new NodeInfo(tempDir, _startPos, _endPos, _nodeIndex, false);

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
		if (tempRotDegree == _assignedDegree) {
			_isCorrectConnection = true;
			gameObject.GetComponent<Renderer> ().material = _GreenMat;
		} else {
			_isCorrectConnection = false;
			gameObject.GetComponent<Renderer> ().material = _originMat;
		}
		//update node info 
		_myNodeInfo.isConnected = _isCorrectConnection;
		//print ("Current Angle " + _ControlColor.ToString () +":" + tempRotDegree);
	}

	// read functions 
	public NodeInfo readNodeInfo(){
		return _myNodeInfo;
	}

}
