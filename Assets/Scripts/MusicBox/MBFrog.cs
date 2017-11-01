using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  for catching the frog logic 
public class MBFrog : MonoBehaviour {
	[SerializeField] PathNode[] _JumpNode;
	//[SerializeField] int[] _Order;
	[SerializeField] Transform _dancerTrans;
	Transform _frogTransform;
	bool _isDetecting= false;
	bool _isCaught = false;
	bool _isEnterPond = true;
	float _detectDistance = 4f;
	int _curNodeOrderIdx = 0;
	int _loopNodeNum = 3;

	[SerializeField] Transform _pondMain;
	[SerializeField] Transform _ponfSide;

	float _radiusRatioMainToSide = 2;
	float _mainRotateAxis;

	float _originAngle; 


	//
	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (PathStateManageHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (PathStateManageHandle);
	}

	// Use this for initialization
	void Start () {
		_frogTransform = gameObject.transform;
		_curNodeOrderIdx = -1;
		// 
		_originAngle = _pondMain.transform.localEulerAngles.z;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_isDetecting && !_isCaught) {
			FrogDetectDancer ();
		}

		FrogDetectDancer ();

		if (_isEnterPond) {
			GearWorkUpdate ();
		}


	}

	void FrogDetectDancer(){
		// if the current node that the player is step on is the one that's behind then check for the next position 
		// or do it by distance? 
		int tempIdx = -1;
		if(Vector3.Distance(_frogTransform.position, _dancerTrans.position) <= _detectDistance || Input.GetKeyDown(KeyCode.J)){
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
		if(_ponfSide.transform.localEulerAngles.z == 360){
			_isCaught = true;
		}

	}



	void PathStateManageHandle(PathStateManagerEvent e){
		if (e.activeEvent == PathState.enter_pond && !_isEnterPond) {
			_isEnterPond = true;
		}
		
	}
}
