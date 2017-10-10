using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePickUp : MonoBehaviour {
	Pickupable _thisPickupableScript;
	[SerializeField] GameObject _text;
	[SerializeField] GameObject _guideText;
	bool _hasNoteBeenPickedUp = false;
	// Use this for initialization
	void Awake () {
		_thisPickupableScript = GetComponent<Pickupable> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!_hasNoteBeenPickedUp) {
			if (_thisPickupableScript.isPickedUp) {
				_text.SetActive (false);
				_guideText.SetActive (true);
				_hasNoteBeenPickedUp = true;
			}
		}
	}
}
