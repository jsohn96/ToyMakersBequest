using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreCabinet : MonoBehaviour {
	TheatreDancer dancerScript;
	bool isActivated;
	bool isDancerOut;
	[SerializeField] Animator _cabinetAnimator;
	BoxCollider _boxCollider;
	bool _isOpen = false;

	// Use this for initialization
	void Awake () {
		dancerScript = FindObjectOfType<TheatreDancer> ().GetComponent<TheatreDancer> ();
		isActivated = false;
		isDancerOut = false;
	}

	void Start(){
		_boxCollider = GetComponent<BoxCollider> ();
		if (AltTheatre.currentSate < TheatreState.magicianRight) {
			_boxCollider.enabled = false;
			isActivated = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTouchDown(){
		if (isActivated) {
			if (!_isOpen) {
				_isOpen = true;
			} else {
				_isOpen = false;
			}
		}

//		if (!isActivated ) {
//			Debug.Log ("Empty cabinet open");
//		} else if(isActivated && !isDancerOut) {
//			Debug.Log ("cabinet open with dancer");
//			isDancerOut = true;
//		}
	}

	public void Activate(bool activate){
		isActivated = activate;
		_boxCollider.enabled = activate;
	}
}
