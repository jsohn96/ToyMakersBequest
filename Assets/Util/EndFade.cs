using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndFade : MonoBehaviour {
	[SerializeField] Image _fadeBlackImage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void ChangeColor(Color receivedColor){
		_fadeBlackImage.color = receivedColor;
	}
}
