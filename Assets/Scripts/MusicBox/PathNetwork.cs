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
	gate_opened,
	first_encounter_TM,
	flip_TM_stage,
	hold_hand_with_TM,
	descend_to_layer_two,
	temp_end_scene,
	activateInterlockNode,
	enter_pond,
	pond_loop,
	move_on_seesaw,
	activate_pond,
	TM_move_to_location,
	MB_Stage_EnterPondScene,
	MB_Stage_EnterPlayScene
}


public class PathNetwork : MonoBehaviour {
	// the solution for the puzzle --> the order of the correct path 
	[SerializeField] int _startIndex;
	[SerializeField] PathOrder[] _correctOrder;

	PathNode[] _myNodes;

	int _myNodesLength = 0;
	[SerializeField] Dancer _myDancer;
	int _curNodeIdx;
	int _orderIdx = 0;
	public PathNode _curNode{ get; set;}
	bool _isCheckingNext = false;
	bool _isDancerFinishPath = false;
	int _curCheckIdx = 0;
	int _nxtCheckIdx = -1;
	bool _isStartPath = false;
	bool _isPathPause = false;
	bool _isPathLoop = false;

	int _loopFromIdx = -1;
	int _loopToIdx = -1;

	PlayMode _myPlayMode;


	// 
	[SerializeField] bool _isActive = false;
	bool _skipGatePause = false;

	// Use this for initialization
	void Awake () {
		_myNodes = GetComponentsInChildren<PathNode> ();
		_myNodesLength = _myNodes.Length;
		print ("Init Info: " + "\nNode Count"+ _myNodesLength);
		// init player position 
		_orderIdx = _startIndex;
		_curNodeIdx = _correctOrder[_orderIdx].index;
	}

	void OnEnable(){
		Events.G.AddListener<DancerFinishPath> (HandleDancerFinishPath);
		Events.G.AddListener<PathResumeEvent> (PathResumeHandle);
		Events.G.AddListener<MBPlayModeEvent> (PlayModeHandle);
		Events.G.AddListener<InterlockNodeStateEvent> (InterlockNodeStateHandle);
		Events.G.AddListener<MBPathIndexEvent> (PathIndexHandle);
		Events.G.AddListener<MBExitPondLoop> (ExitLoopHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<DancerFinishPath> (HandleDancerFinishPath);
		Events.G.RemoveListener<PathResumeEvent> (PathResumeHandle);
		Events.G.RemoveListener<MBPlayModeEvent> (PlayModeHandle);
		Events.G.RemoveListener<InterlockNodeStateEvent> (InterlockNodeStateHandle);
		Events.G.RemoveListener<MBPathIndexEvent> (PathIndexHandle);
		Events.G.RemoveListener<MBExitPondLoop> (ExitLoopHandle);
	}

	public PathNode GetNodeOfIndex(int index){
		if (index >= _myNodesLength || index < 0) {
			return null;
		} else {
			return _myNodes [index];
		}
	}


	
	// Update is called once per frame
	void Update () {
		///
		if(_isCheckingNext && _isActive && !_isPathPause){
			CheckNextIdxUpdate ();
			PathNode tempNode = FindNodeWithIndex (_curNodeIdx);
			print ("Check connection for " + _curNode.readNodeInfo().index + ":" + _curNode.readNodeInfo ().isConnected
				+ "and " +_curNodeIdx + ":" + tempNode.readNodeInfo ().isConnected);

			if (tempNode.readNodeInfo ().isConnected && _curNode.readNodeInfo().isConnected) {
				// music manager continues to play music 
				Events.G.Raise (new MBMusicMangerEvent (true));
				// icc the past pathnode --> active path +1, isOnboard = false
				print("get off node: " + _curNode.readNodeInfo ().index);
				Events.G.Raise (new SetPathNodeEvent (_curNode.readNodeInfo ().index));
				_curNode = tempNode;
				// when there is a path on the current node
				_myDancer.SetNewPath (_curNode);
				// set the dancer onboard of the new one || //
				//TODO: or when interlocked node, it means it switches to the next state
				Events.G.Raise (new DancerOnBoard (_curNode.readNodeInfo ().index));
				// check the 
				_isCheckingNext = false;

			} else {
				//print ("next node not correctly connected");

			}
		}
		
	}

	// todo Tell the dancer where she should start
	// have the dancer get a copy of the info about the path on the node 
	// have the dancer reports back to the network about her status
	public void PositionDancer (){
		// pass down the game object
		print("Current node in poisitondancer:"+ _curNodeIdx);

		_curNode = FindNodeWithIndex (_curNodeIdx);

		_myDancer.SetNewPath (_curNode);
		Events.G.Raise (new DancerOnBoard (_curNode.readNodeInfo ().index));
		print ("dance placed on " + _curNode.readNodeInfo ().index);

	}


	// 
	void PathResumeHandle(PathResumeEvent e){
		if (_curNodeIdx == 6) {
			_skipGatePause = true;
		}
		if (_isPathPause) {
			_isPathPause = false;
			CheckNextIdx ();
		}
	}


	// When dancer finishes the current path, request to check the next connection
	void HandleDancerFinishPath(DancerFinishPath e){
		//print ("Check next available node");
		if (_isActive && _correctOrder [_orderIdx].index==e.NodeIdx) {
			//print(Time.time + " Finish path on : " + e.NodeIdx + ";" + _orderIdx);

			// check if there is anyevent envoked when the path is finished 
			if (_correctOrder [_orderIdx].nameOfEvent == PathState.none) {
				Events.G.Raise (new MBMusicMangerEvent (false));
				CheckNextIdx ();
			} else {
				Events.G.Raise (new PathStateManagerEvent (_correctOrder [_orderIdx].nameOfEvent));
				_isPathPause = true;
				//print (Time.time + "End of Path " + _correctOrder [_orderIdx].nameOfEvent + " " + _correctOrder[_orderIdx].index + " " + _orderIdx);
				if (_correctOrder [_orderIdx].nameOfEvent == PathState.open_gate) {
					if (!_skipGatePause) {
						Events.G.Raise (new MBMusicMangerEvent (false));
					} else {
						_isPathPause = false;
						CheckNextIdx ();
					}
				} else if (_correctOrder [_orderIdx].nameOfEvent == PathState.pond_loop) {
					
					if (!_isPathLoop) {
						_isPathLoop = true;
						_loopFromIdx = _orderIdx;
						_loopToIdx = _orderIdx + 6;
						print ("enter loop from to: " + _loopToIdx);
						PathNode tempNode = FindNodeWithIndex (_correctOrder [_orderIdx].index);
						tempNode.resetAdjNodeAngle (0, 90f);

					}

					_isPathPause = false;
					CheckNextIdx ();
				} else if (_correctOrder [_orderIdx].nameOfEvent == PathState.TM_move_to_location) {
					_isPathPause = false;
					CheckNextIdx ();
				} else if (_correctOrder [_orderIdx].nameOfEvent == PathState.MB_Stage_EnterPlayScene) {
					_isPathPause = false;
					CheckNextIdx ();
				}else if (_correctOrder [_orderIdx].nameOfEvent == PathState.MB_Stage_EnterPondScene) {
					_isPathPause = false;
					CheckNextIdx ();
				}

			}
		}




	}

	// util functions 
	public PathNode FindNodeWithIndex(int i){
		PathNode result = _myNodes[0];
		foreach (PathNode _pn in _myNodes) {
			//print ("find node idx: " + i);
			NodeInfo ninfo = _pn.readNodeInfo ();
			if (ninfo.index == i) {
				result = _pn;
			} 
		}


		return result;


	}

	public void SetPathActive(bool isActive){
		_isCheckingNext = false;
		UpdateNodes ();
		_isActive = isActive;
		if (_isActive) {
			print ("Path Activation");
			PositionDancer ();
			Events.G.Raise (new DancerChangeMoveEvent (DancerMove.none));
			Events.G.Raise (new MBMusicMangerEvent (true));
			_isPathPause = false;

		}
	}

	public void UpdateNodes(){
		//_myNodes = new PathNode();
		_myNodes = GetComponentsInChildren<PathNode> ();
		_orderIdx = _startIndex;
		//_nxtCheckIdx = -1;
		_curNodeIdx = _correctOrder[_orderIdx].index;
		print ("Start from: " + _curNodeIdx);
	}

	void PlayModeHandle(MBPlayModeEvent e){
		_myPlayMode = e.activePlayMode;
	}

	void InterlockNodeStateHandle(InterlockNodeStateEvent e){
		
	}

	// for loop in the path

	void EnterLoop(int fromIdx, int toIdx){
		_isPathLoop = true;
		_loopFromIdx = fromIdx;
		_loopToIdx = toIdx;
		//print ("enter loop from to: " + _loopToIdx);
		//PathNode tempNode = FindNodeWithIndex (_correctOrder [_orderIdx].index);
		//tempNode.resetAdjNodeAngle(0, 90f);
	}

	// loop the next node order between the fromIdx and toIdx
	void LoopBetween(){
		_nxtCheckIdx = _orderIdx;
		if (_nxtCheckIdx + 1 > _loopToIdx) {
			_nxtCheckIdx = _loopFromIdx;
		} else {
			_nxtCheckIdx  = -1;
		}

	}

	void JumpToIndex(int idx){
		if (idx < _correctOrder.Length && idx >= 0) {
			_nxtCheckIdx = idx;
		}
	}

	void CheckNextIdxUpdate(){		
		if (_isPathLoop) {
			LoopBetween ();
		}
		//print("next index candidate " + _nxtCheckIdx);
		if (_nxtCheckIdx >= 0 && _nxtCheckIdx < _correctOrder.Length ) {
			_orderIdx = _nxtCheckIdx;
			_curNodeIdx = _correctOrder [_orderIdx].index;
		}
		_nxtCheckIdx = -1;
	
	}

	// for handling all the different senarios for getting the next node index 
	// call after path finished && after any end of path events finished 
	void CheckNextIdx(){
		if (_isPathLoop) {
			LoopBetween ();
		}
		if (_orderIdx + 1 < _correctOrder.Length && _nxtCheckIdx < 0) {
			_orderIdx += 1;
			_curNodeIdx = _correctOrder [_orderIdx].index;
			// TODO add other senarios 
			_isCheckingNext = true;
		} else if(_nxtCheckIdx > 0){
			_orderIdx = _nxtCheckIdx;
			_curNodeIdx = _correctOrder [_orderIdx].index;
			// TODO add other senarios 
			_nxtCheckIdx = -1;
			_isCheckingNext = true;

		}else if(_orderIdx + 1 < _correctOrder.Length && _nxtCheckIdx < 0){
			print ("success!!");
			// trigger final state 
			Events.G.Raise (new PathCompeleteEvent ());
		}
	}

	//
	void PathIndexHandle(MBPathIndexEvent e){
		JumpToIndex (e.jumpToIndex);
	}

	void ExitLoopHandle(MBExitPondLoop e){
		_isPathLoop = false;
	}


}
