using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPageTest : MonoBehaviour {
	Animator _bookAnim;
	bool _isOnRight = true;
	Timer _bookFlipAnimationTimer;
	[SerializeField] AnimationClip _bookFlipAnimation;


	void Start () {
		_bookAnim = GetComponent<Animator> ();
		float reducedLength = (_bookFlipAnimation.length * 0.8f);
		_bookFlipAnimationTimer = new Timer (reducedLength);
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			FlipRight ();
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			FlipLeft ();
		}
		
	}

	public void FlipRight() {
		if (!_isOnRight && _bookFlipAnimationTimer.IsOffCooldown) {
			_bookAnim.Play ("BackFlip");
			_bookFlipAnimationTimer.Reset ();
			_isOnRight = true;
		}
	}

	public void FlipLeft() {
		if (_isOnRight && _bookFlipAnimationTimer.IsOffCooldown) {
			_bookAnim.Play ("Flip");
			_bookFlipAnimationTimer.Reset ();
			_isOnRight = false;

		}
	}
}
