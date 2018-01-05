using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInteraction : MonoBehaviour {

	int _3DBookObjectLayerMask = 1 << 15;
	[SerializeField] Camera _3dObjectCamera;
	[SerializeField] TextContentTracker _textContentTracker;
	
	void Update () {
		CheckForInteract ();
	}

	void CheckForInteract(){
		if (Input.GetMouseButtonDown (0)) {
			if (!_textContentTracker._isDisplaying) {
				Ray ray = _3dObjectCamera.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit, Mathf.Infinity, _3DBookObjectLayerMask)) {
					BookInteractive thisBookInteractive = hit.collider.GetComponent<BookInteractive> ();
					if (thisBookInteractive != null) {
						thisBookInteractive.Interact ();
					}
				}
			}
		}
	}
}
