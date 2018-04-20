using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTicker : MonoBehaviour {
	[SerializeField] BoxCollider _festivalDropZone;
	[SerializeField] BoxCollider _trueEndDropZone;
	bool _isPointingAtTrue = false;
	[SerializeField] BackTickRotation _backTickerRotation;

	void Start(){
		if (_isPointingAtTrue) {
			_trueEndDropZone.enabled = false;
		} else {
			_festivalDropZone.enabled = false;
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "DropZone") {
			_isPointingAtTrue = !_isPointingAtTrue;
			_backTickerRotation.SetTicker (_isPointingAtTrue);
			if (_isPointingAtTrue) {
				_trueEndDropZone.enabled = false;
				_festivalDropZone.enabled = true;
			} else {
				_trueEndDropZone.enabled = true;
				_festivalDropZone.enabled = false;
			}
		}
	}
}
