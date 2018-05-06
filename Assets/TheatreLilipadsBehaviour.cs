using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreLilipadsBehaviour : MonoBehaviour {
	[SerializeField] GameObject[] _lilipads;
	Vector3 _originPos;
	Vector3 _finalPos;
	float _moveSpeed = 0.001f;
	bool _isUp = false;


	// Use this for initialization
	void Awake () {
		_originPos = transform.position;
		_finalPos = _originPos;
		_finalPos.y += 0.013f;
	}
	
	// Update is called once per frame
	void Update () {
		if (_isUp) { 
			if (Vector3.Distance (transform.position, _finalPos) >= 0.001f) {
				transform.position = Vector3.MoveTowards (transform.position, _finalPos, _moveSpeed);
			} else {
				transform.position = _finalPos;
			}
		}
	}

	public void GoUp(){
		if (!_isUp) {
			_isUp = true;
		}
	}
}
