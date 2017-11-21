using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NotebookPage {
	public int pageNumber;
	public GameObject objectsInPageContainer;
	public bool nextPageLocked;
	public Material pageMaterial;

	public NotebookPage (
		int _pageNumber,
		Transform _pageObject,
		GameObject _objectsInPageContainer,
		bool _nextPageLocked,
		Material _pageMaterial){
		pageNumber = _pageNumber;
		objectsInPageContainer = _objectsInPageContainer;
		nextPageLocked = _nextPageLocked;
		pageMaterial = _pageMaterial;
	}
}

public class PageFlipManagement : MonoBehaviour {
	[SerializeField] int _whichPageToStart = 0;

	PageFlipAnimation[] _pageFlipAnimations;
	[SerializeField] Transform[] _pagePool;
	[SerializeField] Transform _coverPage, _endPage;
	PageFlipAnimation _coverFlipAnimation, _endFlipAnimation;

	float _coverYPos, _endYPos;

	float[] _inBetweenY;
	// 0: empty slot

	[SerializeField] int _currentPage = 0;
	//0 = CoverPage;

	Vector3 _tempVector3;

	int _totalPages;
	int _lastFlippedPage;
	[SerializeField] int _pageToFlip = 0;

	[SerializeField] NotebookPage[] _noteBookPages;
	[SerializeField] RawImage _renderTextureLayer;
	Color _emptyColor;

	Timer _pageTurnTimer;
	Timer _hideObjectTimer;
	Timer _fadeInTimer;
	[SerializeField] AnimationClip _flipAnimation;

	bool _waitingForFadeIn = false;


	[SerializeField] ButtonManager _buttonManagerScript;

	void Start(){
		_totalPages = _noteBookPages.Length + 1;
		for (int i = 0; i < _noteBookPages.Length; i++) {
			_noteBookPages [i].pageNumber = i + 1;
		}

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

		//initialization section
		_emptyColor = Color.white;
		_emptyColor.a = 0.0f;
		_pageTurnTimer = new Timer (_flipAnimation.length);
		_hideObjectTimer = new Timer (0.4f);
		_fadeInTimer = new Timer (0.2f);

		CalculatePagePosition (_whichPageToStart);
	}
		
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			CheckPageFlipInput (false);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			CheckPageFlipInput (true);
		} 

		if (_waitingForFadeIn && _pageTurnTimer.IsOffCooldown) {
			_waitingForFadeIn = false;
			_fadeInTimer.Reset ();
		}

		if (_pageTurnTimer.IsOffCooldown && _renderTextureLayer.color != Color.white) {
			if (!_fadeInTimer.IsOffCooldown) {
				_renderTextureLayer.color = Color.Lerp (_emptyColor, Color.white, (_fadeInTimer.PercentTimePassed));
			} else {
				_renderTextureLayer.color = Color.white;
			}
		}
	}

	void CalculatePagePosition(int pageIndex){
		if (pageIndex > _totalPages) {
			pageIndex = _totalPages;
		}
		_currentPage = pageIndex;

		if (_currentPage != 0) {
			SwapDisplayedBookObjects ();
			if (_currentPage >= _totalPages) {
				_coverFlipAnimation.InstantMoveZ (_endYPos, false);
				_endFlipAnimation.InstantMoveZ (_coverYPos, false);

			} else {
				_coverFlipAnimation.InstantMoveZ (_endYPos, false);
				_endFlipAnimation.InstantMoveZ (_endYPos, true);
			}

			// The section below is for the non-cover/end pages
			if (_currentPage == 1) {
				// dont really need this because other than cover everything is the same
			} else if (_currentPage == 2) {
				_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [3], false); 
			} else {
				int modValue;
				if (_currentPage >= _totalPages - 2) {
					modValue = MathHelpers.Mod (_totalPages-3, 4);
					ContentPageInit (modValue, false);
					switch (modValue) {
					case 0:
						_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [1], false);
						if (_currentPage >= _totalPages - 1) {
							_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [0], false);
						} else if (_currentPage == _totalPages - 2) {
							_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [3], true);
						}
						break;
					case 1:
						_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [1], false);

						if (_currentPage >= _totalPages - 1) {
							_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [0], false);
						} else if (_currentPage == _totalPages - 2) {
							_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [3], true);
						}
						break;
					case 2:
						_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [1], false);

						if (_currentPage >= _totalPages - 1) {
							_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [0], false);
						} else if (_currentPage == _totalPages - 2) {
							_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [3], true);
						}
						break;
					case 3:
						_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [1], false);

						if (_currentPage >= _totalPages - 1) {
							_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [0], false);
						} else if (_currentPage == _totalPages - 2) {
							_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [3], true);
						}
						break;
					default:
						break;
					}

				} else {
					modValue = MathHelpers.Mod (_currentPage, 4);
					ContentPageInit (modValue);
				}
			}
		}
	}

	void ContentPageInit(int modValue, bool doAll = true) {
		switch (modValue) {
		case 0:
			if (doAll) {
				_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [3], true);
				_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [2], true);
			}
			_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [3], false);
			_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [2], false);
			break;
		case 1:
			if (doAll) {
				_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [2], true);
				_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [3], true);
			}
			_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [3], false);
			_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [2], false);
			break;
		case 2:
			if (doAll) {
				_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [2], true);
				_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [3], true);
			}
			_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [2], false);
			_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [3], false);
			break;
		case 3:
			if (doAll) {
				_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [2], true);
				_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [3], true);
			}
			_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [3], false);
			_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [2], false);
			break;
		default:
			break;
		}
	}

	public void FlipPageRight(){
		//if (_currentPage < _totalPages && (_lastFlippedPage != null) && _pageFlipAnimations [MathHelpers.Mod (_lastFlippedPage, 4)].CheckIfReady() && _endFlipAnimation.CheckIfReady() && _coverFlipAnimation.CheckIfReady()) {
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
		_pageToFlip = _currentPage;
		_pageTurnTimer.Reset ();
		_waitingForFadeIn = true;
		_currentPage = _currentPage + 1;

		_buttonManagerScript.BeginFade (_currentPage, _totalPages);
	}

	public void FlipPageLeft(){
		//if (_currentPage > 0) {
			_pageToFlip = _currentPage;
			_pageTurnTimer.Reset ();
		_waitingForFadeIn = true;
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
		_buttonManagerScript.BeginFade (_currentPage, _totalPages);
		//}
	}

	void SwapDisplayedBookObjects(){
		if (_pageToFlip-1 >= 0 && _pageToFlip < _totalPages && _noteBookPages [_pageToFlip-1].objectsInPageContainer != null) {
			_noteBookPages [_pageToFlip-1].objectsInPageContainer.SetActive (false);
		}
		if (_currentPage-1 >= 0 && _currentPage < _totalPages && _noteBookPages [_currentPage-1].objectsInPageContainer != null) {
			_noteBookPages [_currentPage-1].objectsInPageContainer.SetActive (true);
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
				FlipPageRight ();
			} else {
				FlipPageLeft ();
			}
			SwapDisplayedBookObjects ();
			yield return null;
		}
	}


	public void CheckPageFlipInput(bool isRightPressed){
		if (_pageTurnTimer.IsOffCooldown) {
			if (_hideObjectTimer.IsOffCooldown && _fadeInTimer.IsOffCooldown) {
				if (!isRightPressed) {
					if (_currentPage > 0) {
						_hideObjectTimer.Reset ();
						StartCoroutine (DelayForObjectHide(false));
					}
				} else {
					if (_currentPage < _totalPages && (_lastFlippedPage != null)) {
						if (_currentPage <= 0 || _currentPage >= _totalPages-1 || !_noteBookPages [_currentPage - 1].nextPageLocked) {
							_hideObjectTimer.Reset ();
							StartCoroutine (DelayForObjectHide (true));
						}
					}
				}
			}
		}
	}


	public bool IsPageTurnDone(){
		if (_pageTurnTimer.IsOffCooldown) {
			return true;
		} else {
			return false;
		}
	}

	public int GetStartingPage(){
		return _whichPageToStart;
	}
}
