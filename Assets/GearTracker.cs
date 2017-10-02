using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTracker : MonoBehaviour {
	

	[SerializeField] bool[] _gearTracker = new bool[4]{false, false, false, false};
	[SerializeField] GameObject[] _gearImages = new GameObject[4];
	[SerializeField] StopForwardMovementPeep _stopForwardMovementScript;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		
	}

	void AddGear(PickedUpGearEvent e){
		int index = e.WhichGear - 1;
		_gearTracker [index] = true;
		_gearImages [index].SetActive (true);

		bool completionChecker = true;
		for (int i = 0; i < _gearTracker.Length; i++) {
			if (_gearTracker [i] == false) {
				completionChecker = false;
			}
		}
		if (completionChecker) {
			_stopForwardMovementScript.AllowPassage ();
		}
	}

	void OnEnable(){
		Events.G.AddListener<PickedUpGearEvent> (AddGear);
	}
	void OnDisable(){
		Events.G.RemoveListener<PickedUpGearEvent> (AddGear);
	}
}
