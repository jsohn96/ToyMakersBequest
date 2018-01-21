using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStaticRuntime : MonoBehaviour {
	void Start () {
		Transform[] allChildren = GetComponentsInChildren<Transform> ();
		foreach (Transform child in allChildren) {
			child.gameObject.isStatic = false;
		}
		gameObject.isStatic = false;
	}
}
