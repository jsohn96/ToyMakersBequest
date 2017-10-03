using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBToyMaker : MonoBehaviour {
	[SerializeField] Transform _dancerTransform;
	[SerializeField] Light _spotLight;

	Timer _descendTimer;
	bool _isLightOn = false;


	// Use this for initialization
	void Awake () {
		_spotLight.intensity = 0;
		_descendTimer = new Timer (10f);
		
	}

	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (DancerHoldHandEvent);
		Events.G.AddListener<PathStateManagerEvent> (FirstEncounterTMHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (DancerHoldHandEvent);
		Events.G.AddListener<PathStateManagerEvent> (FirstEncounterTMHandle);

	}
	
	// Update is called once per frame
	void Update () {
		if (_isLightOn && _spotLight.intensity <= 10) {
			_spotLight.intensity += Time.deltaTime * 5f;

		} else if (_spotLight.intensity > 10){
			Events.G.Raise (new PathResumeEvent ());
			_isLightOn = false;
		}
	}

	void FirstEncounterTMHandle(PathStateManagerEvent e){
		if (e.activeEvent == PathState.first_encounter_TM) {
			if (!_isLightOn) {
				_isLightOn = true;
				print ("TM : Light on");
			}
		}
	
	}

	void DancerHoldHandEvent(PathStateManagerEvent e){
		if (e.activeEvent == PathState.hold_hand_with_TM) {
			
		}

	}
}
