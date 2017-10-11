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



		BoxCollider _boxCollider;

		[SerializeField] GameObject _gameTitle;

		void Awake(){
			_offTimer = new Timer (1.0f / 24.0f);
			_onTimer = new Timer (1.0f / 24.0f);
			_boxCollider = GetComponent<BoxCollider> ();
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
				}
			}

		}

		void OnTriggerEnter(Collider other){
			if (other.tag == "RotateCircle") {
				TurnOnlight ();
			}
		}


		void OnTriggerExit(Collider other){
			if (other.tag == "RotateCircle") {
				TurnOfflight ();
			}
		}
		public void StartZoetrope(){
			if (!_started) {
				TurnOfflight ();
				_started = true;
				StartCoroutine (WaitToSpeedUp ());
			}
		}

		IEnumerator WaitToSpeedUp(){
			
			_boxCollider.enabled = true;
			yield return new WaitForSeconds (3f);

			_bringInAnimation = true;
			for (int i = 0; i < _removeForZoetrope.Length; i++) {
				_removeForZoetrope[i].SetActive(false);
			}

			_onTimer.CooldownTime = (1.0f / 48.0f);
			_offTimer.CooldownTime = (1.0f / 48.0f);
			_isFlashing = true;

			yield return new WaitForSeconds (3f);
			_gameTitle.SetActive (true);
			yield return new WaitForSeconds (4f);
			_gameTitle.SetActive (false);
			yield return new WaitForSeconds (1f);
			yield return StartCoroutine(StateManager._stateManager.ChangeLevel (1));
		}

		void TurnOnlight(){
			myLight.intensity = maxIntensity;

			_isLightOn = true;
			if (_bringInAnimation) {
				_onTimer.Reset ();
			}

		}

		void TurnOfflight(){
			myLight.intensity = 0;

			_isLightOn = false;
			if (_bringInAnimation) {
				_offTimer.Reset ();
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


