using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBFrogAnimationBehaviour : MonoBehaviour {
	Animator _frogAnim;
	[SerializeField] GameObject _frog;
	bool _isFrogUp = false;
	bool _isPreviousUp = false;

	// Use this for initialization
	void Awake(){
		_frogAnim = GetComponent<Animator> ();
	}

	void Start () {
		if (!_isFrogUp) {
			_frog.SetActive (false);
		}

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


	public void ShowFrog(){
		print ("activate frog");
		_isFrogUp = true;
	}

	public void HideFrog(){
		_isFrogUp = false;
	}

	void hideFrogInScene(){
		_frog.SetActive (false);
	}

}
