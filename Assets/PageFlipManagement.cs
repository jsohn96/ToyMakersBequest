using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NotebookPage {
	public int pageNumber;
	public GameObject objectsInPageContainer;
	public bool nextPageLocked;
	public Material pageFrontMaterial;
	public Material pageBackMaterial;

	public NotebookPage (
		int _pageNumber,
		Transform _pageObject,
		GameObject _objectsInPageContainer,
		bool _nextPageLocked,
		Material _pageFrontMaterial,
		Material _pageBackMaterial){
		pageNumber = _pageNumber;
		objectsInPageContainer = _objectsInPageContainer;
		nextPageLocked = _nextPageLocked;
		pageFrontMaterial = _pageFrontMaterial;
		pageBackMaterial = _pageBackMaterial;
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
	[SerializeField] Camera _renderTextureCamera;
	Color _emptyColor;

	Timer _pageTurnTimer;
	Timer _hideObjectTimer;
	Timer _fadeInTimer;
	[SerializeField] AnimationClip _flipAnimation;

	bool _waitingForFadeIn = false;


	[SerializeField] ButtonManager _buttonManagerScript;

	[SerializeField] Transform _bookSpine;
	Quaternion _goalSpineRotation;
	Quaternion _originSpineRotation;
	[SerializeField] Material _defaultPageMaterial;

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

		_originSpineRotation = _bookSpine.localRotation;
		_goalSpineRotation = Quaternion.Euler(0f, 0f, 90f);
		if (_whichPageToStart != 0) {
			_bookSpine.transform.localRotation = _goalSpineRotation;
		}
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
			if (!_renderTextureCamera.enabled) {
				_renderTextureCamera.enabled = true;
			}
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

		if (_currentPage < 3) {
			PopulatePages (0, 1, 2, 3, 0, 1, 2, 3);
		}

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
						PopulatePages (3, 0, 1, 2, _totalPages-5 , _totalPages-4, _totalPages -3, _totalPages -2);
						break;
					case 1:
						_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [1], false);

						if (_currentPage >= _totalPages - 1) {
							_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [0], false);
						} else if (_currentPage == _totalPages - 2) {
							_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [3], true);
						}
						PopulatePages (3, 0, 1, 2, _totalPages-5 , _totalPages-4, _totalPages -3, _totalPages -2);
						break;
					case 2:
						_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [1], false);

						if (_currentPage >= _totalPages - 1) {
							_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [0], false);
						} else if (_currentPage == _totalPages - 2) {
							_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [3], true);
						}
						PopulatePages (3, 0, 1, 2, _totalPages-5 , _totalPages-4, _totalPages -3, _totalPages -2);
						break;
					case 3:
						_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [1], false);

						if (_currentPage >= _totalPages - 1) {
							_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [0], false);
						} else if (_currentPage == _totalPages - 2) {
							_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [3], true);
						}
						PopulatePages (3, 0, 1, 2, _totalPages-5 , _totalPages-4, _totalPages -3, _totalPages -2);
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
		MeshRenderer tempMeshRenderer;
		switch (modValue) {
		case 0:
			
			if (doAll) {
				_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [3], true);
				_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [2], true);
				PopulatePages (1, 2, 3, 0, _currentPage-2, _currentPage -1, _currentPage, _currentPage+1);
			}
			_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [3], false);
			_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [2], false);
			break;
		case 1:
			if (doAll) {
				_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [2], true);
				_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [3], true);
				PopulatePages (2, 3, 0, 1, _currentPage-2, _currentPage -1, _currentPage, _currentPage+1);
			}
			_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [3], false);
			_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [2], false);
			break;
		case 2:
			if (doAll) {
				_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [2], true);
				_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [3], true);
				PopulatePages (3, 0, 1, 2, _currentPage-2, _currentPage -1, _currentPage, _currentPage+1);
			}
			_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [2], false);
			_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [3], false);
			break;
		case 3:
			if (doAll) {
				_pageFlipAnimations [2].InstantMoveZ (_inBetweenY [2], true);
				_pageFlipAnimations [3].InstantMoveZ (_inBetweenY [3], true);
				PopulatePages (0, 1, 2, 3, _currentPage-2, _currentPage -1, _currentPage, _currentPage+1);
			}
			_pageFlipAnimations [0].InstantMoveZ (_inBetweenY [3], false);
			_pageFlipAnimations [1].InstantMoveZ (_inBetweenY [2], false);
			break;
		default:
			break;
		}
	}

	// b c
	// a d : this is what the structure is, so the third one is the current page
	void PopulatePages(int a, int b, int c, int d, int aPage, int bPage, int cPage, int dPage){
		MeshRenderer tempMeshRenderer;
		Material[] tempMaterialArray;
		//a
		tempMeshRenderer = _pagePool [a].GetChild (0).GetChild(2).GetComponent<MeshRenderer> ();
		tempMaterialArray = tempMeshRenderer.materials;
		if (_noteBookPages [aPage].pageFrontMaterial != null) {
			tempMaterialArray [1] = _noteBookPages [aPage].pageFrontMaterial;
		} else {
			tempMaterialArray [1] = _defaultPageMaterial;
		}

		if (_noteBookPages [aPage].pageBackMaterial != null) {
			tempMaterialArray [2] = _noteBookPages [aPage].pageBackMaterial;
		} else {
			tempMaterialArray [2] = _defaultPageMaterial;
		}
		tempMeshRenderer.materials = tempMaterialArray;
		//b
		tempMeshRenderer = _pagePool [b].GetChild (0).GetChild (2).GetComponent<MeshRenderer> ();
		tempMaterialArray = tempMeshRenderer.materials;
		if (_noteBookPages [bPage].pageFrontMaterial != null) {
			tempMaterialArray [1] = _noteBookPages [bPage].pageFrontMaterial;
		} else {
			tempMaterialArray [1] = _defaultPageMaterial;
		}

		if (_noteBookPages [bPage].pageBackMaterial != null) {
			tempMaterialArray [2] = _noteBookPages [bPage].pageBackMaterial;
		} else {
			tempMaterialArray [2] = _defaultPageMaterial;
		}
		tempMeshRenderer.materials = tempMaterialArray;
		//c
		tempMeshRenderer = _pagePool [c].GetChild (0).GetChild (2).GetComponent<MeshRenderer> ();
		tempMaterialArray = tempMeshRenderer.materials;
		if (_noteBookPages [cPage].pageFrontMaterial != null) {
			tempMaterialArray [1] = _noteBookPages [cPage].pageFrontMaterial;
		} else {
			tempMaterialArray [1] = _defaultPageMaterial;
		}

		if (_noteBookPages [cPage].pageBackMaterial != null) {
			tempMaterialArray [2] = _noteBookPages [cPage].pageBackMaterial;
		} else {
			tempMaterialArray [2] = _defaultPageMaterial;
		}
		tempMeshRenderer.materials = tempMaterialArray;
		//d
		tempMeshRenderer = _pagePool [d].GetChild (0).GetChild (2).GetComponent<MeshRenderer> ();
		tempMaterialArray = tempMeshRenderer.materials;
		if (_noteBookPages [dPage].pageFrontMaterial != null) {
			tempMaterialArray [1] = _noteBookPages [dPage].pageFrontMaterial;
		} else {
			tempMaterialArray [1] = _defaultPageMaterial;
		}

		if (_noteBookPages [dPage].pageBackMaterial != null) {
			tempMaterialArray [2] = _noteBookPages [dPage].pageBackMaterial;
		} else {
			tempMaterialArray [2] = _defaultPageMaterial;
		}
		tempMeshRenderer.materials = tempMaterialArray;
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
				int switchSidesPageIndex = MathHelpers.Mod ((whichPageToFlip - 2), 4);
				_pageFlipAnimations [switchSidesPageIndex].SwapSides (_inBetweenY [4], _inBetweenY [3]);

				//This is where a new material swap needs to happen
				MeshRenderer tempMeshRenderer = _pagePool[switchSidesPageIndex].GetChild (0).GetChild(2).GetComponent<MeshRenderer>();
				Material[] tempMaterialArray = tempMeshRenderer.materials;
				if (_noteBookPages [_currentPage + 1].pageFrontMaterial != null) {
					tempMaterialArray [1] = _noteBookPages [_currentPage + 1].pageFrontMaterial;
				} else {
					tempMaterialArray [1] = _defaultPageMaterial;
				}

				if (_noteBookPages [_currentPage + 1].pageBackMaterial != null) {
					tempMaterialArray [2] = _noteBookPages [_currentPage + 1].pageBackMaterial;
				} else {
					tempMaterialArray [2] = _defaultPageMaterial;
				}
				tempMeshRenderer.materials = tempMaterialArray;
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
		StartCoroutine (BookSpineLerp(true));
		_waitingForFadeIn = true;
		_currentPage = _currentPage + 1;

		_buttonManagerScript.BeginFade (_currentPage, _totalPages);
	}

	public void FlipPageLeft(){
		//if (_currentPage > 0) {
			_pageToFlip = _currentPage;
			_pageTurnTimer.Reset ();
		StartCoroutine (BookSpineLerp(false));
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
				int switchSidesPageIndex = MathHelpers.Mod ((whichPageToFlip + 2), 4);
				_pageFlipAnimations [switchSidesPageIndex].SwapSides (_inBetweenY [4], _inBetweenY [3]);

				//This is where a new material swap needs to happen This is the other spot
				MeshRenderer tempMeshRenderer = _pagePool[switchSidesPageIndex].GetChild (0).GetChild(2).GetComponent<MeshRenderer>();
				Material[] tempMaterialArray = tempMeshRenderer.materials;
				if (_noteBookPages [_currentPage - 3].pageFrontMaterial != null) {
					tempMaterialArray [1] = _noteBookPages [_currentPage - 3].pageFrontMaterial;
				} else {
					tempMaterialArray [1] = _defaultPageMaterial;
				}

				if (_noteBookPages [_currentPage - 3].pageBackMaterial != null) {
					tempMaterialArray [2] = _noteBookPages [_currentPage - 3].pageBackMaterial;
				} else {
					tempMaterialArray[2] = _defaultPageMaterial;
				}
				tempMeshRenderer.materials = tempMaterialArray;
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
			_renderTextureCamera.enabled = false;
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

	public float GetPageTurnTimerPercentTimePassed(){
		return _pageTurnTimer.PercentTimePassed;
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

	public int GetCurrentPage(){
		return _currentPage; 
	}

	public int GetTotalPage(){
		return _totalPages;
	}

	public Transform GetPageTransform(int index){
		if (_pagePool [index] != null) {
			return _pagePool [index];
		} else {
			return null;
		}
	}


	IEnumerator BookSpineLerp(bool opening){
		bool checkIfValidToContinue = false;
		if ((opening && _currentPage == 0) || (!opening && _currentPage == 1)) {
			checkIfValidToContinue = true;
		}
		if (checkIfValidToContinue) {
			while (!_pageTurnTimer.IsOffCooldown) {
				if (opening) {
					_bookSpine.transform.localRotation = Quaternion.Lerp (_originSpineRotation, _goalSpineRotation, (_pageTurnTimer.PercentTimePassed - 0.5f) * 2f);
				} else {
					_bookSpine.transform.localRotation = Quaternion.Lerp (_goalSpineRotation, _originSpineRotation, (_pageTurnTimer.PercentTimePassed) * 2f);
				}
				yield return null;
			}
			if (opening) {
				_bookSpine.transform.localRotation = _goalSpineRotation;
			} else {
				_bookSpine.transform.localRotation = _originSpineRotation;
			}
		}
		yield return null;
	}
}
