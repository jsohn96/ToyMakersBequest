using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightState{
	turn_main_lights_off,
	turn_main_lights_on
}

public class MBLightUtil : MonoBehaviour {
	float startIntensity;
	[SerializeField] float endIntensity = 0f;
	Light myLight;

	bool isTurnOff;
	bool isTurnOn;



	// Use this for initialization
	void Start () {
		myLight = GetComponent<Light> ();
		startIntensity = myLight.intensity;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isTurnOff && !isTurnOn) {
			if (Mathf.Abs (myLight.intensity - endIntensity) >= 0.02f) {
				myLight.intensity = Mathf.Lerp (startIntensity, endIntensity, Time.time*0.3F);
			} else {
				myLight.intensity = endIntensity;
				isTurnOff = false;
			}

		}

		if (!isTurnOff && isTurnOn) {
			if (Mathf.Abs (myLight.intensity - startIntensity) >= 0.02f) {
				myLight.intensity = Mathf.Lerp (endIntensity, startIntensity, Time.time*0.3F);
			} else {
				myLight.intensity = startIntensity;
				isTurnOn = false;
			}
		}
		
	}

	public void TurnLightOff(){
		if (!isTurnOff) {
			isTurnOff = true;
		}
	}


	public void TurnLightOn(){
		if (!isTurnOn) {
			isTurnOn = true;
		}
	}


}
