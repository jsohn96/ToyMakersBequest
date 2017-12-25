using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMirror : MonoBehaviour {
	[SerializeField] Transform _targetMirrorTransform;
	
	void FixedUpdate () {
		transform.position = _targetMirrorTransform.position;
	}
}
