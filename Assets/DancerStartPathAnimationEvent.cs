using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancerStartPathAnimationEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// called in animtion exit the closet
	void DancerStartPath(){
		Events.G.Raise(new DancerMoveOnPathEvent());
	}
}
