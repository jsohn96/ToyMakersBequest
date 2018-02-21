using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreLighting : MonoBehaviour {
	[SerializeField] Light[] _initialLights;
	[SerializeField] Light[] _nextLights;
	// Use this for initialization
	public void MoveToNextLights(){
		for (int i = 0; i < _initialLights.Length; i++) {
			_initialLights [i].enabled = false;
		}
		for (int i = 0; i < _nextLights.Length; i++) {
			_nextLights [i].enabled = true;
		}
	}
}
