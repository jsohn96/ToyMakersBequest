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
	enter_pond,
	pond_loop,
	move_on_seesaw
}


public class PathNetwork : MonoBehaviour {
	// the solution for the puzzle --> the order of the correct path 
	[SerializeField] int _startIndex;
	[SerializeField] PathOrder[] _correctOrder;

	PathNode[] _myNodes;
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

	// Use this for initialization
	void Awake () {
		_myNodes = GetComponentsInChildren<PathNode> ();
		print ("Init Info: " + "\nNode Count"+ _myNodes.Length);
		// init player position 
		//_orderIdx = _startIndex;
		//_curNodeIdx = _correctOrder[_orderIdx].index;
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

	void Start(){
		//Events.G.Raise (new DancerChangeMoveEvent (DancerMove.idleDance));
		//PositionDancer ();
	}


	
	// Update is called once per frame
	void Update () {
		///
		if(_isCheckingNext && _isActive && !_isPathPause){
			CheckNextIdxUpdate ();
			PathNode tempNode = FindNodeWithIndex (_curNodeIdx);
			//print ("Check connection for " + _curNodeIdx + ":" + _curNode.readNodeInfo ().isConnected
				//+ "and " +_curNode.readNodeInfo().index + ":" + tempNode.readNodeInfo ().isConnected);

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
				//print ("next node not correctly connected");

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
		Events.G.Raise (new DancerOnBoard (_curNode.readNodeInfo ().index));

	}


	// 
	void PathResumeHandle(PathResumeEvent e){
		if (_isPathPause) {
			_isPathPause = false;
			CheckNextIdx ();
		}
	}


	// When dancer finishes the current path, request to check the next connection
	void HandleDancerFinishPath(DancerFinishPath e){
		//print ("Check next available node");

		if (_isActive) {
			// check if there is anyevent envoked when the path is finished 
			if (_correctOrder [_orderIdx].nameOfEvent == PathState.none) {
				Events.G.Raise (new MBMusicMangerEvent (false));
				CheckNextIdx ();
			} else {
				Events.G.Raise (new PathStateManagerEvent (_correctOrder [_orderIdx].nameOfEvent));
				_isPathPause = true;
				//print ("End of Path " + _correctOrder [_orderIdx].nameOfEvent);
				if (_correctOrder [_orderIdx].nameOfEvent == PathState.open_gate) {
					Events.G.Raise (new MBMusicMangerEvent (false));
				} else if(_correctOrder[_orderIdx].nameOfEvent == PathState.pond_loop){
					
					if (!_isPathLoop) {
						_isPathLoop = true;
						_loopFromIdx = _orderIdx;
						_loopToIdx = _orderIdx + 6;
						print ("enter loop from to: " + _loopToIdx);
						PathNode tempNode = FindNodeWithIndex (_correctOrder [_orderIdx].index);
						tempNode.resetAdjNodeAngle(0, 150f);

					}

					_isPathPause = false;
					CheckNextIdx ();
				}else {
					
				}

			}
		}




	}

	// util functions 
	public PathNode FindNodeWithIndex(int i){
		//print ("find node idx: " + i);
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

	void EnterLoop(int fromIdx, int toIdx){
		
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
