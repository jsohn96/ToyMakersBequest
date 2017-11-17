using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			if (_currentPage > 2) {
				if (_currentPage == _totalPages - 1) {
					//last page
					_endFlipAnimation.FlipLeft (_coverYPos, false);
				} else if (_currentPage == _totalPages - 2) {
					_pageFlipAnimations [MathHelpers.Mod (_currentPage - 1,4)].FlipLeft (_inBetweenY [0], false);
				} else if (_currentPage == _totalPages - 3) {
					_pageFlipAnimations [MathHelpers.Mod (_currentPage - 1,4)].FlipLeft (_inBetweenY [1], false);
				} else {
					int whichPageToFlip = MathHelpers.Mod((_currentPage -1),4);

					_pageFlipAnimations [whichPageToFlip].FlipLeft (_inBetweenY [2]);
					_pageFlipAnimations[MathHelpers.Mod((whichPageToFlip + 1),4)].MoveZ(_inBetweenY[2]);
					_pageFlipAnimations[MathHelpers.Mod((whichPageToFlip - 1),4)].MoveZ(_inBetweenY[3]);
					_pageFlipAnimations[MathHelpers.Mod((whichPageToFlip - 2),4)].SwapSides(_inBetweenY[4], _inBetweenY[3]);
				}
			} else if (_currentPage == 0) {
				_coverFlipAnimation.FlipLeft (_endYPos);
			} else if (_currentPage == 1) {
				_pageFlipAnimations [_currentPage -1].FlipLeft (_inBetweenY[3]);
			} else if (_currentPage == 2) {
				_pageFlipAnimations [_currentPage -1].FlipLeft (_inBetweenY[2]);
			}

			_currentPage = _currentPage + 1;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			_currentPage = _currentPage - 1;

			if (_currentPage > 2) {
				if (_currentPage == _totalPages - 1) {
					//last page
					_endFlipAnimation.FlipRight (_endYPos);
				} else if (_currentPage == _totalPages - 2) {
					_pageFlipAnimations [MathHelpers.Mod (_currentPage - 1,4)].FlipRight (_inBetweenY [3]);
				} else if (_currentPage == _totalPages - 3) {
					_pageFlipAnimations [MathHelpers.Mod (_currentPage - 1,4)].FlipRight (_inBetweenY [2]);
				} else {
					
					int whichPageToFlip = MathHelpers.Mod ((_currentPage - 1), 4);
					_pageFlipAnimations [whichPageToFlip].FlipRight (_inBetweenY [2]);
					_pageFlipAnimations[MathHelpers.Mod((whichPageToFlip - 1),4)].MoveZ(_inBetweenY[2]);
					_pageFlipAnimations[MathHelpers.Mod((whichPageToFlip + 1),4)].MoveZ(_inBetweenY[3]);
					_pageFlipAnimations[MathHelpers.Mod((whichPageToFlip + 2),4)].SwapSides(_inBetweenY[4], _inBetweenY[3]);
				}
			} else if (_currentPage == 0) {
				_coverFlipAnimation.FlipRight (_coverYPos, false);
			} else if (_currentPage == 1) {
				_pageFlipAnimations [_currentPage -1].FlipRight (_inBetweenY[0], false);
			} else if (_currentPage == 2) {
				_pageFlipAnimations [_currentPage -1].FlipRight (_inBetweenY[1], false);
			}
		}
	}

	// move page on each side a little bit
	// keep record of up to 5 on each so that only the last 5 will make a difference

	// flip animation
	// swap parent to the otherside

	// move it a little up && down

	// bring in the last page, put none cover last page in the next thing


	//first five are populated

	// last five are also populated

}
