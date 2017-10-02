using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAfterPickup : MonoBehaviour {
	[SerializeField] int _whichIndex = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void HideObjects(PickedUpGearEvent e){
		if (e.WhichGear == _whichIndex) {
			gameObject.SetActive (false);
		}
	}

	void OnEnable(){
		Events.G.AddListener<PickedUpGearEvent> (HideObjects);
	}
	void OnDisable(){
		Events.G.RemoveListener<PickedUpGearEvent> (HideObjects);
	}
}
