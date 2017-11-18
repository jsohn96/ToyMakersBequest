using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotebookPageManager : MonoBehaviour {
//
//	[SerializeField] int _currentPage = 0;	// Use this for initialization
//	//[SerializeField] int _pageToFlip = 0;
//	bool _isTurningRight = false;
//	[SerializeField] NotebookPage[] _noteBookPages;
//	[SerializeField] GameObject _musicbox;
//	[SerializeField] RawImage _renderTextureLayer;
//	Color _emptyColor;
//
//	Vector3 _originRot = new Vector3 (0.0f, 3.0f, 0.0f);
//	Vector3 _goalROt = new Vector3 (0.0f, 177.0f, 0.0f);
//
//	Timer _pageTurnTimer;
//	Timer _hideObjectTimer;
//	Timer _leftPageFadeInTimer;
//
//
//	bool _waitForLeftPage = false;
//	bool _isLeftTurning = false;
//
//	void Start () {
//		_pageTurnTimer = new Timer (1.5f);
//		_hideObjectTimer = new Timer (0.4f);
//		_leftPageFadeInTimer = new Timer (0.5f);
//		_emptyColor = Color.white;
//		_emptyColor.a = 0.0f;
//
//	}
//
//	public void TurnPageRight(){
//		_pageToFlip = _currentPage;
//		_currentPage = _currentPage + 1;
//		_isTurningRight = true;
//		_pageTurnTimer.Reset ();
//	}
//
//	public void TurnPageLeft(){
//		_pageToFlip = _currentPage;
//		_currentPage = _currentPage - 1;
//		_isTurningRight = false;
//		_pageTurnTimer.Reset ();
//	}
//
//	public void CheckPageFlipInput(bool isRightPressed){
//		if (_pageTurnTimer.IsOffCooldown && !_isLeftTurning) {
//			if (_hideObjectTimer.IsOffCooldown) {
//				if (!isRightPressed) {
//					if (_currentPage != 0) {
//						_hideObjectTimer.Reset ();
//						_isLeftTurning = true;
//					
//						StartCoroutine (DelayForObjectHide(false));
//					}
//				} else {
//					if (_currentPage < _noteBookPages.Length - 1 && !_noteBookPages [_currentPage].nextPageLocked) {
//						_hideObjectTimer.Reset ();
//						StartCoroutine (DelayForObjectHide (true));
//					}
//				}
//			}
//		}
//	}
//
//	// Update is called once per frame
//	void Update () {
//		if (_pageTurnTimer.IsOffCooldown && !_isLeftTurning) {
//			if (_hideObjectTimer.IsOffCooldown) {
//				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
//					if (_currentPage != 0) {
//						_hideObjectTimer.Reset ();
//						_isLeftTurning = true;
//					
//						StartCoroutine (DelayForObjectHide (false));
//					}
//				} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
//					if (_currentPage < _noteBookPages.Length - 1 && !_noteBookPages [_currentPage].nextPageLocked) {
//						_hideObjectTimer.Reset ();
//						StartCoroutine (DelayForObjectHide (true));
//					}
//				}
//			}
//		} else if(!_pageTurnTimer.IsOffCooldown || _isLeftTurning) {
//			if(_isTurningRight){
//				_noteBookPages [_pageToFlip].pageObject.rotation = Quaternion.Euler (Vector3.Lerp (_originRot, _goalROt, _pageTurnTimer.PercentTimePassed));
//
//				if (_currentPage < _noteBookPages.Length || _noteBookPages [_currentPage].objectsInPage.Length != null) {
//					_renderTextureLayer.color = Color.Lerp (_emptyColor, Color.white, (_pageTurnTimer.PercentTimePassed - 0.5f)*2f);
//				}
//			} else {
//				_noteBookPages[_currentPage].pageObject.rotation = Quaternion.Euler(Vector3.Lerp (_goalROt, _originRot, _pageTurnTimer.PercentTimePassed));
//				if (_pageTurnTimer.IsOffCooldown) {
//					if (!_waitForLeftPage) {
//						_waitForLeftPage = true;
//						_leftPageFadeInTimer.Reset ();
//					} else {
//						_renderTextureLayer.color = Color.Lerp (_emptyColor, Color.white, (_leftPageFadeInTimer.PercentTimePassed));
//						if (_leftPageFadeInTimer.IsOffCooldown) {
//							_renderTextureLayer.color = Color.white;
//							_isLeftTurning = false;
//							_waitForLeftPage = false;
//						}
//					}
//				}
//			}
//		}
//	}
//
//	void SwapDisplayedBookObjects(){
//		for (int i = 0; i < _noteBookPages [_pageToFlip].objectsInPage.Length; i++) {
//			_noteBookPages [_pageToFlip].objectsInPage [i].SetActive (false);
//		}
//		for (int i = 0; i < _noteBookPages [_currentPage].objectsInPage.Length; i++) {
//			_noteBookPages [_currentPage].objectsInPage [i].SetActive (true);
//		}
//	}
//
//	IEnumerator DelayForObjectHide(bool isRight){
//		Color tempColor = _renderTextureLayer.color;
//
//		if (!isRight) {
//			TurnPageLeft ();
//		}
//		while (!_hideObjectTimer.IsOffCooldown) {
//			_renderTextureLayer.color = Color.Lerp (tempColor, _emptyColor, (_hideObjectTimer.PercentTimePassed));
//			yield return null;
//		}
//		if (_hideObjectTimer.IsOffCooldown) {
//			_renderTextureLayer.color = _emptyColor;
//			if (isRight) {
//				TurnPageRight ();
//			}
//			SwapDisplayedBookObjects ();
//			yield return null;
//		}
//	}
}
