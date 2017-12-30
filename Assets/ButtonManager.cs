using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
	[SerializeField] PageFlipManagement _pageFlipManagementScript;
	bool _beginCheckingPageFlipFinish = false;
	Timer _fadeTimer;
	[SerializeField] Button _leftButton, _rightButton;
	Image _leftImage, _rightImage;
	Color _normalColor;
	Color _emptyColor;
	IEnumerator _fadeCoroutine;
	int _currentPage;
	int _totalPages;

	bool _justUnlocked = false;

	// Use this for initialization
	void Start () {
		_fadeTimer = new Timer (0.3f);
		_leftImage = _leftButton.gameObject.GetComponent<Image> ();
		_rightImage = _rightButton.gameObject.GetComponent<Image> ();
		_normalColor = _leftButton.colors.normalColor;
		_emptyColor = _normalColor;
		_emptyColor.a = 0.0f;

		if (_pageFlipManagementScript.GetStartingPage () == 0) {
			_leftButton.interactable = false;
			_leftImage.color = _emptyColor;
		}

		if (_justUnlocked) {
			_rightButton.interactable = false;
			_rightImage.color = _emptyColor;
			_leftButton.interactable = false;
			_leftImage.color = _emptyColor;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (_beginCheckingPageFlipFinish) {
			if (_pageFlipManagementScript.IsPageTurnDone ()) {
				_beginCheckingPageFlipFinish = false;
				FadeInArrows ();
			}
		}
	}
		

	public void BeginFade(int currentPage, int totalPages){
		_currentPage = currentPage;
		_totalPages = totalPages;
		_fadeTimer.Reset ();
		_fadeCoroutine = FadeButton ();
		StartCoroutine (_fadeCoroutine);
		_beginCheckingPageFlipFinish = true;
	}

	IEnumerator FadeButton(){
		bool _rightIsOn = true;
		bool _leftIsOn = true;
		if (!_leftButton.interactable) {
			_leftIsOn = false;
		} else {
			_leftButton.interactable = false;
		}
		if (!_rightButton.interactable) {
			_rightIsOn = false;
		} else {
			_rightButton.interactable = false;
		}
			
		while (!_fadeTimer.IsOffCooldown) {
			if (_leftIsOn) {
				_leftImage.color = Color.Lerp (_normalColor, _emptyColor, _fadeTimer.PercentTimePassed);
			}
			if (_rightIsOn) {
				_rightImage.color = Color.Lerp (_normalColor, _emptyColor, _fadeTimer.PercentTimePassed);
			}
			yield return null;
		}
		if (_leftIsOn) {
			_leftImage.color = _emptyColor;
		}
		if (_rightIsOn) {
			_rightImage.color = _emptyColor;
		}
		yield return null;
	}

	IEnumerator FadeBackButton(){
		bool nextPageLocked = _pageFlipManagementScript.CheckCurrentPageLock ();
		while (!_fadeTimer.IsOffCooldown) {
			if (_currentPage != 0) {
				_leftImage.color = Color.Lerp (_emptyColor, _normalColor, _fadeTimer.PercentTimePassed);
			}
			if (_currentPage != _totalPages && !nextPageLocked) {
				_rightImage.color = Color.Lerp (_emptyColor, _normalColor, _fadeTimer.PercentTimePassed);
			}
			yield return null;
		}
		if (_currentPage != 0) {
			_leftImage.color = _normalColor;
			_leftButton.interactable = true;
		}
		if (_currentPage != _totalPages && !nextPageLocked) {
			_rightImage.color = _normalColor;
			_rightButton.interactable = true;
		}
		yield return null;
	}

	void UnlockEventHandle (LeatherUnlockEvent e){
		int unlockIndex = e.UnlockIndex;
		_justUnlocked = e.JustUnlocked;
//		if (unlockIndex != 0) {
//			if (unlockIndex > _id) {
//				_preUnlocked = true;
//			} else if (unlockIndex == _id) {
//				if (justUnlocked) {
//					_unlocked = true;
//				} else {
//					_preUnlocked = true;
//				}
//			}
//		}
	}

	public void FadeInArrows(){
		_currentPage = _pageFlipManagementScript.GetCurrentPage ();
		_fadeTimer.Reset ();
		if (_fadeCoroutine != null) {
			StopCoroutine (_fadeCoroutine);
		}
		StartCoroutine (FadeBackButton ());
	}

	void OnEnable(){
		Events.G.AddListener<LeatherUnlockEvent> (UnlockEventHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<LeatherUnlockEvent> (UnlockEventHandle);
	}
}
