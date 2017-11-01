using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PathOrder{
	public int index;
	public PathState nameOfEvent;
	PathOrder(int idx, PathState noe){
		index = idx;
		nameOfEvent = noe;	
	}
}

public enum PathState{
	none,
	descend_inital_stage,
	open_gate,
	first_encounter_TM,
	flip_TM_stage,
	hold_hand_with_TM,
	descend_to_layer_two,
	temp_end_scene,
	activateInterlockNode,
	enter_pond
}


public class PathNetwork : MonoBehaviour {
	// the solution for the puzzle --> the order of the correct path 
	[SerializeField] int _startIndex;
	[SerializeField] PathOrder[] _correctOrder;

	PathNode[] _myNodes;
	[SerializeField] Dancer _myDancer;
	int _curNodeIdx;
	int _orderIdx = 0;
	PathNode _curNode;
	bool _isCheckingNext = false;
	bool _isDancerFinishPath = false;
	int _curCheckIdx = 0;
	bool _isStartPath = false;
	bool _isPathPause = false;

	PlayMode _myPlayMode;


	// 
	bool _isActive = false;

	// Use this for initialization
	void Awake () {
		_myNodes = GetComponentsInChildren<PathNode> ();
		print ("Init Info: " + "\nNode Count"+ _myNodes.Length);
		// init player position 
		_orderIdx = _startIndex;
		_curNodeIdx = _correctOrder[_orderIdx].index;
	}

	void OnEnable(){
		Events.G.AddListener<DancerFinishPath> (HandleDancerFinishPath);
		Events.G.AddListener<PathResumeEvent> (PathResumeHandle);
		Events.G.AddListener<MBPlayModeEvent> (PlayModeHandle);
		Events.G.AddListener<InterlockNodeStateEvent> (InterlockNodeStateHandle);

	}

	void OnDisable(){
		Events.G.RemoveListener<DancerFinishPath> (HandleDancerFinishPath);
		Events.G.RemoveListener<PathResumeEvent> (PathResumeHandle);
		Events.G.RemoveListener<MBPlayModeEvent> (PlayModeHandle);
		Events.G.RemoveListener<InterlockNodeStateEvent> (InterlockNodeStateHandle);
	}

	void Start(){
		//Events.G.Raise (new DancerChangeMoveEvent (DancerMove.idleDance));
		//PositionDancer ();
	}


	
	// Update is called once per frame
	void Update () {
		///
		if(_isCheckingNext && _isActive && !_isPathPause){
			PathNode tempNode = FindNodeWithIndex (_curNodeIdx);
			print ("Check connection for " + _curNodeIdx + ":" + _curNode.readNodeInfo ().isConnected
				+ "and " +_curNode.readNodeInfo().index + ":" + tempNode.readNodeInfo ().isConnected);

			if (tempNode.readNodeInfo ().isConnected && _curNode.readNodeInfo().isConnected) {
				// music manager continues to play music 
				Events.G.Raise (new MBMusicMangerEvent (true));
				// icc the past pathnode --> active path +1, isOnboard = false
				Events.G.Raise (new SetPathNodeEvent (_curNode.readNodeInfo ().index));
				_curNode = tempNode;
				// when there is a path on the current node
				_myDancer.SetNewPath (_curNode);
				// set the dancer onboard of the new one || //
				//TODO: or when interlocked node, it means it switches to the next state
				Events.G.Raise (new DancerOnBoard (_curNode.readNodeInfo ().index));
				// check the 
				if (_myPlayMode == PlayMode.MBPrototype_With_Path) {
					_isCheckingNext = false;
				} else if (_myPlayMode == PlayMode.MBPrototype2_Without_Path) {
					if (_curNode.readNodeInfo ().paths.Length == 0) {
						_isCheckingNext = true;
					}
				}

			} else {
				print ("next node not correctly connected");

			}
		}

//		if (Input.GetKeyDown (KeyCode.S) && !_isStartPath) {
//			_isStartPath = true;
//			PositionDancer ();
//			Events.G.Raise (new DancerChangeMoveEvent (DancerMove.none));
//
//		}
		
	}

	// todo Tell the dancer where she should start
	// have the dancer get a copy of the info about the path on the node 
	// have the dancer reports back to the network about her status
	void PositionDancer (){
		// pass down the game object

		_curNode = FindNodeWithIndex (_curNodeIdx);
		_myDancer.SetNewPath (_curNode);

	}


	// 
	void PathResumeHandle(PathResumeEvent e){
		if (_isPathPause) {
			_isPathPause = false;
			if (_orderIdx + 1 < _correctOrder.Length) {
				_orderIdx += 1;
				_curNodeIdx = _correctOrder [_orderIdx].index;
				_isCheckingNext = true;
			} else {
				print ("success!!");
				// trigger final state 
				Events.G.Raise (new PathCompeleteEvent ());
			}
		}
	}


	// When dancer finishes the current path, request to check the next connection
	void HandleDancerFinishPath(DancerFinishPath e){
		print ("Check next available node");

		if (_isActive) {
			// check if there is anyevent envoked when the path is finished 
			if (_correctOrder [_orderIdx].nameOfEvent == PathState.none) {
				Events.G.Raise (new MBMusicMangerEvent (false));
				if (_orderIdx + 1 < _correctOrder.Length) {
					_orderIdx += 1;
					_curNodeIdx = _correctOrder [_orderIdx].index;
					_isCheckingNext = true;
				} else {
					print ("success!!");
					// trigger final state 
					Events.G.Raise (new PathCompeleteEvent ());
				}
			} else {
				//print ("End of Path " + _correctOrder [_orderIdx].nameOfEvent);
				if (_correctOrder [_orderIdx].nameOfEvent == PathState.open_gate) {
					Events.G.Raise (new MBMusicMangerEvent (false));
				} else {
					
				}
				Events.G.Raise (new PathStateManagerEvent (_correctOrder [_orderIdx].nameOfEvent));
				_isPathPause = true;
			}
		}




	}

	// util functions 
	public PathNode FindNodeWithIndex(int i){
		foreach (PathNode _pn in _myNodes) {
			NodeInfo ninfo = _pn.readNodeInfo ();
			if (ninfo.index == i) {
				return _pn;
				break;
			} 
		}

		return _myNodes [0];
	}

	public void SetPathActive(bool isActive){
		_isActive = isActive;
		if (_isActive) {
			PositionDancer ();
			Events.G.Raise (new DancerChangeMoveEvent (DancerMove.none));
			Events.G.Raise (new MBMusicMangerEvent (true));
		}
	}

	public void UpdateNodes(){
		//_myNodes = new PathNode();
		_myNodes = GetComponentsInChildren<PathNode> ();
		print ("Update Info: " + "\nNode Count"+ _myNodes.Length);
		// init player position 
		_orderIdx = _startIndex;
		_curNodeIdx = _correctOrder[_orderIdx].index;
	}

	void PlayModeHandle(MBPlayModeEvent e){
		_myPlayMode = e.activePlayMode;
	}

	void InterlockNodeStateHandle(InterlockNodeStateEvent e){
		
	}

	// for loop in the path
	// loop the next node order between the fromIdx and toIdx
	void EnterLoop(int fromIdx, int toIdx){
		
	}


}
