using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreCabinet : MonoBehaviour {
	TheatreDancer dancerScript;
	bool isActivated;
	bool isDancerOut;

	// Use this for initialization
	void Awake () {
		dancerScript = FindObjectOfType<TheatreDancer> ().GetComponent<TheatreDancer> ();
		isActivated = false;
		isDancerOut = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTouchDown(){
		// 
		if (!isActivated ) {
			Debug.Log ("Empty cabinet open");
		} else if(isActivated && !isDancerOut) {
			Debug.Log ("cabinet open with dancer");
			dancerScript.DancerEnterScene ();
			isDancerOut = true;
		}
	}

	public void Activate(){
		if (!isActivated) {
			isActivated = true;
		}
	}
}
