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
		[SerializeField] MeshRenderer[] _meshRendererDog; 

		int _cnt = 0;


		void Start(){
			_offTimer = new Timer (1.0f / 48.0f);
			_onTimer = new Timer (1.0f / 48.0f);
		}

		void Update(){
			if (Input.GetKeyDown (KeyCode.S)) {
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

//		void OnTriggerEnter(Collider other){
//			if (other.tag == "Player") {
//				TurnOnlight ();
//				Debug.Log (other.name);
//			}
//		}
//
//
//		void OnTriggerExit(Collider other){
//			if (other.tag == "Player") {
//				TurnOfflight ();
//			}
//		}


		void TurnOnlight(){
			myLight.intensity = maxIntensity;
			_onTimer.Reset ();
			_isLightOn = true;

		}

		void TurnOfflight(){
			myLight.intensity = 0;
			_offTimer.Reset ();
			_isLightOn = false;
			if (_cnt - 1 == -1) {
				_meshRendererDog [4].enabled = false;
			} else {
				_meshRendererDog [_cnt -1].enabled = false;
			}
			_meshRendererDog [_cnt].enabled = true;
			_cnt++;
			if (_cnt == 5) {
				_cnt = 0;
			}

		}
	}
	
}


