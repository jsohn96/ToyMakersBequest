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
		[SerializeField] GameObject[] _removeForZoetrope;
		bool _started = false;

		int _cnt = 0;
		bool _bringInAnimation = false;

		AudioSource _audioSourceWhir;

		void Awake(){
			_offTimer = new Timer (1.0f / 24.0f);
			_onTimer = new Timer (1.0f / 24.0f);
			_audioSourceWhir = GetComponent<AudioSource> ();
		}

		void Update(){
			#if UNITY_EDITOR
			if (Input.GetKeyDown (KeyCode.S)) {
				_started = true;
				_isFlashing = !_isFlashing;
			}
			#endif

			if (_started) {
				if (_isFlashing) {
					//TurnOnlight ();
					if (_isLightOn && _onTimer.IsOffCooldown) {
						TurnOfflight ();
					} else if (!_isLightOn && _offTimer.IsOffCooldown) {
						TurnOnlight ();
					}
				} else {
					TurnOfflight ();
				}
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
		public void StartZoetrope(){
			if (!_started) {
				_started = true;
				StartCoroutine (WaitToSpeedUp ());
			}
		}

		IEnumerator WaitToSpeedUp(){
			_audioSourceWhir.Play ();
			yield return new WaitForSeconds (0.7f);

			yield return new WaitForSeconds (1.0f);
	
			yield return new WaitForSeconds (1.3f);

			_bringInAnimation = true;
			for (int i = 0; i < _removeForZoetrope.Length; i++) {
				_removeForZoetrope[i].SetActive(false);
			}
			_onTimer.CooldownTime = (1.0f / 48.0f);
			_offTimer.CooldownTime = (1.0f / 48.0f);
			_isFlashing = true;
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
			if (_bringInAnimation) {
				if (_cnt - 1 == -1) {
					_meshRendererDog [4].enabled = false;
				} else {
					_meshRendererDog [_cnt - 1].enabled = false;
				}
				_meshRendererDog [_cnt].enabled = true;
				_cnt++;
				if (_cnt == 5) {
					_cnt = 0;
				}
			}
		}
	}
	
}


