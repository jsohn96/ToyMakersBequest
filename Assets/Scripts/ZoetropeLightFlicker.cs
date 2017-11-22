using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test;

public class ZoetropeLightFlicker : MonoBehaviour {

	MinMax _flickerRange = new MinMax (1.1f, 1.5f);
	AudioSource _audioSource;
	bool _littleFlicker = false;
	bool _darkerFlicker = false;
	bool _shuttingDown = false;

	Light _dLight;
	Timer _shutDownLightTimer;

	float _dlightTempIntensity;
	[SerializeField] FlashLight _flashLightScript;
	[SerializeField] Light _dogDirectionalLight;

	bool _dogDLightGo = false;
	Timer _dogDLightTimer;

	// Use this for initialization
	void Awake () {
		_dLight = GetComponent<Light> ();
		_shutDownLightTimer = new Timer (2.5f);
		_audioSource = GetComponent<AudioSource> ();
		_dogDLightTimer = new Timer (1.0f);
	}

	public void LittleFlicker(){
		_littleFlicker = true;
	}

	public void DarkerFlicker(){
		_darkerFlicker = true;
	}

	public void PlayTick(){
		_audioSource.Play ();
	}

	public void ShutDown(){
		if (!_shuttingDown) {
			_shuttingDown = true;
			_dlightTempIntensity = _dLight.intensity;
			_shutDownLightTimer.Reset ();
			if (!_dogDLightGo) {
				_dogDLightGo = true;
				StartCoroutine (FadeOutDogDLight ());
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_littleFlicker) {
			if (_darkerFlicker) {
				_dLight.intensity = MathHelpers.LinMapFrom01 (0.8f, _flickerRange.Min, Mathf.PingPong (Time.time, 1.8f));
			} else {
				_dLight.intensity = MathHelpers.LinMapFrom01 (_flickerRange.Min, _flickerRange.Max, Mathf.PingPong (Time.time, 1.3f));
			}
		}

		if (_shuttingDown) {
			_dLight.intensity = Mathf.Lerp (_dlightTempIntensity, 0.0f, _shutDownLightTimer.PercentTimePassed);
			if (_shutDownLightTimer.IsOffCooldown) {
				_flashLightScript.StartZoetrope ();
			}
		}
	}

	IEnumerator FadeOutDogDLight(){
		yield return new WaitForSeconds (1.0f);
		_dogDLightTimer.Reset ();
		float originIntensity = _dogDirectionalLight.intensity;
		while (!_dogDLightTimer.IsOffCooldown) {
			_dogDirectionalLight.intensity = Mathf.Lerp (originIntensity, 0.0f, _dogDLightTimer.PercentTimePassed);
			yield return null;
		}
		_dogDirectionalLight.intensity = 0.0f;
		yield return null;
	}
}
