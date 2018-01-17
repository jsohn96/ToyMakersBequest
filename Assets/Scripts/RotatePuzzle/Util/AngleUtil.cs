using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleUtil : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static float DampAngle(float angle){
		if (angle >= 360) {
			angle -= 360;
			return DampAngle (angle);
		} else if (angle < 0) {
			angle += 360;
			return DampAngle (angle);
		} else {
			return angle;
			//break;
		}

		//return angle;
	}
}
