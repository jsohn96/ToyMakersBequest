using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTestPathScript : MonoBehaviour {
	[SerializeField] MusicTestScript[] _musicTestScripts = new MusicTestScript[14];
	Timer _moveTimer;
	int cnt = 0;
	Vector3 _tempCurrentPos;
	Vector3 _goalPos;
	bool _once = false;
	// Use this for initialization
	void Start () {
		_moveTimer = new Timer (1f);
		_tempCurrentPos = transform.position;
		_goalPos = _tempCurrentPos;
	}
	
	// Update is called once per frame
	void Update () {
		if (_moveTimer.IsOffCooldown) {
			
			if (cnt == 0 || _musicTestScripts [cnt - 1]._peg || _musicTestScripts [cnt-1]._removeCircle || (!_musicTestScripts [cnt - 1]._peg && _musicTestScripts [cnt - 1]._isOther)) {
				if (!_once && cnt>0) {
					if (_musicTestScripts [cnt-1]._peg) {
						_musicTestScripts [cnt-1].PlayNote ();
						_once = true;
					}
				}
				transform.SetParent (null);
				if (_musicTestScripts [cnt]._removeCircle) {
					if (!_musicTestScripts [cnt]._isOther) {
						cnt++;
						_once = false;
					}
				}
				else if (!_musicTestScripts [cnt]._isOther) {
					_tempCurrentPos = transform.position;
					if (_musicTestScripts [cnt]._peg) {
						_goalPos = _musicTestScripts [cnt].transform.position;

					} else {
						_goalPos = _musicTestScripts [cnt]._inside.transform.position;
						transform.SetParent (_musicTestScripts [cnt]._inside.transform);
					}

					_moveTimer.Reset ();

					if (_musicTestScripts.Length != cnt + 1) {
						cnt++;
						_once = false;
					}

				}
			}
		} else {
			transform.position = Vector3.Lerp (_tempCurrentPos, _goalPos, _moveTimer.PercentTimePassed);
		}
	}
}
