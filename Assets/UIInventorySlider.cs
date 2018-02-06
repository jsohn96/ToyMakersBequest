using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventorySlider : MonoBehaviour {

	[SerializeField] RectTransform _uiSliderRectTransform;
	[SerializeField] AnimationCurve _easeSlideCurve;

	Vector2 _originPos, _zeroPos;
	bool _isSliding = false;
	bool _isIn = false;

	void Start () {
		_originPos = _uiSliderRectTransform.anchoredPosition;
		_zeroPos = _originPos;
		_zeroPos.x = 0.0f;
	}
	
	public void SlideInOrOut(bool slidingIn){
		if (!_isSliding) {
			if (slidingIn != _isIn) {
				_isSliding = true;
				_isIn = slidingIn;
				StartCoroutine (Slide (slidingIn));
			}
		}
	}

	IEnumerator Slide(bool slidingIn){
		float timer = 0f;
		float duration = 0.8f;
		while (timer < duration) {
			if (AltCentralControl.isGameTimePaused) {
				timer += Time.unscaledDeltaTime;
			} else {
				timer += Time.deltaTime;
			}
			if (slidingIn) {
				_uiSliderRectTransform.anchoredPosition = Vector2.Lerp (_originPos, _zeroPos, _easeSlideCurve.Evaluate(timer / duration));
			} else {
				_uiSliderRectTransform.anchoredPosition = Vector2.Lerp (_zeroPos, _originPos, _easeSlideCurve.Evaluate(timer / duration));
			}
			yield return null;
		}
		if (slidingIn) {
			_uiSliderRectTransform.anchoredPosition = _zeroPos;
		} else {
			_uiSliderRectTransform.anchoredPosition = _originPos;
		}
		yield return null;
		_isSliding = false;
	}
}
