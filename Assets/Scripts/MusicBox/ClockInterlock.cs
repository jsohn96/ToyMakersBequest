using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// keyNode: node that locks the current node
// lockNode: current node




[System.Serializable]
public class NodeKey{
	public int relativeNodeIdx;
	public float unlockAngle;
	public GameObject[] key;        // the intersection part 
	public bool isUnlock;         // is the key is in the correct angle, the lock is un locked

}


[System.Serializable]
public class NodeKeyPool{
	public int PoolProvideIdx;
	public List<NodeKey> NodeKeys;

//	public NodeKeyPool(){
//		NodeKeys = new List<NodeKey> ();
//	}

} 



// for nodes that in the lv3 with auto rotation 
[RequireComponent(typeof(PathNode))]
public class ClockInterlock : MonoBehaviour {
	int requireKeysNumer = 0;
	[SerializeField] List<NodeKeyPool> _keyPools;
	[SerializeField] PathNode _myNode;
	[SerializeField] ClockNode _myClockNode;
	PathNetwork _myPathNet;
	List<GameObject> _activeKeys;
	bool isNodeActive = false;
	int foundKeysNumber = 0;

	// Use this for initialization
	void Awake () {
		if (GetComponent<PathNode> () != null) {
			_myNode = GetComponent<PathNode> ();
		} else {
			Debug.LogError ("require pathnode component");
		}

		if (GetComponentInParent<PathNetwork> () != null) {
			_myPathNet = GetComponentInParent<PathNetwork> ();
		} else {
			Debug.LogError ("requrire pathnetwork component");
		}

		if (GetComponent<ClockNode> () != null) {
			_myClockNode = GetComponent<ClockNode> ();
		}

		initLockNode ();
	}

	void initLockNode(){
		if (_keyPools != null) {
			requireKeysNumer = _keyPools.Count;
			_activeKeys = new List<GameObject> ();
		} else {
			requireKeysNumer = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckInterlockConnection ();
	}



	// 
	void CheckInterlockConnection(){
		foundKeysNumber = 0;
		_activeKeys.Clear ();
		UpdateKeyPool ();
		//Debug.Log (_myNode.readNodeInfo().index + " check active number: " + foundKeysNumber + " " + requireKeysNumer);
		if (requireKeysNumer > 0 && foundKeysNumber >= requireKeysNumer) {
			isNodeActive = true;
			_myNode.SetCheckClockInterlock (isNodeActive, _activeKeys);
			if (_myClockNode) {
				_myClockNode.SetClockCircle (isNodeActive, _activeKeys);
			}
		} else if (requireKeysNumer > 0 && foundKeysNumber != requireKeysNumer) {
			isNodeActive = false;
			_myNode.SetCheckClockInterlock (isNodeActive, null);
			if (_myClockNode) {
				_myClockNode.SetClockCircle (isNodeActive, null);
			}
		} else if(requireKeysNumer == 0) {
			isNodeActive = true;
			_myNode.SetCheckClockInterlock (isNodeActive, null);
			if (_myClockNode) {
				_myClockNode.SetClockCircle (isNodeActive, null);
			}
			
		}
		
	}


	void UpdateKeyPool(){
		// update keypool connection 
		float tempRotZ = gameObject.transform.localEulerAngles.z;
		foreach (NodeKeyPool nkpool in _keyPools) {
			PathNode providerNode = _myPathNet.FindNodeWithIndex (nkpool.PoolProvideIdx);
			foreach (NodeKey nkey in nkpool.NodeKeys) {
				// compare angle 
				if (nkey.relativeNodeIdx == -1) {
					// absolute angle 
					if (Mathf.Abs(AngleUtil.DampAngle (providerNode.gameObject.transform.localEulerAngles.z) - AngleUtil.DampAngle (nkey.unlockAngle)) <= 0.1f) {
						if (nkey.key.Length >= 2) {
							foreach (GameObject k in nkey.key) {
								if (!_activeKeys.Contains (k)) {
									_activeKeys.Add (k);

								}
							}
							foundKeysNumber += 1;
							nkey.isUnlock = true;
							break;
						} else if (nkey.key.Length <= 0 || nkey.key == null) {
							foundKeysNumber += 1;
							_activeKeys.Clear ();
							nkey.isUnlock = true;
						} else{
							if (!_activeKeys.Contains (nkey.key[0])) {
								nkey.isUnlock = true;
								_activeKeys.Add (nkey.key[0]);
								foundKeysNumber += 1;
								break;
							}
						}


					} else {
						nkey.isUnlock = false;
					}

				} else {
					PathNode relativeNode = _myPathNet.FindNodeWithIndex (nkey.relativeNodeIdx);
					// TODO check angle difference
					float angledifference = Mathf.Abs(Mathf.Abs(AngleUtil.DampAngle(providerNode.gameObject.transform.localEulerAngles.z)-AngleUtil.DampAngle(relativeNode.gameObject.transform.localEulerAngles.z)) - nkey.unlockAngle);
					angledifference = Mathf.Min (angledifference, 360f - angledifference);
					//Debug.Log ("$$$ check angle interlock : " + angledifference);
					if ( angledifference <= 0.1f) {
						if (nkey.key.Length >= 2) {
							foreach (GameObject k in nkey.key) {
								if (!_activeKeys.Contains (k)) {
									_activeKeys.Add (k);

								}
							}
							foundKeysNumber += 1;
							nkey.isUnlock = true;
							break;
						} else if (nkey.key.Length <= 0 || nkey.key == null) {
							foundKeysNumber += 1;
							nkey.isUnlock = true;
						} else{
							if (!_activeKeys.Contains (nkey.key[0])) {
								nkey.isUnlock = true;
								_activeKeys.Add (nkey.key[0]);
								foundKeysNumber += 1;
								break;
							}
						}
					} else {
						nkey.isUnlock = false;
					}
				}
			}// end of check all keys in keypool 
		}// end of checking all keypool 
	}
}
