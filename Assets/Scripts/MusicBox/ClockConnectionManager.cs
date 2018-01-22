using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// when circle matches relative angle , handles the circle angel check activation 
// for circles which are not interlocking 

[System.Serializable]
public class ActivateRecord{
	public int activateIdx;
	public List<Connection> connections;
	public bool isIn;
	
}

public class ChangeRecord{
	public int changeAtIdx;
	public int changeValIdx;
	public Connection connection;
	
}

[System.Serializable]
public class Connection{
	public int fromIdx;
	public int toIdx;
	public float relativeAngle;
}

public class ClockConnectionManager : MonoBehaviour {
	[SerializeField] ActivateRecord[] _clockActivateRecords;
	[SerializeField] ChangeRecord[] _changeNodeRecords;
	[SerializeField] PathNetwork _myPathNetwork;


	// Use this for initialization
	void Awake () {
		_myPathNetwork = GetComponent<PathNetwork> ();
	}
	
	// Update is called once per frame
	void Update () {
		CheckingActivateRecords ();
	}

	// everytime the clc ticks
	public void CheckingActivateRecords(){
		// traverse all the records to see if the connection is updated
		// check the angles 
		if(_clockActivateRecords != null){
			foreach (ActivateRecord r in _clockActivateRecords){
				bool isConnecting = true;
				foreach (Connection cnn in r.connections) {
					PathNode fromPn = _myPathNetwork.FindNodeWithIndex (cnn.fromIdx);
					if (cnn.toIdx < 0) {
						if (Mathf.Abs (AngleUtil.DampAngle (fromPn.gameObject.transform.localEulerAngles.z) - AngleUtil.DampAngle (cnn.relativeAngle)) < 0.001f) {
							isConnecting = true;
						} else {
							isConnecting = false;
							break;
						}
					} else {
						PathNode toPn = _myPathNetwork.FindNodeWithIndex (cnn.toIdx);
						float angledifference = Mathf.Abs (Mathf.Abs (AngleUtil.DampAngle (fromPn.gameObject.transform.localEulerAngles.z) - AngleUtil.DampAngle (toPn.gameObject.transform.localEulerAngles.z))
							- AngleUtil.DampAngle (cnn.relativeAngle));
						angledifference = Mathf.Min (angledifference, 360f - angledifference);
						//Debug.Log ("$$check angle diff: " + angledifference);
						if(angledifference< 0.001f){
							isConnecting = true;
						} else {
							isConnecting = false;
							break;
						}

					}
				}// for each connection check 
				PathNode activePn = _myPathNetwork.FindNodeWithIndex (r.activateIdx);
				if (isConnecting) {
					activePn.SetCheckConnection (true, r.isIn);
				} else {
					activePn.SetCheckConnection (false, r.isIn);
				}
			}// for each records 
			
		}


		if (_changeNodeRecords != null) {
			foreach (ChangeRecord r in _changeNodeRecords){
				bool isConnecting = true;

				PathNode fromPn = _myPathNetwork.FindNodeWithIndex (r.connection.fromIdx);
				if (r.connection.toIdx < 0) {
					if (Mathf.Abs (AngleUtil.DampAngle (fromPn.gameObject.transform.localEulerAngles.z) - AngleUtil.DampAngle (r.connection.relativeAngle)) < 0.001f) {
						_myPathNetwork.ChangePathnetworkValue (r.changeAtIdx, r.changeValIdx);
					} 
				} else {
					PathNode toPn = _myPathNetwork.FindNodeWithIndex (r.connection.toIdx);
					float angledifference = Mathf.Abs (Mathf.Abs (AngleUtil.DampAngle (fromPn.gameObject.transform.localEulerAngles.z) - AngleUtil.DampAngle (toPn.gameObject.transform.localEulerAngles.z))
						- AngleUtil.DampAngle (r.connection.relativeAngle));
					angledifference = Mathf.Min (angledifference, 360f - angledifference);
					//Debug.Log ("$$check angle diff: " + angledifference);
					if(angledifference< 0.001f){
						_myPathNetwork.ChangePathnetworkValue (r.changeAtIdx, r.changeValIdx);
					} 

				}


			}// for each records
		
		}
		 
	}
}
