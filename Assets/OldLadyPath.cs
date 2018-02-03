using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldLadyPath : MonoBehaviour {
	[SerializeField] Vector3[] _paths = new Vector3[5];
	[SerializeField] float _durationBetween = 2.0f;
	int _whichSegment = 0;
	bool _segmentDone = true;
	Timer _movementTimer;

	int _whichLightOn = 4;
	[SerializeField] OldLadyLevelManager _oldLadyLevelManager;

	void Start () {
		_movementTimer = new Timer (_durationBetween);
		transform.position = _paths [0];
	}
	
	void Update () {
		if (_whichSegment < 4) {
			if (!_movementTimer.IsOffCooldown && !_segmentDone) {
				transform.position = Vector3.Lerp (_paths [_whichSegment], _paths [_whichSegment + 1], _movementTimer.PercentTimePassed);
			} else if (_movementTimer.IsOffCooldown && !_segmentDone) {
				_segmentDone = true;
				_whichSegment++;
				transform.position = _paths [_whichSegment];
				if (_whichSegment == 4) {
					_oldLadyLevelManager.MoveToNextCameraView ();
				}
			}
		}
	}

	public void SwapLights(int whichLight){
		_whichLightOn = whichLight;
		if (_segmentDone && _whichSegment < 4) {
			if (_whichSegment == whichLight) {
				_segmentDone = false;
				_movementTimer.Reset ();
			}
		} else if (!_segmentDone) {
			if (_whichSegment == whichLight) {
				_movementTimer.Resume ();
			} else {
				_movementTimer.Pause ();
			}
		}

	}
}
