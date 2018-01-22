using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDirectionFlow : MonoBehaviour {
	Vector2 _textureOffset;
	Renderer _pathRenderer;
	// Use this for initialization
	void Start () {
		_pathRenderer = GetComponent<Renderer> ();
		_textureOffset = _pathRenderer.material.mainTextureOffset;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_textureOffset.x -= 0.01f;
		_pathRenderer.material.mainTextureOffset = _textureOffset;
	}
}
