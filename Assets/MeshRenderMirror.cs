using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRenderMirror : MonoBehaviour {
	MeshRenderer _parentRenderer;
	MeshRenderer _thisRenderer;
	// Use this for initialization
	void Awake () {
		_parentRenderer = transform.parent.GetComponent<MeshRenderer> ();
		_thisRenderer = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		_thisRenderer.enabled = _parentRenderer.enabled;
	}
}
