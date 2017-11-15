using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxBookInteractive : BookInteractive {

	bool _lidOpen = false;
	Timer _lidTimer;
	Vector3 _goalRotationVector3 = new Vector3 (0.343f, 0.051f, -81.50f);
	Quaternion _goalRotation;
	Quaternion _originRotation;
	[SerializeField] Transform _lidTransform;

	void Start(){
		_lidTimer = new Timer (0.5f);
		_originRotation = _lidTransform.localRotation;
		_goalRotation = Quaternion.Euler (_goalRotationVector3);
	}

	public override void Interact(){
		base.Interact ();
		if (_lidTimer.IsOffCooldown) {
			Events.G.Raise (new NotebookInteractionEvent ());
			_lidOpen = !_lidOpen;
			_lidTimer.Reset ();
		}
	}

	void FixedUpdate(){
		if (_lidOpen) {
			_lidTransform.localRotation = Quaternion.Lerp (_originRotation, _goalRotation, _lidTimer.PercentTimePassed);
		} else {
			_lidTransform.localRotation = Quaternion.Lerp (_goalRotation, _originRotation, _lidTimer.PercentTimePassed);
		}
	}


}
