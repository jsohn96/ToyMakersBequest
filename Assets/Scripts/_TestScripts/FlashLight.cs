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

		[SerializeField] Transform _transform;
		[SerializeField] Vector3[] _24rotations;
		[SerializeField] Vector3[] _36rotations;
		[SerializeField] Vector3[] _48rotations;

		BoxCollider _boxCollider;

		[SerializeField] GameObject _gameTitle;

		[SerializeField] ZoetropeCrankHandler _ZcrankHandler;

		bool _began48fps = false;
		bool _began36fps = false;

		void Awake(){
			_offTimer = new Timer (1.0f / 30.0f);
			_onTimer = new Timer (1.0f / 30.0f);
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
			if (!_bringInAnimation) {
				if (other.tag == "RotateCircle") {
					TurnOnlight ();
				}
			}
		}


		void OnTriggerExit(Collider other){
			if (!_bringInAnimation) {
				if (other.tag == "RotateCircle") {
					TurnOfflight ();
				}
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
			yield return new WaitForSeconds (2f);

			_bringInAnimation = true;
//			_ZcrankHandler.enabled = false;
			_ZcrankHandler.HijackSpeed(5f);
			for (int i = 0; i < _removeForZoetrope.Length; i++) {
				_removeForZoetrope[i].SetActive(false);
			}

			_isFlashing = true;

			yield return new WaitForSeconds (2f);
			_onTimer.CooldownTime = (1.0f / 36.0f);
			_offTimer.CooldownTime = (1.0f / 36.0f);
			_began36fps = true;
			yield return new WaitForSeconds (1f);
			_ZcrankHandler.enabled = false;
			_began48fps = true;
			_onTimer.CooldownTime = (1.0f / 48.0f);
			_offTimer.CooldownTime = (1.0f / 48.0f);
			yield return new WaitForSeconds (1f);
			_gameTitle.SetActive (true);
			yield return new WaitForSeconds (4f);
			_gameTitle.SetActive (false);
			yield return new WaitForSeconds (1f);
			yield return StartCoroutine(StateManager._stateManager.ChangeLevel (2));
		}

		void TurnOnlight(){
			myLight.intensity = maxIntensity;
			_isLightOn = true;
			if (_bringInAnimation) {
				_onTimer.Reset ();
			}

		}

		//TODO: figure out why flicker isnt working properly
		 

		void TurnOfflight(){
			myLight.intensity = 0;

			_isLightOn = false;
			if (_bringInAnimation) {
				_offTimer.Reset ();

				if (!_began48fps) {
					if (!_began36fps) {
						if (_cnt - 1 == -1) {
							_transform.localRotation = Quaternion.Euler (_24rotations [4]);
						} else {
							_transform.localRotation = Quaternion.Euler (_24rotations [_cnt]);
						}
						_transform.localRotation = Quaternion.Euler (_24rotations [_cnt]);
					} else {
						if (_cnt - 1 == -1) {
							_transform.localRotation = Quaternion.Euler (_36rotations [4]);
						} else {
							_transform.localRotation = Quaternion.Euler (_36rotations [_cnt]);
						}
						_transform.localRotation = Quaternion.Euler (_36rotations [_cnt]);
					}
				} else {
					if (_cnt - 1 == -1) {
						_transform.localRotation = Quaternion.Euler (_48rotations [4]);
					} else {
						_transform.localRotation = Quaternion.Euler (_48rotations [_cnt]);
					}
					_transform.localRotation = Quaternion.Euler (_48rotations [_cnt]);
				}
				_cnt++;
				if (_cnt == 5) {
					_cnt = 0;
				}
			}
		}

	}
	
}


