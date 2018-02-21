using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreFrogAnimationCtrl : MonoBehaviour {

	Animator _frogAnim;
	[SerializeField] GameObject _frog;
	[SerializeField] int _frogIdx = -1;
	bool _isFrogUp = false;
	bool _isPreviousUp = false;
	bool _isControlActive = false;
	bool _isClickActive = false;
	[SerializeField] int _iconIdx;
	bool _isIconMatched = false;
	[SerializeField] bool _isFrogOn = false;
	bool _isClicked = false;

	BoxCollider bCol;
	// Use this for initialization
	void Awake(){
		_frogAnim = GetComponent<Animator> ();
		bCol = GetComponent<BoxCollider> ();
		bCol.enabled = false;

	}

	void Start () {
		if (!_isFrogUp) {
			_frog.SetActive (false);
		}
		_frogIdx = GetComponentInParent<PathNode> ().readNodeInfo ().index;

	}

	// Update is called once per frame
	void Update () {
//		if (_isFrogUp!=_isPreviousUp) {
//			if (_isFrogUp) {
//				_frog.SetActive (true);
//				_frogAnim.Play ("frog_getup");
//			} else {
//				_frogAnim.Play ("frog_getdown");
//			}
//			_isPreviousUp = _isFrogUp;
//		
//		}

	}

	public void SetControl(bool isactive){
		Debug.Log ("Activate Frog Contorl : " + _frogIdx);
		_isControlActive = isactive;
		if (_isControlActive) {
			bCol.enabled = true;
		} else {
			bCol.enabled = false;
		}

	}

	public void PlayFrogSound(){
		TheatreSound._instance.PlayFrogSound ();
	}


	public void ShowFrog(){
		print ("activate frog: " + _frogIdx);
		_isFrogUp = true;
		if (_isFrogOn) {
			_frog.SetActive (true);
			_frogAnim.Play ("frog_getup");
		} else {
			_frogAnim.Play ("frog_lilipad_flipDown");
		}
		//_isClickActive = true;
		//bCol.enabled = true;
	}

	public void HideFrog(){
		_isFrogUp = false;
		if (_isFrogOn) {
			_frogAnim.Play ("frog_getdown");
		} else {
			_frogAnim.Play ("frog_lilipad_flipBack");
		}
//		if (_isPreviousUp != _isFrogUp) {
//			_isPreviousUp = _isFrogUp;
//
//		}
		//_isClickActive = false;
		//bCol.enabled = false;
	}

	public void ResetFrog(){
		// if is frogup == false 
		_isFrogUp = false;
		_frogAnim.Play ("frog_lilipad_flipBack");
		_isClicked = false;
	}

	public void SetFrogOn(bool frogjumpon){
		_isFrogOn = frogjumpon;
		Debug.Log ("Check frog on:  " + _isFrogOn);
	}

	void hideFrogInScene(){
		_frog.SetActive (false);
		//_isClickActive = false;
		//bCol.enabled = false;
	}


	// when clicked on the object 
	void OnTouchDown(){
//		if (_isControlActive && _isFrogUp) {
//			Debug.Log ("Click on Frog : " + _frogIdx);
//			HideFrog ();
//			Events.G.Raise (new TheatreFrogClickEvent (_frogIdx));
//		}
		if(_isControlActive && !_isClicked){
			//HideFrog ();
			//Debug.Log ("Click on Frog : " + _frogIdx);
			if (_isFrogUp) {
				HideFrog ();
			} else {
				ShowFrog ();
			}
			Events.G.Raise (new TheatreFrogClickEvent (_frogIdx, _iconIdx));
			_isClicked = true;
		}

	}

}
