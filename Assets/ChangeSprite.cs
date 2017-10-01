using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour {
	[SerializeField] Sprite _newSprite;
	SpriteRenderer _thisSpriteRenderer;

	// Use this for initialization
	void Awake () {
		_thisSpriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "MainCamera") {
			_thisSpriteRenderer.sprite = _newSprite;
		}
	}
}
