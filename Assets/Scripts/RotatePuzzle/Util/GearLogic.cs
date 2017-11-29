using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// utility for rotating nodes 
// when nodes are connected as gears 

public class GearLogic : MonoBehaviour {
	[SerializeField] GearLogic[] _connectedGears;
	[SerializeField] float _teethCount;
	public int nodeIndex;
	int _drivenBy = -1;
	[SerializeField] bool _isSelfDriven = false;


	float _speed = 1f;
	Quaternion _originalRot;

	void OnEnable(){
		Events.G.AddListener<MBNodeRotate> (GearRotateActivateHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<MBNodeRotate> (GearRotateActivateHandle);
	}


	// Use this for initialization
	void Start () {
		_originalRot = transform.localRotation;
		if (gameObject.GetComponent<PathNode> ()) {
			nodeIndex = gameObject.GetComponent<PathNode> ().readNodeInfo ().index;
			gameObject.GetComponent<PathNode> ().SetAngleSnapWhenMouseUp (false);
		} else {
			nodeIndex = 8080;
		}
		
	}


	void GearRotateActivateHandle(MBNodeRotate e){
		if (e.nodeIndex == nodeIndex) {
			_isSelfDriven = e.isRoating;
		}
	
	}

	// Update is called once per frame
	void Update () {
		if (_isSelfDriven) {
			_drivenBy = -1;
			ActivateRotation ();
		}
	}

	void ActivateRotation(){
		Quaternion curRot = transform.localRotation;
		//print ("$$$$$$$Turn: " + Mathf.Abs(Quaternion.Angle(curRot, _originalRot)));
		if (Mathf.Abs(Quaternion.Angle(curRot, _originalRot)) >= 0.001f) {
			//print("called inside");
			float amount = curRot.eulerAngles.z - _originalRot.eulerAngles.z;
			if (_connectedGears != null && _connectedGears.Length > 0) {
				foreach (GearLogic gl in _connectedGears) {
					//print ("Cur Node " + nodeIndex + "Driven by " + _drivenBy + " send to " + gl.nodeIndex);

					gl.Turn (_teethCount, amount, nodeIndex);


				}
			}

			_originalRot = curRot;
			//_drivenBy = -1;
		}

	}

	public void Turn(float driverTeethCount, float turnAmount, int driverIdx){
		if (driverIdx != nodeIndex && !_isSelfDriven) {
			
			float ratio = driverTeethCount / _teethCount;
			float turnDelta = -turnAmount * ratio;
			// rotate
			//print ("Cur Node " + nodeIndex + "amount : " + turnAmount + "driven t " + driverTeethCount + "ra");
			Quaternion deltaRot = Quaternion.Euler (0, 0, turnDelta);
			Quaternion curRot = transform.localRotation;
			curRot = curRot * deltaRot;
			transform.localRotation = curRot;
			_originalRot = curRot;
			_drivenBy = driverIdx;

			if (_connectedGears != null && _connectedGears.Length > 0) {
				foreach (GearLogic gl in _connectedGears) {
					//print ("Secondary turn || Cur Node " + nodeIndex + "Driven by " + _drivenBy + " send to " + gl.nodeIndex);
					if (gl.nodeIndex != _drivenBy) {
						gl.Turn (_teethCount, turnDelta, nodeIndex);
					}

				}
			}
		}


	}

}
