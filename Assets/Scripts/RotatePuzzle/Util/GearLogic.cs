using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// utility for rotating nodes 
// when nodes are connected as gears 

public class GearLogic : MonoBehaviour {
	[SerializeField] GearLogic[] _connectedGears;
	[SerializeField] int _teethCount;

	float _speed = 1f;
	Quaternion _originalRot;


	// Use this for initialization
	void Start () {
		_originalRot = transform.localRotation;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_connectedGears != null && _connectedGears.Length > 0) {
			checkRotation ();
		}

	}

	void checkRotation(){
		Quaternion curRot = transform.localRotation;
		//print ("$$$$$$$Turn: " + Mathf.Abs(Quaternion.Angle(curRot, _originalRot)));
		if (Mathf.Abs(Quaternion.Angle(curRot, _originalRot)) >= 0.001f) {
			//print("called inside");
			float amount = curRot.eulerAngles.z - +_originalRot.eulerAngles.z;
			foreach (GearLogic gl in _connectedGears) {
				gl.Turn (_teethCount, amount);
			}
			_originalRot = curRot;
		}
		
	}

	public void Turn(int driverTeethCount, float turnAmount){
		//print ("$$$$$$$Turn called");
		float ratio = driverTeethCount / _teethCount;
		// rotate
		Quaternion deltaRot = Quaternion.Euler (0, 0, -turnAmount * ratio);
		Quaternion curRot = transform.localRotation;
		curRot = curRot * deltaRot;
		transform.localRotation = curRot;
	}

}
