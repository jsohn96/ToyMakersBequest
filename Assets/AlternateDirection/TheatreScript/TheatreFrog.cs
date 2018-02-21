using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TheatreFrog : MonoBehaviour {
	[SerializeField] AltTheatre _myTheatre;
	[SerializeField] Animator _frogAnim;
	Vector3 _finalPosition;
	[SerializeField] PathNode[] _JumpNode;
	[SerializeField] PathNode _FirstJumpNode;
	List<int> alreadyGoneIndex; 
	List<int> tempGoneIndex;
	TheatreFrogAnimationCtrl[] _FrogAnimCtrl;


	int DancerOnNodeIdx = -1;
	int curFrogIdx = -1;
	int _curNodeOrderIdx = -1;
	[SerializeField] bool isContorlAcivate = false;

	// frog matching game controls
	int _matchedCouple = 0;
	int _currentIcon = -1;


	//Vector3 _endPosition;
	bool _isMoving;

	// Use this for initialization
	void Start () {
		_FrogAnimCtrl = new TheatreFrogAnimationCtrl[_JumpNode.Length];
		for (int i = 0; i < _JumpNode.Length; i++){
			_FrogAnimCtrl [i] = _JumpNode [i].gameObject.GetComponentInChildren<TheatreFrogAnimationCtrl> ();
		}
		alreadyGoneIndex = new List<int> ();
		tempGoneIndex = new List<int> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_curNodeOrderIdx >= 0 && isContorlAcivate) {
			FrogDetectDancer ();
		}

	}

	void OnEnable(){
		//Events.G.AddListener<PathStateManagerEvent> (PathStateManageHandle);
		Events.G.AddListener<DancerOnBoard> (DancerOnBoardHandle);
		Events.G.AddListener<TheatreFrogClickEvent> (FrogClickHandle);
	}

	void OnDisable(){
		//Events.G.RemoveListener<PathStateManagerEvent> (PathStateManageHandle);
		Events.G.RemoveListener<DancerOnBoard> (DancerOnBoardHandle);
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
			// currently checking 
		}
	}

	// set all lilypads as control active
	void SetFrogControl(bool isactive){
		if (isContorlAcivate) {
			// deactivate all frog nodes 
			foreach (TheatreFrogAnimationCtrl tf in _FrogAnimCtrl){
				tf.SetControl (isactive);
			}
		}
	}

	// recv frog pad index and the icon index 
	void FrogClickHandle(TheatreFrogClickEvent e){
		Debug.Log ("Theatre Frog REcv : " + e.frogIdx + " current : " +_curNodeOrderIdx);

		CheckIconMatch (e.frogIdx, e.iconIdx);

//		if ((e.frogIdx-1) == _curNodeOrderIdx) {
//			// frog jump to random pos
//			if (alreadyGoneIndex.Count <= 5) {
//				JumpToRandomNode ();
//			} 
//		}
	}

	void CheckIconMatch(int _frogIdx, int _iconIdx){
		// update frog poistion 
		if ((_frogIdx-1) == _curNodeOrderIdx) {
			// frog jump to random pos
			if (alreadyGoneIndex.Count < 5) {
				JumpToRandomNode ();
			} else {
				Debug.Log ("Jump to water");
			}
		}

		tempGoneIndex.Add (_frogIdx-1);
		alreadyGoneIndex.Add (_frogIdx-1);
		// if already comparing 
		if (_currentIcon != -1) {
			if (_currentIcon == _iconIdx) {
				Debug.Log ("found icon match: " + _iconIdx);
				FoundIconMatch ();
			} else {
				// release forg index 
				Debug.Log ("No Match Reset");
				StartCoroutine (ReleaseNoMatch ());

			}
		} else {
			Debug.Log ("Set first match: " + _iconIdx);
			// reset current icon index ; wait for the next input 
			_currentIcon = _iconIdx;
		}


				
	}

	// release no match 
	IEnumerator  ReleaseNoMatch(){
		SetFrogControl (false);
		yield return new WaitForSeconds(1.5f);
		for(int i = 0; i<tempGoneIndex.Count; i ++){
			int tempIdx = tempGoneIndex [i];
			Debug.Log ("reset temp: " + _JumpNode [tempIdx]);
			TheatreFrogAnimationCtrl tempBehaviour = _JumpNode[tempIdx].gameObject.GetComponentInChildren<TheatreFrogAnimationCtrl>();
			tempBehaviour.ResetFrog ();
			alreadyGoneIndex.Remove (tempIdx);
		}
		tempGoneIndex.Clear ();
		_currentIcon = -1;
		SetFrogControl (true);
		
	}

	void FoundIconMatch(){
		TheatreSound._instance.PlayBellFeedback ();
		tempGoneIndex.Clear ();
		_currentIcon = -1;
		if (_matchedCouple + 1 < 3) {
			_matchedCouple += 1;
		} else {
			Debug.Log ("All matches found !! Frog in water");
			_myTheatre.MoveToNext ();
		}
	}

	void DancerOnBoardHandle(DancerOnBoard e){
		DancerOnNodeIdx = e.NodeIdx;
		Debug.Log ("Frog Dancer on node index: " + e.NodeIdx);
	}

	void FrogDetectDancer(){
		// if the current node that the player is step on is the one that's behind then check for the next position 
		//int tempIdx = -1;
		if(DancerOnNodeIdx == _curNodeOrderIdx + 1){
			JumpToNextNode(_curNodeOrderIdx + 1);
		}

	}
		

	void ActivateFrog(int index){
		TheatreFrogAnimationCtrl _curBehaviour = _JumpNode[index].gameObject.GetComponentInChildren<TheatreFrogAnimationCtrl>();
		if (_curBehaviour != null) {
			Events.G.Raise(new FrogIsOnTheMoveEvent());
			_curBehaviour.SetFrogOn (true);
			_curBehaviour.ShowFrog ();
			Debug.Log ("DROG DRGO");
		}
	}

	void JumpToNextNode(int jumptoIndex){
		// move on to the next node 


		// hide the frog in the current node 
//		if(jumptoIndex > _JumpNode.Length){
//			jumptoIndex = 0;
//
//		}

		TheatreFrogAnimationCtrl _curBehaviour = _JumpNode[_curNodeOrderIdx].gameObject.GetComponentInChildren<TheatreFrogAnimationCtrl>();
		if (_curBehaviour != null) {
			_curBehaviour.HideFrog ();
			_curBehaviour.SetFrogOn (false);
		}

		_curNodeOrderIdx = jumptoIndex;

		ActivateFrog (jumptoIndex);

	}



	void JumpToRandomNode(){
		
		int randomNode = Random.Range (0, 6);
		Debug.Log ("Jump To Random : " + randomNode);
		if (randomNode == _curNodeOrderIdx || randomNode == DancerOnNodeIdx || alreadyGoneIndex.Contains(randomNode)) {
			JumpToRandomNode ();
		} else {
			JumpToNextNode (randomNode);
			//ActivateFrog (randomNode);
			//_curNodeOrderIdx = randomNode;
			//alreadyGoneIndex.Add (randomNode);
		}
	}

	// fog behaviour 1 - when clicked jump to a random node 
	// fog behaviour 2 - when dancer is about to enter the same node, frog jump to a random node

	
}
