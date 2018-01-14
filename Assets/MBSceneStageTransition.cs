using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBSceneStageTransition : MonoBehaviour {
	bool isRotating = false;
	Quaternion originAngle;
	Quaternion finalAngle;
	float rotateAmount; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isRotating) {

			// rotate the node
			if (Mathf.Abs (Quaternion.Angle (transform.localRotation, finalAngle)) > 0.02f) {
				transform.localRotation = Quaternion.Lerp (transform.localRotation, finalAngle, Time.deltaTime * 10f);
			} else {
				transform.localRotation = finalAngle;
				isRotating = false;
			}
		}
	}

	public void RotateNode(float amount){
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
