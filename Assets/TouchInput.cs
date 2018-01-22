﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {
	[SerializeField] Camera _camera;
	public LayerMask touchInputMask;

	List<GameObject> touchList = new List<GameObject> ();
	GameObject[] touchesOld;
	RaycastHit hit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		#if UNITY_EDITOR
		if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {
			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo (touchesOld);
			touchList.Clear ();

			Ray ray = _camera.ScreenPointToRay (Input.mousePosition);


				if (Physics.Raycast (ray, out hit, touchInputMask)) {
					GameObject recipient = hit.transform.gameObject;
					touchList.Add (recipient);

				if (Input.GetMouseButtonDown(0) ) {
						recipient.SendMessage ("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
					}
				if (Input.GetMouseButtonUp(0)) {
						recipient.SendMessage ("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
					}
				if (Input.GetMouseButton(0)) {
						recipient.SendMessage ("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
				}
			}
			foreach (GameObject g in touchesOld) {
				if (!touchList.Contains (g)) {
					g.SendMessage ("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		#endif

		if (Input.touchCount > 0) {
			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo (touchesOld);
			touchList.Clear ();

			foreach (Touch touch in Input.touches) {
				Ray ray = _camera.ScreenPointToRay (touch.position);


				if (Physics.Raycast (ray, out hit, touchInputMask)) {
					GameObject recipient = hit.transform.gameObject;
					touchList.Add (recipient);

					if (touch.phase == TouchPhase.Began) {
						recipient.SendMessage ("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if (touch.phase == TouchPhase.Ended) {
						recipient.SendMessage ("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
						recipient.SendMessage ("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if (touch.phase == TouchPhase.Canceled) {
						recipient.SendMessage ("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			foreach (GameObject g in touchesOld) {
				if (!touchList.Contains (g)) {
					g.SendMessage ("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
