using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchHandle : MonoBehaviour {
	[SerializeField] LineRenderer _lineRenderer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		_lineRenderer.SetPosition(0, transform.position);
	}
}
