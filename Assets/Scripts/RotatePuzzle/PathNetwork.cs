using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNetwork : MonoBehaviour {
	// the solution for the puzzle --> the order of the correct path 
	[SerializeField] int[] _correctOrder;
	PathNode[] _myNodes;
	[SerializeField] Dancer _myDancer;
	int _curNodeIdx;
	int _orderIdx = 0;
	PathNode _curNode;
	bool _isCheckingNext = false;

	// Use this for initialization
	void Awake () {
		_myNodes = GetComponentsInChildren<PathNode> ();
		print ("Init Info: " + "\nNode Count"+ _myNodes.Length);
		// init player position 
		_curNodeIdx = _correctOrder[_orderIdx];
	}

	void OnEnable(){
		Events.G.AddListener<DancerFinishPath> (HandleDancerFinishPath);

	}

	void OnDisable(){
		Events.G.RemoveListener<DancerFinishPath> (HandleDancerFinishPath);

	}

	void Start(){
		PositionDancer ();
	}


	
	// Update is called once per frame
	void Update () {
		//if(FindNodeWithIndex (_curNodeIdx + 1).readNodeInfo.)
		// checking all the connection info here
		if(_isCheckingNext){
			PathNode tempNode = FindNodeWithIndex (_curNodeIdx);
			if (tempNode.readNodeInfo ().isConnected && _curNode.readNodeInfo().isConnected) {
				Events.G.Raise (new SetPathNodeEvent (_curNode.readNodeInfo ().index));
				_curNode = tempNode;
				_myDancer.SetNewPath (_curNode);
				_isCheckingNext = false;
			} else {
				print ("next node not correctly connected");
			}
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



	void HandleDancerFinishPath(DancerFinishPath e){
		print ("Check next available node");
		if (_orderIdx + 1 < _correctOrder.Length) {
			_orderIdx += 1;
			_curNodeIdx = _correctOrder [_orderIdx];
			_isCheckingNext = true;
		} else {
			print ("success!!");
			// trigger final state 
			Events.G.Raise(new PathCompeleteEvent());
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
