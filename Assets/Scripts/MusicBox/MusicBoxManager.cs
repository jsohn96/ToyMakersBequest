﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxManager : MonoBehaviour {
	bool isBoxOpen = false;
	Animator _myAnim;
	[SerializeField] GameObject[] _Layers;
	[SerializeField] PathNetwork[] _musicPaths;
	[SerializeField] GameObject _firstDescendCircle;

	Vector3 _goalRotation = new Vector3 (0.0f, 360.0f, -90.0f);
	Quaternion _originRotation;
	Timer _transitionLayerTimer;

	bool _isDecend;
	Vector3 FinalPos;
	float _speed = 5f;
	bool _hideFirstLayer=false;

	bool _isStartPath = false;

	// Use this for initialization
	void Awake () {
		_myAnim = GetComponent<Animator> ();
		FinalPos = _firstDescendCircle.transform.localPosition;
		FinalPos.z += 17.6f;

		_transitionLayerTimer = new Timer (2.0f);
	}

	void Start(){
		Events.G.Raise (new DancerChangeMoveEvent (DancerMove.idleDance));
	}

	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (PathStateManagerHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (PathStateManagerHandle);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			isBoxOpen = !isBoxOpen;
			
		}

		if (isBoxOpen) {
			_myAnim.Play("Open");
			// activate the first layer path 

		}

		if (_isDecend) {
			DescendFirstCircle ();
		}

		if (_hideFirstLayer) {
			HideLayer (_Layers [0].transform);
		}

		if (_transitionLayerTimer.IsOffCooldown) {
			_hideFirstLayer = false;
		}


		if (Input.GetKeyDown (KeyCode.S) && !_isStartPath) {
			_isStartPath = true;
			_musicPaths[0].SetPathActive(true);
			Events.G.Raise (new DancerChangeMoveEvent (DancerMove.none));
		}
	}

	void PathStateManagerHandle(PathStateManagerEvent e){
		switch (e.activeEvent) {
		case PathState.none:
			break;
		case PathState.first_encounter_TM:
			break;
		case PathState.hold_hand_with_TM:
			break;
		case PathState.flip_TM_stage:
			break;
		case PathState.descend_to_layer_two:
			OpenLayer (2);
			break;
			
		}
	}

	void OpenLayer(int idx){
		_Layers [idx - 1].GetComponent<Animator> ().Play ("SideOpen");
		//_Layers [idx - 2].SetActive (false);
		_isDecend = true;

	}

	void DescendFirstCircle(){
		if (Mathf.Abs (_firstDescendCircle.transform.localPosition.z - FinalPos.z) > 0.1f) {
			Vector3 tempPos = _firstDescendCircle.transform.localPosition;
			tempPos.z += _speed * Time.deltaTime;
			_firstDescendCircle.transform.localPosition = tempPos;
		} else {
			print ("First layer move to the second layer");
			_firstDescendCircle.transform.localPosition = FinalPos;
			Events.G.Raise (new PathResumeEvent ());
			_isDecend = false;
			_musicPaths [0].SetPathActive (false);
			_firstDescendCircle.transform.parent = _musicPaths[1].transform;
			_musicPaths [1].UpdateNodes ();
			_musicPaths [1].SetPathActive (true);
			_hideFirstLayer = true;

			_originRotation = _Layers [0].transform.localRotation;
			_transitionLayerTimer.Reset ();

		}
			
	}


	void HideLayer(Transform layer){
		layer.localRotation = Quaternion.Lerp (_originRotation, Quaternion.Euler (_goalRotation), _transitionLayerTimer.PercentTimePassed);
	}
}
