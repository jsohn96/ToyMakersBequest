using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceHeadMovement : MonoBehaviour {

	Vector3 _lookDirection;
	Vector3 _tempDirection;

	[SerializeField] Transform _whoToLookAt;

	void Start(){
		_lookDirection = _whoToLookAt.position;
		transform.LookAt (_lookDirection);
	}

	void Update () {
		LookAtMouse ();
	}

	void LookAtMouse(){
		_tempDirection = _whoToLookAt.position;
		if (!MathHelpers.Vector3Equals (_tempDirection, _lookDirection)) {
			_lookDirection = _tempDirection;
			transform.LookAt (_lookDirection);
		}
	}
}
