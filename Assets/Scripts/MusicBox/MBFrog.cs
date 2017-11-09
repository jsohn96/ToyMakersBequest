using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  for catching the frog logic 
public class MBFrog : MonoBehaviour {
	[SerializeField] PathNode[] _JumpNode;
	//[SerializeField] int[] _Order;
	[SerializeField] Transform _dancerTrans;
	Transform _frogTransform;
	bool _isDetecting= true;
	bool _isCaught = false;
	bool _isEnterPond = true;
	float _detectDistance = 4f;
	int _curNodeOrderIdx = 0;
	int _loopNodeNum = 3;
	int _dancerOnNodeIdx = -1;
	bool _isNetDown = false;

	[SerializeField] Transform _pondMain;
	[SerializeField] Transform _ponfSide;

	float _radiusRatioMainToSide = 2;
	float _mainRotateAxis;

	float _originAngle; 


	//
	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (PathStateManageHandle);
		Events.G.AddListener<DancerOnBoard> (DancerOnBoardHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (PathStateManageHandle);
		Events.G.RemoveListener<DancerOnBoard> (DancerOnBoardHandle);
	}

	// Use this for initialization
	void Start () {
		_frogTransform = gameObject.transform;
		_curNodeOrderIdx = 0;
		_isDetecting = true;
		// 
		_originAngle = _pondMain.transform.localEulerAngles.z;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_isDetecting && !_isCaught) {
			FrogDetectDancer ();
		}

		//FrogDetectDancer ();

		if (_isEnterPond) {
			GearWorkUpdate ();
			if (_isNetDown && _JumpNode [_curNodeOrderIdx].readNodeInfo ().index == 13) {
				CatchFrog();
				// TODO exit loop 
				Events.G.Raise(new MBExitPondLoop());
			}
		}



	}

	void FrogDetectDancer(){
		// if the current node that the player is step on is the one that's behind then check for the next position 
		// or do it by distance? 
		int tempIdx = -1;
		if(_dancerOnNodeIdx == _JumpNode[_curNodeOrderIdx].readNodeInfo().index){
			if (_curNodeOrderIdx + 1 < _JumpNode.Length) {
				tempIdx = _curNodeOrderIdx + 1;
			} else {
				if (!_isEnterPond) {
					_isEnterPond = true;
				}
				tempIdx = _JumpNode.Length - _loopNodeNum;
			}

			JumpToNextNode(_JumpNode[tempIdx]);
			_curNodeOrderIdx = tempIdx;
		}

	}

	void JumpToNextNode(PathNode pn){
		// move on to the next node 
		print("Jump to Node: " + pn.readNodeInfo().index);
		_frogTransform.position = pn.gameObject.transform.position;
		_frogTransform.parent = pn.gameObject.transform;

	}


	public void CatchFrog(){
		if (!_isCaught) {
			_isCaught = true;
			gameObject.SetActive(false);
			// break out from the network loop 
		}
	}

	// 
	void GearWorkUpdate(){
		//float radiusLength = 0;
		float mainrotateAngle = _pondMain.transform.localEulerAngles.z - _originAngle;
		_originAngle = _pondMain.transform.localEulerAngles.z;
		float sideAngle = -mainrotateAngle * 2;
		Quaternion angle = _ponfSide.transform.localRotation;
		angle = angle * Quaternion.Euler (0, 0, sideAngle);
		_ponfSide.transform.localRotation = angle;

		// check side circle rotation 
//		if(_ponfSide.transform.localEulerAngles.z == 360){
//			_isCaught = true;
//		}

		if (DampAngle (_pondMain.transform.localEulerAngles.z) == DampAngle (-120)) {
			_isNetDown = true;
		} else {
			_isNetDown = false;
		}

	}



	void PathStateManageHandle(PathStateManagerEvent e){
		if (e.activeEvent == PathState.enter_pond && !_isEnterPond) {
			_isEnterPond = true;
		}
		
	}

	void DancerOnBoardHandle(DancerOnBoard e){
		_dancerOnNodeIdx = e.NodeIdx;
		print ("Frog! : the dancer is on : " + _dancerOnNodeIdx + " the frog is on: " + _JumpNode[_curNodeOrderIdx].readNodeInfo().index);
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
