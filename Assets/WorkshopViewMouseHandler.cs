using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopViewMouseHandler : MouseOverHandler {
	[SerializeField] shaderGlowCustom _shaderGlowCustomScript;
	[SerializeField] Vector3 _viewPos, _viewRot;

	Zoom _zoomScript;
	bool _banZoom = false;
	bool _sourceOfZoom = false;
	[SerializeField] float _zoomDuration = 1.5f;

	void Start () {
		
	}
	
	void Update () {
		if (isPointerIn && !_banZoom) {
			if (Input.GetMouseButtonUp (0)) {
				_zoomScript = Camera.main.GetComponent<Zoom> ();
				_sourceOfZoom = true;
				Events.G.Raise (new WorkshopItemClicked (true));
				_zoomScript.ZoomIn (_viewPos, _viewRot, _zoomDuration);
				Debug.Log(this.name + " clicked:");
			}
		}
	}

	void WorkshopToggle (WorkshopItemClicked e) {
		if (e.Zoom) {
			_shaderGlowCustomScript.DisablePointerEnter (true);
			OtherPointerExit ();
			if (!_sourceOfZoom) {
				_banZoom = true;
			}
		} else {
			_shaderGlowCustomScript.DisablePointerEnter (false);
			if (!_sourceOfZoom) {
				_banZoom = false;
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

	void OnEnable(){
		Events.G.AddListener<WorkshopItemClicked> (WorkshopToggle);
	}
	void OnDisable(){
		Events.G.RemoveListener<WorkshopItemClicked> (WorkshopToggle);
	}
}
