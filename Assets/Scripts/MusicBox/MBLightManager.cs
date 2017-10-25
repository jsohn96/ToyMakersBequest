using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBLightManager : MonoBehaviour {
	[SerializeField] MBLightUtil[] _startLights;

	void OnEnable(){
		Events.G.AddListener<MBLightManagerEvent> (LightStateHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<MBLightManagerEvent> (LightStateHandle);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TurnLightsOff(MBLightUtil[] lights){
		foreach (MBLightUtil mlu in lights) {
			mlu.TurnLightOff ();
		}
	}

	void TurnLightsOn(MBLightUtil[] lights){
		foreach (MBLightUtil mlu in lights) {
			mlu.TurnLightOn ();
		}
	}

	void LightStateHandle(MBLightManagerEvent e){
		if (e.activeLightState == LightState.turn_main_lights_off) {
			TurnLightsOff (_startLights);
		} else if (e.activeLightState == LightState.turn_main_lights_on) {
			TurnLightsOn (_startLights);
		}
	}
}
