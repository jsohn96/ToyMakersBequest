using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateObject : ToggleAction {
	[SerializeField] Transform _myTransform;
	[SerializeField] float _targetSpeed = 100f;
	[Header ("0:x, 1:y, 2:z")]
	[SerializeField] int _whichAxis;
	bool _isRotating = false;
	[SerializeField] bool _rotateOnStart = false;


	void Start() {
		if (_rotateOnStart) {
			ToggleActionOn ();
		}
	}

	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			_isRotating = !_isRotating;

		}

		if (_isRotating) {
			if (_whichAxis == 0) {
				_myTransform.Rotate (-Time.deltaTime * _targetSpeed, 0f, 0f, Space.Self);
			} else if (_whichAxis == 1) {
				_myTransform.Rotate (0f, -Time.deltaTime * _targetSpeed, 0f, Space.Self);
			} else {
				_myTransform.Rotate (0f, 0f, -Time.deltaTime * _targetSpeed, Space.Self);
			}
		}
	}

	public override void ToggleActionOn(){
		base.ToggleActionOn ();
		_isRotating = true;
	}

	void OnEnable(){
	}

	void OnDisable() {
		_isRotating = false;
	}
}


