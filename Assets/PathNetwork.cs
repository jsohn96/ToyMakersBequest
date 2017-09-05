using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNetwork : MonoBehaviour {
	[SerializeField] PathNode[] _myNodes;
	[SerializeField] Dancer _myDancer;
	int _curNodeIdx = 0;
	PathNode _curNode;
	bool _isCheckingNext = false;

	// Use this for initialization
	void Awake () {
		_myNodes = GetComponentsInChildren<PathNode> ();
		print ("Init Info: " + "\nNode Count"+ _myNodes.Length);
		// init player position 

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
		if(_isCheckingNext){
			PathNode tempNode = FindNodeWithIndex (_curNodeIdx);
			if (tempNode.readNodeInfo ().isConnected && _curNode.readNodeInfo().isConnected) {
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

	// util functions 
	PathNode FindNodeWithIndex(int i){
		foreach (PathNode _pn in _myNodes) {
			NodeInfo ninfo = _pn.readNodeInfo ();
			if (ninfo.index == i) {
				return _pn;
				break;
			} 
		}

		return _myNodes [0];
	}

	void HandleDancerFinishPath(DancerFinishPath e){
		print ("Check next available node");
		if (_curNodeIdx < _myNodes.Length-1) {
			_curNodeIdx += 1;
			_isCheckingNext = true;
		} else {
			print ("success!!");
		}


	}
}
