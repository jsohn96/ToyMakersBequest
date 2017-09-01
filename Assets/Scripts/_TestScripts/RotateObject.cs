using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test{
public class RotateObject : MonoBehaviour {
	[SerializeField] Transform _myTransform;
	[SerializeField] float _targetSpeed = 100f;
	bool _isRotating = false;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			_isRotating = !_isRotating;

		}

		if (_isRotating) {
			_myTransform.Rotate(0f,0f,-Time.deltaTime*_targetSpeed,Space.World);
		}


		
	}
}
}

