using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour {

	public bool isInBox
	{
		get {return _isInBox; }
		set {_isInBox = value; }
	}

	public bool isPickedUp
	{
		get {return _isPickedUp; }
		set {_isPickedUp = value; }
	}

	bool _isInBox = false;
	bool _isPickedUp = false;
}
