using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NotebookPage {
	public int pageNumber;
	public Transform pageObject;
	public GameObject[] objectsInPage;
	public bool nextPageLocked;

	public NotebookPage (
		int _pageNumber,
		Transform _pageObject,
		GameObject[] _objectsInPage,
		bool _nextPageLocked){
		pageNumber = _pageNumber;
		pageObject = _pageObject;
		objectsInPage = _objectsInPage;
		nextPageLocked = _nextPageLocked;
	}
}

public class NotebookPageManager : MonoBehaviour {

	[SerializeField] int _currentPage = 0;	// Use this for initialization
	int _pageToFlip = 0;
	bool _isTurningRight = false;
	[SerializeField] NotebookPage[] _noteBookPages;
	[SerializeField] GameObject _musicbox;
	[SerializeField] RawImage _renderTextureLayer;
	Color _emptyColor;

	Vector3 _originRot = new Vector3 (0.0f, 3.0f, 0.0f);
	Vector3 _goalROt = new Vector3 (0.0f, 177.0f, 0.0f);

	Timer _pageTurnTimer;
	Timer _hideObjectTimer;

	void Start () {
		_pageTurnTimer = new Timer (1.5f);
		_hideObjectTimer = new Timer (0.4f);
		_emptyColor = Color.white;
		_emptyColor.a = 0.0f;
	}

	public void TurnPageRight(){
		if (_currentPage >= _noteBookPages.Length || _noteBookPages[_currentPage].nextPageLocked) {
			return;
		} else {
			_pageToFlip = _currentPage;
			_currentPage = _currentPage + 1;
			_isTurningRight = true;
			_pageTurnTimer.Reset ();
		}
	}

	public void TurnPageLeft(){
		if (_currentPage == 0) {
			return;
		} else {
			_currentPage = _currentPage - 1;
			_pageToFlip = _currentPage;
			_isTurningRight = false;
			_pageTurnTimer.Reset ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_pageTurnTimer.IsOffCooldown) {
			if (_hideObjectTimer.IsOffCooldown) {
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					_hideObjectTimer.Reset ();
					StartCoroutine (DelayForObjectHide(false));
				} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
					_hideObjectTimer.Reset ();
					StartCoroutine (DelayForObjectHide(true));
				}
			}
		} else {
			if(_isTurningRight){
				_noteBookPages [_pageToFlip].pageObject.rotation = Quaternion.Euler (Vector3.Lerp (_originRot, _goalROt, _pageTurnTimer.PercentTimePassed));

				if (_currentPage < _noteBookPages.Length || _noteBookPages [_currentPage].objectsInPage.Length != null) {
					_renderTextureLayer.color = Color.Lerp (_emptyColor, Color.white, (_pageTurnTimer.PercentTimePassed - 0.5f)*2f);
				}
			} else {
				_noteBookPages[_pageToFlip].pageObject.rotation = Quaternion.Euler(Vector3.Lerp (_goalROt, _originRot, _pageTurnTimer.PercentTimePassed));
//				if (_noteBookPages [_currentPage].objectsInPage.Length != 0) {
//					_renderTextureLayer.color = Color.Lerp (_emptyColor, Color.white, (_pageTurnTimer.PercentTimePassed - 0.5f)*2f);
//				}
				//_renderTextureLayer.color = Color.Lerp (Color.white, _emptyColor, (_pageTurnTimer.PercentTimePassed *2f));
			}
		}
	}

	IEnumerator DelayForObjectHide(bool isRight){
		Color tempColor = _renderTextureLayer.color;
		while (!_hideObjectTimer.IsOffCooldown) {
			_renderTextureLayer.color = Color.Lerp (tempColor, _emptyColor, (_hideObjectTimer.PercentTimePassed));
			yield return null;
		}
		if (_hideObjectTimer.IsOffCooldown) {
			_renderTextureLayer.color = _emptyColor;
			if (isRight) {
				TurnPageRight ();
			} else {
				TurnPageLeft ();
			}
			yield return null;
		}
	}
}
