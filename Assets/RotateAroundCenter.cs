using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundCenter : MonoBehaviour {
	[SerializeField] Transform _centerPoint;
	MinMax _speedMinMax = new MinMax(10.0f, 100.0f);
	float _counter = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.R)) {
			_counter += Time.deltaTime;
			transform.RotateAround(_centerPoint.position, Vector3.forward, (MathHelpers.LinMapFrom01(_speedMinMax.Min, _speedMinMax.Max, _counter/5.0f)) * Time.deltaTime);
		} else {
			if (_counter > 0.0f) {
				_counter -= 0.05f;
				transform.RotateAround(_centerPoint.position, Vector3.forward, (MathHelpers.LinMapFrom01(_speedMinMax.Min, _speedMinMax.Max, _counter/5.0f)) * Time.deltaTime);
			} else {
				_counter = 0.0f;
			}
		}
	}
}
