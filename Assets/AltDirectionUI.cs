using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AltDirectionUI : MonoBehaviour {
	[SerializeField] RectTransform _canvasRectTransform;

	[SerializeField] RectTransform _windowLeft, _windowRight;
	[SerializeField] AnimationCurve _windowScrollCurve;
	[SerializeField] WindowScrollSound _windowScrollSound;
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

	[Header("Scene Arrow References")]
	[SerializeField] RectTransform _sceneArrow;
	[SerializeField] Vector2[] _arrowPositions = new Vector2[4];
	bool _triggeredNextScene = false;
	int _pointerGoalIndex = 0;

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
	}

	void OnSceneLoaded(Scene scene,LoadSceneMode mode){
		WindowScroll (true);
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}


	public void TriggerNextScene(int pointerGoalIndex){
		if (!_triggeredNextScene && !_windowIsScrolling) {
			_pointerGoalIndex = pointerGoalIndex;
			_triggeredNextScene = true;
			StartCoroutine (MovePointer ());
		}
	}

	IEnumerator MovePointer(){
		float timer = 0f;
		float duration = 0.8f;
		Vector2 tempArrowPosition = _sceneArrow.anchoredPosition;
		Vector2 tempGoalPosition = _arrowPositions [_pointerGoalIndex];
		while (timer < (duration)) {
			timer += Time.unscaledDeltaTime;
			_sceneArrow.anchoredPosition = Vector2.Lerp (tempArrowPosition, tempGoalPosition, timer / duration);
			yield return null;
		}
		_sceneArrow.anchoredPosition = tempGoalPosition;
		yield return null;
		WindowScroll (false);
		Events.G.Raise (new ClosingDoorsForSceneTransition ());
	}

	void LoadScene(){
		switch (_pointerGoalIndex) {
		case 0:
			SceneManager.LoadScene ("ControlRoom");
			break;
		case 1: 
			SceneManager.LoadScene ("AltDancer");
			break;
		case 2:
			SceneManager.LoadScene ("AltDancer");
			break;
		case 3:
			SceneManager.LoadScene ("AltDancer");
			break;
		default:
			break;
		}
	}

//	public void SceneChange(int sceneIndex){
//		StartCoroutine (ChangeLevel (sceneIndex));
//	}
//
//	IEnumerator ChangeLevel(int sceneIndex){
//		yield return new WaitForSeconds(0.2f);
//		Fading fadeScript = GameObject.Find ("Fade").GetComponent<Fading> ();
//		float fadeTime = fadeScript.BeginFade (1);
//		yield return new WaitForSeconds(fadeTime);
//		SceneManager.LoadScene (sceneIndex);
//	}


	public void WindowScroll(bool open){
		
		StartCoroutine (MoveWindow (open));
	}

	IEnumerator MoveWindow(bool open){
		if (open) {
			_windowScrollSound.OpenWindowSound ();
		} else {
			_windowScrollSound.CloseWindowSound ();
		}
		_timer = 0f;
		_leftTemp = _windowLeft.anchoredPosition;
		_rightTemp = _windowRight.anchoredPosition;
		_windowOpening = open;
		_windowIsScrolling = true;

		if (_windowIsScrolling) {
			while (_timer < _duration) {
				if (AltCentralControl.isGameTimePaused) {
					_timer += Time.unscaledDeltaTime;
				} else {
					_timer += Time.deltaTime;
				}
				if (_windowOpening) {
					_windowLeft.anchoredPosition = Vector2.Lerp (_leftTemp, _leftWindowOpen, _windowScrollCurve.Evaluate (_timer / _duration));
					_windowRight.anchoredPosition = Vector2.Lerp (_rightTemp, _rightWindowOpen, _windowScrollCurve.Evaluate (_timer / _duration));
				} else {
					_windowLeft.anchoredPosition = Vector2.Lerp (_leftTemp, _leftWindowClose, _windowScrollCurve.Evaluate (_timer / _duration));
					_windowRight.anchoredPosition = Vector2.Lerp (_rightTemp, _rightWindowClose, _windowScrollCurve.Evaluate (_timer / _duration));
				}
				yield return null;
			}
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
			yield return null;
			Events.G.Raise (new SlidingDoorFinished (_windowOpen));
			if (_triggeredNextScene) {
				LoadScene ();
			}

		}
	}
}
