using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxCameraInitialize : MonoBehaviour {
	[SerializeField] Transform _otherPositionCamera;
	bool _once = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!_once && Input.GetKeyDown(KeyCode.Space)) {
			transform.parent = null;
			transform.SetPositionAndRotation (_otherPositionCamera.position, _otherPositionCamera.rotation);
			_once = true;
		}
	}
}
