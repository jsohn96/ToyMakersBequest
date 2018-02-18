using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AltDirectionUI : MonoBehaviour {
	[SerializeField] RectTransform _canvasRectTransform;

	[SerializeField] RectTransform _windowLeft, _windowRight;
	Vector2 _leftWindowOpen = new Vector2(0f, 0f), _rightWindowOpen = new Vector2(0f, 0f);
	Vector2 _leftWindowClose, _rightWindowClose;
	Vector2 _leftTemp, _rightTemp;
	bool _windowOpen = false;
	bool _windowIsScrolling = false;
	bool _windowOpening = true;

	IEnumerator _windowCoroutine;

	float _timer = 0f;
	float _duration = 1.2f;

	[SerializeField] float _rightUIWidth = 160f;
	float _screenWidth = 1920f;

	float _referenceHeight = 1080f;

	void Awake(){
		_screenWidth = (_referenceHeight/_canvasRectTransform.sizeDelta.y)* _canvasRectTransform.sizeDelta.x;
		Vector2 windowWidth = new Vector2((_screenWidth - _rightUIWidth)/2f, 0f);
		_windowLeft.sizeDelta = windowWidth;
		_windowRight.sizeDelta = windowWidth;

		_leftWindowOpen = -windowWidth;
		_rightWindowOpen = windowWidth;

		if (!_windowOpen) {
			_windowLeft.anchoredPosition = _leftWindowClose;
			_windowRight.anchoredPosition = _rightWindowClose;

		} else {
			_windowLeft.anchoredPosition = _leftWindowOpen;
			_windowRight.anchoredPosition = _rightWindowOpen;
		}

		WindowScroll (true);
	}

	public void SceneChange(int sceneIndex){
		StartCoroutine (ChangeLevel (sceneIndex));
	}

	IEnumerator ChangeLevel(int sceneIndex){
		yield return new WaitForSeconds(0.2f);
		Fading fadeScript = GameObject.Find ("Fade").GetComponent<Fading> ();
		float fadeTime = fadeScript.BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene (sceneIndex);
	}


	public void WindowScroll(bool open){
		_timer = 0f;
		_leftTemp = _windowLeft.anchoredPosition;
		_rightTemp = _windowRight.anchoredPosition;
		_windowOpening = open;
		_windowIsScrolling = true;
	}

	void Update(){
		if (_windowIsScrolling) {
			if (_timer < _duration) {
				if (AltCentralControl.isGameTimePaused) {
					_timer += Time.unscaledDeltaTime;
				} else {
					_timer += Time.deltaTime;
				}
				if (_windowOpening) {
					_windowLeft.anchoredPosition = Vector2.Lerp (_leftTemp, _leftWindowOpen, _timer / _duration);
					_windowRight.anchoredPosition = Vector2.Lerp (_rightTemp, _rightWindowOpen, _timer / _duration);
				} else {
					_windowLeft.anchoredPosition = Vector2.Lerp (_leftTemp, _leftWindowClose, _timer / _duration);
					_windowRight.anchoredPosition = Vector2.Lerp (_rightTemp, _rightWindowClose, _timer / _duration);
				}
			} else {
				if (_windowOpening) {
					_windowLeft.anchoredPosition = _leftWindowOpen;
					_windowRight.anchoredPosition = _rightWindowOpen;
					_windowOpen = true;
				} else {
					_windowLeft.anchoredPosition = _leftWindowClose;
					_windowRight.anchoredPosition = _rightWindowClose;
					_windowOpen = false;
				}
				_windowIsScrolling = false;
				Events.G.Raise (new SlidingDoorFinished (_windowOpen));
			}
		}
	}
}
