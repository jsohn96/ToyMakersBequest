using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldLadyLightControl : MonoBehaviour {
	[SerializeField] GameObject[] _lights = new GameObject[5];
	[SerializeField] OldLadyPath _oldLadyPath;
	[SerializeField] OldLadyAudio _oldLadyAudio;
	int _currentLight = 4;

	public void LightSwitch(int whichLight){
		_oldLadyAudio.PlayLightSwitchSound ();
		_oldLadyPath.SwapLights (whichLight);
		if (_currentLight != whichLight) {
			_lights [_currentLight].SetActive (false);
			_lights [whichLight].SetActive (true);
			_currentLight = whichLight;
		}
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.A)) {
			LightSwitch (0);
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			LightSwitch (1);
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			LightSwitch (2);
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			LightSwitch (3);
		}
	}
}
