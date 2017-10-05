using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePickUp : MonoBehaviour {
	Pickupable _thisPickupableScript;
	bool _hasNoteBeenPickedUp = false;
	// Use this for initialization
	void Awake () {
		_thisPickupableScript = GetComponent<Pickupable> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!_hasNoteBeenPickedUp) {
			if (_thisPickupableScript.isPickedUp) {
				_hasNoteBeenPickedUp = true;
			}
		}
	}
}
