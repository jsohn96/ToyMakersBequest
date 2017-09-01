using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test{
	public class FlashLight : MonoBehaviour {

		public float totalSeconds;     // The total of seconds the flash wil last
		public float maxIntensity;     // The maximum intensity the flash will reach
		public Light myLight;        // Your light
		Timer _offTimer; 
		Timer _onTimer;
		bool _isFlashing = false;
		bool _isLightOn = false;


		void Start(){
			_offTimer = new Timer (1.0f / 24.0f);
			_onTimer = new Timer (0.02f);
		}

		void Update(){
			if (Input.GetKeyDown (KeyCode.Space)) {
				_isFlashing = !_isFlashing;
			}

			if (_isFlashing) {
				//TurnOnlight ();
				if (_isLightOn && _onTimer.IsOffCooldown) {
					TurnOfflight ();
				} else if (!_isLightOn && _offTimer.IsOffCooldown) {
					TurnOnlight ();
				}
			} else {
				TurnOfflight();
			}


		}


		void TurnOnlight(){
			myLight.intensity = maxIntensity;
			_onTimer.Reset ();
			_isLightOn = true;

		}

		void TurnOfflight(){
			myLight.intensity = 0;
			_offTimer.Reset ();
			_isLightOn = false;
		}
	}
	
}


