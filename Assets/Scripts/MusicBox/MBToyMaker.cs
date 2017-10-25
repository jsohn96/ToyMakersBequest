using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBToyMaker : MonoBehaviour {
	[SerializeField] Transform _dancerTransform;
	[SerializeField] Transform _TMTransform;
	[SerializeField] Light _spotLight;

	Timer _descendTimer;
	bool _isLightOn = false;
	bool _isFollow = false;
	bool _isLookaAtDancer=false;
	bool _isStartPath = false;


	bool _isFlip = false;
	[SerializeField] Animator _stageAnimator;


	// Use this for initialization
	void Awake () {
		_spotLight.intensity = 0;
		_descendTimer = new Timer (10f);
		
	}

	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (PathStateManagerHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (PathStateManagerHandle);

	}
	
	// Update is called once per frame
	void Update () {
		if (_isLightOn && _spotLight.intensity <= 10) {
			_spotLight.intensity += Time.deltaTime * 5f;

		} else if (_spotLight.intensity > 10){
			Events.G.Raise (new PathResumeEvent ());
			_isLightOn = false;
		}

//		if (Input.GetKeyDown (KeyCode.F)) {
//			_stageAnimator.Play ("Flip");
//		}
		if(_isFollow){
			Vector3 temp = _dancerTransform.position;
			temp.x -= 4f;
			_TMTransform.position = temp;
			_TMTransform.parent = _dancerTransform.parent;
			//_isFollow = false;
		}

		if (_isLookaAtDancer) {
			print ("Looking at the dancer");
			_TMTransform.LookAt (_dancerTransform, Vector3.up);
		}

	}

	void FirstEncounterTMHandle(){
		if (!_isLightOn) {
			_isLightOn = true;
			print ("TM : Light on");
			_isLookaAtDancer = true;
		}

	}

	public void ResumePathWhenFlipFinish(){
		Events.G.Raise (new PathResumeEvent ());
	}

	void DancerHoldHandEvent(){
		if (!_isFollow) {
			_isFollow = true;
		}

	}


	void FlipTMStage(){
		print("TM Flip Stage");
		if (!_isFlip) {
			_stageAnimator.Play ("Flip");
			_isFlip = false;
		}
		if (!_isFollow) {
			_isFollow = true;
		}
	
	}

	void PathStateManagerHandle(PathStateManagerEvent e){
		switch (e.activeEvent) {
		case PathState.none:
			break;
		case PathState.first_encounter_TM:
			FirstEncounterTMHandle ();
			Events.G.Raise (new PathResumeEvent ());
			break;
		case PathState.hold_hand_with_TM:
			DancerHoldHandEvent ();
			Events.G.Raise (new PathResumeEvent ());
			break;
		case PathState.flip_TM_stage:
			FlipTMStage ();
			break;
		}
	}
}
