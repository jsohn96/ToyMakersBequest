using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopViewMouseHandler : MouseOverHandler {
	[SerializeField] shaderGlowCustom _shaderGlowCustomScript;
	[SerializeField] Vector3 _viewPos, _viewRot;

	Zoom _zoomScript;

	void Start () {
		
	}
	
	void Update () {
		if (isPointerIn) {
			if (Input.GetMouseButtonDown (0)) {
				_zoomScript = Camera.main.GetComponent<Zoom> ();
				_zoomScript.ZoomIn (_viewPos, _viewRot);
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
