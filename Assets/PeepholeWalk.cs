using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepholeWalk : MonoBehaviour {
	[SerializeField] PeepIn _peepInScript;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_peepInScript._isPeepingIn) {
			if (Input.GetAxis ("Mouse ScrollWheel") > 0f) {
				transform.Translate (Vector3.forward * Time.deltaTime * 70.0f);
			}
			if (Input.GetAxis ("Mouse ScrollWheel") < 0f) {
				transform.Translate (Vector3.back * Time.deltaTime * 70.0f);
			}

			if (transform.localPosition.z < -10f) {
				transform.localPosition = new Vector3 (-25.0f, 0.0f, -10.0f);
			}
		}
	}
}
