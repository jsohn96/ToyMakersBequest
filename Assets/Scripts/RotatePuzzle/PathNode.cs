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

}
