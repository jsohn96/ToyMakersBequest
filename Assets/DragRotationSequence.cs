using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DragRotationSequence : MonoBehaviour {
	bool _dragHadStarted = false;

	Timer _transitionTimer;
	[SerializeField] float _duration = 2.0f;
	[SerializeField] DragRotation _dragRotationScript;
	[SerializeField] ToggleAction[] _toggleAction;

	bool _once = false;
	float _toggleActionLength = 0;

	// Use this for initialization
	void Start () {
		_toggleActionLength = _toggleAction.Length;
		_transitionTimer = new Timer(_duration);
		_transitionTimer.Reset ();
		_transitionTimer.Pause ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (_dragRotationScript.isDragStart) {
			_transitionTimer.Resume ();
		} else {
			_transitionTimer.Pause ();
		}

		if (_transitionTimer.IsOffCooldown && !_once) {
			_once = true;
			Events.G.Raise (new NotebookInteractionEvent ());
			if (_toggleActionLength != 0) {
				for (int i = 0; i < _toggleActionLength; i++) {
					_toggleAction[i].ToggleActionOn ();	
				}
			}
		}
	}
}
