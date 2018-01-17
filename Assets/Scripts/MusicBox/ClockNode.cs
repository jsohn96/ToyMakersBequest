using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockNode : MonoBehaviour {
	[SerializeField] int clockCircleID;
	[SerializeField] bool isClockActive;
	[SerializeField] float intervalTime;
	Timer _intervalTimer;   // time between two rotations
	[SerializeField] float _angle;
	public bool isRotating = false;
	Quaternion originAngle;
	Quaternion finalAngle;


	// Use this for initialization
	void Start () {
		isClockActive = true;
		_intervalTimer = new Timer (intervalTime);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			//isClockActive = !isClockActive;
			RotateNode (_angle);
		}

		if (isClockActive && _intervalTimer.IsOffCooldown) {
			
			_intervalTimer.Reset ();
		}
			
		
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isRotating) {
			// rotate the node


			float diffAngle = Mathf.Abs(transform.localEulerAngles.z - finalAngle.eulerAngles.z );
			diffAngle = Mathf.Min (diffAngle, 360f - diffAngle);

			//Debug.Log("### " + diffAngle);

			if (diffAngle > 0.05f) {
				transform.localRotation = Quaternion.Slerp (transform.localRotation, finalAngle, Time.deltaTime * 5f);
			} else {
				Debug.Log ("reset rotation");
				transform.localRotation = finalAngle;
				isRotating = false;
			}
		}
	}

	void RotateNode(float amount){
		Debug.Log ("bug check: isrotating val" + isRotating);
		// temp Rot Degree = 0, 90, 180, 270 
		//float tempRotDegree = transform.rotation.eulerAngles.z;
		if(!isRotating){
			isRotating = true;
			originAngle = transform.localRotation;
			//transform.Rotate(0,0,-90);
			finalAngle = originAngle;
			finalAngle = originAngle * Quaternion.Euler ( 0, 0, amount);

		}
	}
}
