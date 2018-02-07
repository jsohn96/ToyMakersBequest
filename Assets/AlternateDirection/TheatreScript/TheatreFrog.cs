using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreFrog : MonoBehaviour {
	[SerializeField] AltTheatre _myTheatre;
	[SerializeField] Animator _frogAnim;
	Vector3 _finalPosition;
	[SerializeField] PathNode[] _JumpNode;
	[SerializeField] PathNode _FirstJumpNode;
	TheatreFrogAnimationCtrl[] _FrogAnimCtrl;

	int DancerOnNodeIdx = -1;
	int curFrogIdx = -1;
	int _curNodeOrderIdx = -1;
	[SerializeField] bool isContorlAcivate = false;

	//Vector3 _endPosition;
	bool _isMoving;

	// Use this for initialization
	void Start () {
		_FrogAnimCtrl = new TheatreFrogAnimationCtrl[_JumpNode.Length];
		for (int i = 0; i < _JumpNode.Length; i++){
			_FrogAnimCtrl [i] = _JumpNode [i].gameObject.GetComponentInChildren<TheatreFrogAnimationCtrl> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable(){
		//Events.G.AddListener<PathStateManagerEvent> (PathStateManageHandle);
		//Events.G.AddListener<DancerOnBoard> (DancerOnBoardHandle);
		Events.G.AddListener<TheatreFrogClickEvent> (FrogClickHandle);
	}

	void OnDisable(){
		//Events.G.RemoveListener<PathStateManagerEvent> (PathStateManageHandle);
		//Events.G.RemoveListener<DancerOnBoard> (DancerOnBoardHandle);
		Events.G.RemoveListener<TheatreFrogClickEvent> (FrogClickHandle);
	}

	public void FrogJump(){
		ActivateFrogControl ();
		ActivateFrog(_curNodeOrderIdx);
		_myTheatre.MoveToNext ();
	}

	void ActivateFrogControl(){
		if (!isContorlAcivate) {
			isContorlAcivate = true;
			// activate all frog nodes 
			SetFrogControl(true);
			//jump to the first node
			_curNodeOrderIdx = 5;
		}
	}

	void SetFrogControl(bool isactive){
		if (isContorlAcivate) {
			// deactivate all frog nodes 
			foreach (TheatreFrogAnimationCtrl tf in _FrogAnimCtrl){
				tf.SetControl (isactive);
			}
		}
	}

	void FrogClickHandle(TheatreFrogClickEvent e){
		Debug.Log ("Theatre Frog REcv : " + e.frogIdx + " current : " +_curNodeOrderIdx);
		if ((e.frogIdx-1) == _curNodeOrderIdx) {
			// frog jump to random pos
			JumpToRandomNode();
		}
	}
		

	void ActivateFrog(int index){
		TheatreFrogAnimationCtrl _curBehaviour = _JumpNode[index].gameObject.GetComponentInChildren<TheatreFrogAnimationCtrl>();
		if (_curBehaviour != null) {
			Events.G.Raise(new FrogIsOnTheMoveEvent());
			_curBehaviour.ShowFrog ();
		}
	}

	void JumpToNextNode(int jumptoIndex){
		// move on to the next node 
		//print("Jump to Node: " + pn.readNodeInfo().index);

		// hide the frog in the current node 
		if(jumptoIndex > _JumpNode.Length){
			jumptoIndex = 0;
		}
		TheatreFrogAnimationCtrl _curBehaviour = _JumpNode[_curNodeOrderIdx].gameObject.GetComponentInChildren<TheatreFrogAnimationCtrl>();
		if (_curBehaviour != null) {
			_curBehaviour.HideFrog ();
		}

		ActivateFrog (jumptoIndex);

	}

	void JumpToRandomNode(){
		
		int randomNode = Random.Range (0, 6);
		Debug.Log ("Jump To Random : " + randomNode);
		if (randomNode == _curNodeOrderIdx || randomNode == DancerOnNodeIdx) {
			JumpToRandomNode ();
		} else {
			ActivateFrog (randomNode);
			_curNodeOrderIdx = randomNode;
		}
	}

	// fog behaviour 1 - when clicked jump to a random node 
	// fog behaviour 2 - when dancer is about to enter the same node, frog jump to a random node

	
}
