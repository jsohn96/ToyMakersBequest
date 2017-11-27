using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxTitleBookInteractive : BookInteractive {

	[SerializeField] Transform _dancer;
	[SerializeField] Transform _dLight;
	[SerializeField] Transform _rotateAroundPivot;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		_dancer.Rotate (Vector3.up, 3f);
		//_rotateAroundPivot.Rotate (Vector3.up, 4f);
		_dLight.LookAt (_dancer);
	}
}
