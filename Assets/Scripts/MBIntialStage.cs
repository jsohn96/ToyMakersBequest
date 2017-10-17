using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBIntialStage : MonoBehaviour {
	bool _isDecend = false;
	[SerializeField] Transform _DancerTransform;
	float _speed = 1.0f;
	Vector3 FinalPos;

	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (DescendHandle);

	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (DescendHandle);

	}

	// Use this for initialization
	void Start () {
		
		FinalPos = transform.localPosition;
		FinalPos.z += 2.5f;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_isDecend) {
			if (Mathf.Abs (gameObject.transform.localPosition.z - FinalPos.z) > 0.1f) {
				Vector3 tempPos = transform.localPosition;
				tempPos.z += _speed * Time.deltaTime;
				transform.localPosition = tempPos;
			} else {
				transform.localPosition = FinalPos;
				Events.G.Raise (new PathResumeEvent ());
				_isDecend = false;
			}
		}
		
	}

	void DescendHandle(PathStateManagerEvent e){
		if (e.activeEvent == PathState.descend_inital_stage) {
			_isDecend = true;
		}
	
	}
}
