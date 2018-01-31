using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltPeephole : MonoBehaviour {

	[SerializeField] ControlRoomManager _controlRoomManager;

	[Header("Which Peephole? 0-3")]
	public int _peepHoleIndex;

	bool _thisPeepHoleActivated = false;

	void Start(){
		CheckPeepHoleActivation ();
	}

	void CheckPeepHoleActivation(){
		if ((int)AltCentralControl._currentState >= _peepHoleIndex) {
			_thisPeepHoleActivated = true;
		} else {
			_thisPeepHoleActivated = false;
		}
	}

	void OnTouchDown(Vector3 touchPoint){
		CheckPeepHoleActivation ();
		if (_thisPeepHoleActivated) {
			_controlRoomManager.LookIntoPeephole (_peepHoleIndex);
		}
	}
}
