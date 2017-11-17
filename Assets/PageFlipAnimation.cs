using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageFlipAnimation : MonoBehaviour {
	Animator _bookAnim;
	bool _isOnRight = true;
	Timer _bookFlipAnimationTimer;
	[SerializeField] AnimationClip _bookFlipAnimation;

	IEnumerator _pageZLerpCoroutine;
	Vector3 _tempPos;
	float _originY = 0.0f;

	void Start () {
		_bookAnim = GetComponent<Animator> ();
		float reducedLength = (_bookFlipAnimation.length * 0.8f);
		Debug.Log (reducedLength);
		_bookFlipAnimationTimer = new Timer (reducedLength);
	
		Debug.Log (_bookFlipAnimationTimer.IsOffCooldown);
	}

	public void FlipRight(float newY, bool firstHalf = true) {
		if (!_isOnRight && _bookFlipAnimationTimer.IsOffCooldown) {
			_bookAnim.Play ("BackFlip");
		
			MoveZ (newY, firstHalf);

			_isOnRight = true;
		}
	}

	public void FlipLeft(float newY, bool firstHalf = true) {
		if (_isOnRight) {
			if (_bookFlipAnimationTimer.IsOffCooldown) {
				_bookAnim.Play ("Flip");

				MoveZ (newY, firstHalf);

				_isOnRight = false;
			}
		}
	}

	public void MoveZ(float newY, bool firstHalf = true){
		_bookFlipAnimationTimer.Reset ();
		if (_pageZLerpCoroutine != null) {
			StopCoroutine (_pageZLerpCoroutine);
		}
		_pageZLerpCoroutine = LerpZ (newY, firstHalf);
		StartCoroutine (_pageZLerpCoroutine);
	}

	public void SwapSides(float startY, float newY) {
		_tempPos = transform.parent.localPosition;
		_tempPos.y = startY;
		transform.parent.localPosition = _tempPos;
		MoveZ (newY);

		if (_isOnRight) {
			_bookAnim.Play("Flip", -1, _bookFlipAnimation.length);
		} else {
			_bookAnim.Play("BackFlip", -1, _bookFlipAnimation.length);
		}
		_isOnRight = !_isOnRight;
	}

	IEnumerator LerpZ(float newY, bool firstHalf) {
		_tempPos = transform.parent.localPosition;
		_originY = _tempPos.y;
		while (!_bookFlipAnimationTimer.IsOffCooldown) {
			if (firstHalf) {
				_tempPos.y = Mathf.Lerp (_originY, newY, (_bookFlipAnimationTimer.PercentTimePassed - 0.5f) * 2f);
			} else {
				_tempPos.y = Mathf.Lerp (_originY, newY, (_bookFlipAnimationTimer.PercentTimePassed) * 2f);
			}
			transform.parent.localPosition = _tempPos;
			yield return null;
		}
		_tempPos.y = newY;
		transform.parent.localPosition = _tempPos;
		yield return null;
	}
}
