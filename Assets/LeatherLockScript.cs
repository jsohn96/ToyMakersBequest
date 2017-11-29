using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherLockScript : MonoBehaviour {
	float _upper = 2.4319f;
	float _lower = 2.4259f;
	float _lowest = 2.4199f;
	Vector3 _lowerHeight;
	Vector3 _upperHeight;
	Vector3 _lowestHeight;

	bool _isLowest = false;
	[SerializeField] bool _unlocked = false;

	[SerializeField] PageFlipManagement _pageFlipManagementScript;
	[SerializeField] int _pageNumberToLock = 0;
	int _lastPage = 0;

	void Start(){
		_lowerHeight = transform.localPosition;
		_lowerHeight.y = _lower;
		_upperHeight = _lowerHeight;
		_upperHeight.y = _upper;
		_lowestHeight = _upperHeight;
		_lowestHeight.y = _lowest;

		if (_pageNumberToLock == _pageFlipManagementScript.GetCurrentPage ()) {
			transform.localPosition = _upperHeight;
		} else if (_pageNumberToLock - 1 == _pageFlipManagementScript.GetCurrentPage ()) {
			transform.localPosition = _lowerHeight;
		} else {
			transform.localPosition = _lowestHeight;
			_isLowest = true;
		}
		_lastPage = _pageFlipManagementScript.GetCurrentPage ();
	}

	void Update () {
		if (_pageNumberToLock == _pageFlipManagementScript.GetCurrentPage ()) {
			if (!_pageFlipManagementScript.IsPageTurnDone ()) {
				StartCoroutine (LeatherLockHeightAdjustment (true));
			}
			_isLowest = false;
		} else if (_pageNumberToLock-1 == _pageFlipManagementScript.GetCurrentPage ()) {
			if (!_pageFlipManagementScript.IsPageTurnDone ()) {
				if (_lastPage > _pageFlipManagementScript.GetCurrentPage ()) {
					StartCoroutine (LeatherLockHeightAdjustment (false));
				} else {
					StartCoroutine (LeatherLockHeightAdjustment (false, true));
				}
			}
			_isLowest = false;
		} else if (!_isLowest) {
			_isLowest = true;
			transform.localPosition = _lowestHeight;
		}
		_lastPage = _pageFlipManagementScript.GetCurrentPage ();
	}

	IEnumerator LeatherLockHeightAdjustment(bool goUp, bool isComingFromLeft = false){
		while (!_pageFlipManagementScript.IsPageTurnDone ()) {
			if (goUp) {
				transform.localPosition = Vector3.Lerp (_lowerHeight, _upperHeight, _pageFlipManagementScript.GetPageTurnTimerPercentTimePassed ());
			} else {
				if (!isComingFromLeft) {
					transform.localPosition = Vector3.Lerp (_upperHeight, _lowerHeight, _pageFlipManagementScript.GetPageTurnTimerPercentTimePassed ());
				} else {
					transform.localPosition = _lowestHeight;
					break;
				}
			}
			yield return null;
		}
		if (goUp) {
			transform.localPosition = _upperHeight;
		} else if(!isComingFromLeft){
			transform.localPosition = _lowerHeight;
		}
		yield return null;
	}
}
