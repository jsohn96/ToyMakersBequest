using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightSystem {
	public Light light;
	public float intensity;
	public Color color;
	public float range;

	public LightSystem (
		Light _light,
		float _intensity,
		Color _color,
		float _range){
		light = _light;
		intensity = _intensity;
		color = _color;
		range = _range;
	}
}

public class MBLightManager : MonoBehaviour {
	[SerializeField] MBLightUtil[] _startLights;
	[SerializeField] LightSourceController[] _layer1Lights;
	MusicBoxCameraStates _currentCameraState;

	void OnEnable(){
		Events.G.AddListener<MBLightManagerEvent> (LightStateHandle);
		Events.G.AddListener<PathStateManagerEvent> (PathStateManagerHandle);
		Events.G.AddListener<MBCameraStateManagerEvent> (MBCameraStateHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<MBLightManagerEvent> (LightStateHandle);
		Events.G.RemoveListener<PathStateManagerEvent> (PathStateManagerHandle);
		Events.G.RemoveListener<MBCameraStateManagerEvent> (MBCameraStateHandle);
	}

	void PathStateManagerHandle(PathStateManagerEvent e){

	}

	void MBCameraStateHandle(MBCameraStateManagerEvent e){
		_currentCameraState = e.activeState;
		float lerpDuration = e.CamDuration;
		switch (_currentCameraState) {
		case MusicBoxCameraStates.intro:
			break;
		case MusicBoxCameraStates.activation:
			StartCoroutine (_layer1Lights [0].LightOn (2.0f));
			break;
		default:
			break;
		}
	}

	// Use this for initialization
	void Start () {
		_layer1Lights [0]._thisLightSystem.light.intensity = 0.0f;
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
