using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockNode : MonoBehaviour {
	[SerializeField] bool isClockActive;
	[SerializeField] float intervalTime;
	Timer _intervalTimer;   // time between two rotations
	[SerializeField] float _angle;
	bool isRotating = false;
	Quaternion originAngle;
	Quaternion finalAngle;


	// Use this for initialization
	void Start () {
		
		_intervalTimer = new Timer (intervalTime);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			isClockActive = !isClockActive;
		}

		if (isClockActive && _intervalTimer.IsOffCooldown) {
			RotateNode (_angle);
			_intervalTimer.Reset ();
		}
			
		
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isRotating) {
			// rotate the node
			if (Mathf.Abs (Quaternion.Angle (transform.localRotation, finalAngle)) > 0.02f) {
				transform.localRotation = Quaternion.Lerp (transform.localRotation, finalAngle, Time.deltaTime * 5f);
			} else {
				transform.localRotation = finalAngle;
				isRotating = false;
			}
		}
	}

	void RotateNode(float amount){
		// temp Rot Degree = 0, 90, 180, 270 
		//float tempRotDegree = transform.rotation.eulerAngles.z;
		if(!isRotating){
			isRotating = true;
			originAngle = transform.localRotation;
			//transform.Rotate(0,0,-90);
			finalAngle = transform.localRotation;
			finalAngle = originAngle * Quaternion.Euler (0, 0, amount);

		}


	}
}
