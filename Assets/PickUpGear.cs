using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGear : MonoBehaviour {
	[SerializeField] Camera _insideCamera;
	int _peepHoleLayer = 1 << 14;
	[SerializeField] int _gearIndex = 1;
	[SerializeField] AudioSource _audioSource;
	bool _gearsReadyForPickup = false;
	void Awake(){
		
	}

	void Update () {
		if (_gearsReadyForPickup) {
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = _insideCamera.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, _peepHoleLayer)) {
					if (hit.collider.name == gameObject.name) {
						Events.G.Raise (new PickedUpGearEvent (_gearIndex));
						_audioSource.Play ();
						gameObject.SetActive (false);
					} 
				} 
			}
		}
	}

	void GearsReady(GearsReadyForPickupEvent e){
		_gearsReadyForPickup = true;
	}

	void OnEnable(){
		Events.G.AddListener<GearsReadyForPickupEvent> (GearsReady);
	}
	void OnDisable(){
		Events.G.RemoveListener<GearsReadyForPickupEvent> (GearsReady);
	}
}
