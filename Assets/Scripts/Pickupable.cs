using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour {

	public int identifier
	{
		get {return _identifier; }
		set {_identifier = value; }
	}

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

	public bool isInDropZone
	{
		get {return _isInDropZone; }
		set {_isInDropZone = value; }
	}

	[SerializeField] int _identifier = 0;
	bool _isInBox = false;
	bool _isPickedUp = false;
	bool _isInDropZone = false;
}
