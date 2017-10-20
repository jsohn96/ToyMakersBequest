using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverHandler : MonoBehaviour {
	//For Each Object

	public bool isPointerIn {
		get { return _isPointerIn; }
		set { _isPointerIn = value; }
	}
	bool _isPointerIn = false;

	virtual public void OtherPointerEnter() {
		_isPointerIn = true;
	}

	virtual public void OtherPointerExit() {
		_isPointerIn = false;
	}
}
