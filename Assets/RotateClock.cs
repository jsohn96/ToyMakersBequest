﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateClock : MonoBehaviour {
	[SerializeField] Transform[] _clockTransform;
	MinMax _speedMinMax = new MinMax(10.0f, 100.0f);
	[SerializeField] Transform[] _reverseClockTransform;
	float _dogMultiplier = 1.0f;

	float _counter = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.R)) {
			_counter += Time.deltaTime;
			for (int i = 0; i < _clockTransform.Length; i++) {
				_clockTransform [i].Rotate (Vector3.up * (MathHelpers.LinMapFrom01(_speedMinMax.Min, _speedMinMax.Max, _counter/5.0f)) * Time.deltaTime);
			}
			for (int r = 0; r < _reverseClockTransform.Length; r++) {
				if (r < 5) {
					_dogMultiplier = 2.0f;
				} else {
					_dogMultiplier = 1.0f;
				}
				_reverseClockTransform[r].Rotate(Vector3.down * _dogMultiplier *(MathHelpers.LinMapFrom01(_speedMinMax.Min, _speedMinMax.Max, _counter/5.0f)) * Time.deltaTime);
			}
		} else {
			if (_counter > 0.0f) {
				_counter -= 0.05f;
				for (int i = 0; i < _clockTransform.Length; i++) {
					_clockTransform [i].Rotate (Vector3.up * (MathHelpers.LinMapFrom01(_speedMinMax.Min, _speedMinMax.Max, _counter/5.0f)) * Time.deltaTime);
				}
				for (int r = 0; r < _reverseClockTransform.Length; r++) {
					if (r < 5) {
						_dogMultiplier = 2.0f;
					} else {
						_dogMultiplier = 1.0f;
					}
					_reverseClockTransform[r].Rotate(Vector3.down * _dogMultiplier *(MathHelpers.LinMapFrom01(_speedMinMax.Min, _speedMinMax.Max, _counter/5.0f)) * Time.deltaTime);
				}			} else {
				_counter = 0.0f;
			}
		}
	}
}
