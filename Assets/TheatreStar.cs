using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreStar : MonoBehaviour {
	[SerializeField] TheatreSound _theatreSound;
	[SerializeField] SpriteRenderer _spriteRenderer;
	[SerializeField] Sprite _glowStar;
	[SerializeField] Sprite _nonGlowStar;

	void OnTouchDown(){
		_theatreSound.PlayStarSound ();
		_spriteRenderer.sprite = _glowStar;
	}
}
