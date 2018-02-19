using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltPeephole : MonoBehaviour {

	[SerializeField] ControlRoom _controlRoomScript;

	[Header("Which Peephole? 0-3")]
	public int _peepHoleIndex;

	bool _thisPeepHoleActivated = false;
	bool _clickedOnce = false;

	[SerializeField] Vector3 _cameraZoomPosition;

	void CheckPeepHoleActivation(PeepHoleActivationCheck e){
		if (AltCentralControl._peepAnimated[_peepHoleIndex]) {
			_thisPeepHoleActivated = true;
		} else {
			_thisPeepHoleActivated = false;
		}
		_clickedOnce = false;
	}

	void OnTouchDown(Vector3 touchPoint){
		if (_thisPeepHoleActivated && !_clickedOnce) {
			_controlRoomScript.LookIntoPeephole (_peepHoleIndex, _cameraZoomPosition);
			_clickedOnce = true;
		}
	}


	void OnEnable(){
		Events.G.AddListener<PeepHoleActivationCheck> (CheckPeepHoleActivation);
	}

	void OnDisable(){
		Events.G.RemoveListener<PeepHoleActivationCheck> (CheckPeepHoleActivation);
	}
}
