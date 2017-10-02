using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepEnd : MonoBehaviour {
	[SerializeField] PeepIn _peepInScript;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "MainCamera") {
			_peepInScript.PeepInTransition (true);
		}
	}
}
