using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {
	bool _pickUpReady = true;
	[SerializeField] LevelManager _levelManager;

	void OnTouchDown(Vector3 touchPoint){
		if (_pickUpReady) {
			_levelManager.PickUpCharm ();
			_pickUpReady = false;
			gameObject.SetActive (false);
		}
	}
}
