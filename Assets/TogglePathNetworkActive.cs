using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePathNetworkActive : ToggleAction {

//	[SerializeField] Transform _myTransform;
//	[SerializeField] float _targetSpeed = 100f;
//	[Header ("0:x, 1:y, 2:z")]
//	[SerializeField] int _whichAxis;
//	bool _isRotating = false;
	[SerializeField] PathNetwork _pathNetwork;

	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.Space)) {
//			_isRotating = !_isRotating;
			_pathNetwork.SetPathActive (true);
		}
//
//		if (_isRotating) {
//			if (_whichAxis == 0) {
//				_myTransform.Rotate (-Time.deltaTime * _targetSpeed, 0f, 0f, Space.Self);
//			} else if (_whichAxis == 1) {
//				_myTransform.Rotate (0f, -Time.deltaTime * _targetSpeed, 0f, Space.Self);
//			} else {
//				_myTransform.Rotate (0f, 0f, -Time.deltaTime * _targetSpeed, Space.Self);
//			}
//		}
	}

	public override void ToggleActionOn(){
		base.ToggleActionOn ();
		_pathNetwork.SetPathActive (true);
	}

//	void OnEnable(){
//		_isRotating = true;
//	}

	void OnDisable() {
//		_isRotating = false;
		_pathNetwork.SetPathActive (false);
	}
}
