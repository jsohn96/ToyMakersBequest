using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockCompletion : MonoBehaviour {
	[SerializeField] DropZone[] _dogDropZones;
	bool _isComplete = false;
	[SerializeField] float _maxSpeed = 600.0f;
	
	void Update () {
		if (!_isComplete) {
			_isComplete = true;
			for (int i = 0; i < _dogDropZones.Length; i++) {
				if (!_dogDropZones [i].occupied) {
					_isComplete = false;
				}
			}
		} else {
			Events.G.Raise (new ClockCompletionEvent (_isComplete, _maxSpeed));
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			Events.G.Raise (new ClockCompletionEvent (_isComplete, _maxSpeed));
		}
	}
}
