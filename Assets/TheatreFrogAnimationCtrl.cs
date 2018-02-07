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

	BoxCollider bCol;
	// Use this for initialization
	void Awake(){
		_frogAnim = GetComponent<Animator> ();
		bCol = GetComponent<BoxCollider> ();

	}

	void Start () {
		if (!_isFrogUp) {
			_frog.SetActive (false);
		}
		_frogIdx = GetComponentInParent<PathNode> ().readNodeInfo ().index;

	}

	// Update is called once per frame
	void Update () {
		if (_isFrogUp!=_isPreviousUp) {
			if (_isFrogUp) {
				_frog.SetActive (true);
				_frogAnim.Play ("frog_getup");
			} else {
				_frogAnim.Play ("frog_getdown");
			}
			_isPreviousUp = _isFrogUp;
		
		}

	}

	public void SetControl(bool isactive){
		Debug.Log ("Activate Frog Contorl : " + _frogIdx);
		_isControlActive = isactive;
	}


	public void ShowFrog(){
		print ("activate frog: " + _frogIdx);
		_isFrogUp = true;
		_isClickActive = true;
		bCol.enabled = true;
	}

	public void HideFrog(){
		_isFrogUp = false;
		_isClickActive = false;
		bCol.enabled = false;
	}

	void hideFrogInScene(){
		_frog.SetActive (false);
		_isClickActive = false;
		bCol.enabled = false;
	}

	void OnTouchDown(){
		if (_isControlActive && _isFrogUp) {
			Debug.Log ("Click on Frog : " + _frogIdx);
			HideFrog ();
			Events.G.Raise (new TheatreFrogClickEvent (_frogIdx));
		}

	}

}
