using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeRenderTexture : MonoBehaviour {
	[SerializeField] RenderTexture _renderTexture;

	// Use this for initialization
	void Awake () {
		_renderTexture.width = Screen.width;
		_renderTexture.height = Screen.height;
	}
}
