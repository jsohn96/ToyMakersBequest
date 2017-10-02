﻿using System.Collections;
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
	hold_hand_with_TM
}


public class PathNetwork : MonoBehaviour {
	// the solution for the puzzle --> the order of the correct path 
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

	// Use this for initialization
	void Awake () {
		_myNodes = GetComponentsInChildren<PathNode> ();
		print ("Init Info: " + "\nNode Count"+ _myNodes.Length);
		// init player position 
		_curNodeIdx = _correctOrder[_orderIdx].index;
	}

	void OnEnable(){
		Events.G.AddListener<DancerFinishPath> (HandleDancerFinishPath);
		Events.G.AddListener<PathResumeEvent> (PathResumeHandle);

	}

	void OnDisable(){
		Events.G.RemoveListener<DancerFinishPath> (HandleDancerFinishPath);
		Events.G.RemoveListener<PathResumeEvent> (PathResumeHandle);
	}

	void Start(){
		Events.G.Raise (new DancerChangeMoveEvent (DancerMove.idleDance));
		//PositionDancer ();
	}


	
	// Update is called once per frame
	void Update () {
		///
		if(_isCheckingNext && _isPathPause != null){
			PathNode tempNode = FindNodeWithIndex (_curNodeIdx);
			if (tempNode.readNodeInfo ().isConnected && _curNode.readNodeInfo().isConnected) {
				// icc the past pathnode --> active path +1, isOnboard = false
				Events.G.Raise (new SetPathNodeEvent (_curNode.readNodeInfo ().index));
				_curNode = tempNode;
				_myDancer.SetNewPath (_curNode);
				// set the dancer onboard of the new one
				Events.G.Raise (new DancerOnBoard (_curNode.readNodeInfo ().index));
				_isCheckingNext = false;
			} else {
				print ("next node not correctly connected");
			}
		}

		if (Input.GetKeyDown (KeyCode.S) && !_isStartPath) {
			_isStartPath = true;
			PositionDancer ();
			Events.G.Raise (new DancerChangeMoveEvent (DancerMove.none));

		}
		
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
		// check if there is anyevent envoked when the path is finished 
		if (_correctOrder [_orderIdx].nameOfEvent == PathState.none) {
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
			Events.G.Raise (new PathStateManagerEvent (_correctOrder [_orderIdx].nameOfEvent));
			_isPathPause = true;
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


}
