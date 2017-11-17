using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class PageFlipManagement : MonoBehaviour {
	PageFlipAnimation[] _pageFlipAnimations;
	[SerializeField] Transform[] _pagePool;
	[SerializeField] Transform _coverPage, _endPage;
	PageFlipAnimation _coverFlipAnimation, _endFlipAnimation;
	[SerializeField] Material[] _pageContents;

	float _coverYPos, _endYPos;

	float[] _inBetweenY;
	// 0: empty slot

	int _currentPage = 0;
	//0 = CoverPage;

	Vector3 _tempVector3;

	int _totalPages;
	int _lastFlippedPage;

	void Start(){
		_totalPages = _pageContents.Length + 2;

		// gets references for page flipping animation
		_pageFlipAnimations = new PageFlipAnimation[_pagePool.Length];
		for (int i = 0; i < _pagePool.Length; i++) {
			_pageFlipAnimations[i] = _pagePool [i].GetChild (0).GetComponent<PageFlipAnimation> ();
		}
		_coverFlipAnimation = _coverPage.GetChild (0).GetComponent<PageFlipAnimation> ();
		_endFlipAnimation = _endPage.GetChild (0).GetComponent<PageFlipAnimation> ();

		// the z index fighting problem fix init
		_coverYPos = _coverPage.localPosition.y;
		_endYPos = _endPage.localPosition.y;
		float tempDistanceBetweenCovers = _coverYPos - _endYPos;
		float distanceBetweenEachPage = tempDistanceBetweenCovers / 6f;
		_inBetweenY = new float[5];
		for (int i = 0; i < 5; i ++) {
			_inBetweenY[i] = _coverYPos - ((float)(i+1) * distanceBetweenEachPage);
			if (i != 4) {
				_tempVector3 = _pagePool [i].localPosition;
				_tempVector3.y = _inBetweenY [i];
				_pagePool [i].localPosition = _tempVector3;
			}
		}
	}
		
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			FlipPageLeft ();
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			FlipPageRight ();
		}
	}

	public void FlipPageRight(){
		if (_currentPage < _totalPages && (_lastFlippedPage != null) && _pageFlipAnimations [MathHelpers.Mod (_lastFlippedPage, 4)].CheckIfReady() && _endFlipAnimation.CheckIfReady() && _coverFlipAnimation.CheckIfReady()) {
			_lastFlippedPage = _currentPage - 1;
			if (_currentPage > 2) {
				if (_currentPage == _totalPages - 1) {
					//last page
					_endFlipAnimation.FlipLeft (_coverYPos, false);
				} else if (_currentPage == _totalPages - 2) {
					_pageFlipAnimations [MathHelpers.Mod (_currentPage - 1, 4)].FlipLeft (_inBetweenY [0], false);
				} else if (_currentPage == _totalPages - 3) {
					_pageFlipAnimations [MathHelpers.Mod (_currentPage - 1, 4)].FlipLeft (_inBetweenY [1], false);
				} else {
					int whichPageToFlip = MathHelpers.Mod ((_currentPage - 1), 4);

					_pageFlipAnimations [whichPageToFlip].FlipLeft (_inBetweenY [2]);
					_pageFlipAnimations [MathHelpers.Mod ((whichPageToFlip + 1), 4)].MoveZ (_inBetweenY [2]);
					_pageFlipAnimations [MathHelpers.Mod ((whichPageToFlip - 1), 4)].MoveZ (_inBetweenY [3]);
					_pageFlipAnimations [MathHelpers.Mod ((whichPageToFlip - 2), 4)].SwapSides (_inBetweenY [4], _inBetweenY [3]);
				}
			} else if (_currentPage == 0) {
				_coverFlipAnimation.FlipLeft (_endYPos);
			} else if (_currentPage == 1) {
				_pageFlipAnimations [_currentPage - 1].FlipLeft (_inBetweenY [3]);
			} else if (_currentPage == 2) {
				_pageFlipAnimations [_currentPage - 1].FlipLeft (_inBetweenY [2]);
			}

			_currentPage = _currentPage + 1;
		}
	}

	public void FlipPageLeft(){
		if (_currentPage > 0 && (_lastFlippedPage != null) && _pageFlipAnimations [MathHelpers.Mod (_lastFlippedPage, 4)].CheckIfReady() && _endFlipAnimation.CheckIfReady() && _coverFlipAnimation.CheckIfReady()) {
			_currentPage = _currentPage - 1;
			_lastFlippedPage = _currentPage - 1;
			if (_currentPage > 2) {
				if (_currentPage == _totalPages - 1) {
					//last page
					_endFlipAnimation.FlipRight (_endYPos);
				} else if (_currentPage == _totalPages - 2) {
					_pageFlipAnimations [MathHelpers.Mod (_currentPage - 1, 4)].FlipRight (_inBetweenY [3]);
				} else if (_currentPage == _totalPages - 3) {
					_pageFlipAnimations [MathHelpers.Mod (_currentPage - 1, 4)].FlipRight (_inBetweenY [2]);
				} else {

					int whichPageToFlip = MathHelpers.Mod ((_currentPage - 1), 4);
					_pageFlipAnimations [whichPageToFlip].FlipRight (_inBetweenY [2]);
					_pageFlipAnimations [MathHelpers.Mod ((whichPageToFlip - 1), 4)].MoveZ (_inBetweenY [2]);
					_pageFlipAnimations [MathHelpers.Mod ((whichPageToFlip + 1), 4)].MoveZ (_inBetweenY [3]);
					_pageFlipAnimations [MathHelpers.Mod ((whichPageToFlip + 2), 4)].SwapSides (_inBetweenY [4], _inBetweenY [3]);
				}
			} else if (_currentPage == 0) {
				_coverFlipAnimation.FlipRight (_coverYPos, false);
			} else if (_currentPage == 1) {
				_pageFlipAnimations [_currentPage - 1].FlipRight (_inBetweenY [0], false);
			} else if (_currentPage == 2) {
				_pageFlipAnimations [_currentPage - 1].FlipRight (_inBetweenY [1], false);
			}
		}
	}
}
