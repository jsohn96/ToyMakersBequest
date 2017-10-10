using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
	public static StateManager stateManager;

	// Use this for initialization
	void Start () {
		if (stateManager == null) {
			DontDestroyOnLoad (gameObject);
			stateManager = this;
		}
		else if (stateManager != this) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
