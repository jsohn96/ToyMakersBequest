using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
	[SerializeField] Camera _lookAtCamera;

	void Update () {
		transform.LookAt (transform.position + _lookAtCamera.transform.rotation * Vector3.forward,
			_lookAtCamera.transform.rotation * Vector3.up);
	}
}
