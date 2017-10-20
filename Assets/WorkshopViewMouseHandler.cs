using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopViewMouseHandler : MouseOverHandler {
	[SerializeField] shaderGlowCustom _shaderGlowCustomScript;

	void Start () {
		
	}
	
	void Update () {
		if (isPointerIn) {
			if (Input.GetMouseButtonDown (0)) {
				Debug.Log(this.name + " clicked:");
			}
		}
	}

	public override void OtherPointerEnter ()
	{
		base.OtherPointerEnter ();
		_shaderGlowCustomScript.OtherPointerEnter ();
	}

	public override void OtherPointerExit ()
	{
		base.OtherPointerExit ();
		_shaderGlowCustomScript.OtherPointerExit ();
	}
}
