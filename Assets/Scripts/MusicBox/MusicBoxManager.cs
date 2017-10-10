using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxManager : MonoBehaviour {
	bool isBoxOpen = false;
	Animator _myAnim;
	[SerializeField] GameObject[] _Layers;
	[SerializeField] GameObject _firstDescendCircle;
	bool _isDecend;
	Vector3 FinalPos;
	float _speed = 5f;

	// Use this for initialization
	void Awake () {
		_myAnim = GetComponent<Animator> ();
		FinalPos = _firstDescendCircle.transform.localPosition;
		FinalPos.z += 17.6f;
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
		}

		if (_isDecend) {
			DescendFirstCircle ();
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
		_isDecend = true;
	}

	void DescendFirstCircle(){
		if (Mathf.Abs (_firstDescendCircle.transform.localPosition.z - FinalPos.z) > 0.1f) {
			Vector3 tempPos = _firstDescendCircle.transform.localPosition;
			tempPos.z += _speed * Time.deltaTime;
			_firstDescendCircle.transform.localPosition = tempPos;
		} else {
			_firstDescendCircle.transform.localPosition = FinalPos;
			Events.G.Raise (new PathResumeEvent ());
			_isDecend = false;
		}
			
	}
}
