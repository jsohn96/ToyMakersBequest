using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeRenderTexture : MonoBehaviour {
	[SerializeField] Camera _secondCamera;
	[SerializeField] RawImage _rawImage;
	RenderTexture _renderTexture;

	// Use this for initialization
	void Start () {
		_renderTexture = new RenderTexture (Screen.width, Screen.height, 24);
		_secondCamera.targetTexture = _renderTexture;
		_rawImage.texture = _renderTexture;
	}
}
