using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLink : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 GetLinkPosition(){
		//return local positions 
		return transform.localPosition;
	}
}
