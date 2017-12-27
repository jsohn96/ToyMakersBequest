using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSourceController : MonoBehaviour {
	public LightSystem _thisLightSystem;

	public IEnumerator LightOn(float duration){
		float timer = 0.0f;
		float originIntensity = _thisLightSystem.light.intensity;
		while (timer < duration) {
			timer += Time.deltaTime;
			_thisLightSystem.light.intensity = Mathf.Lerp (originIntensity, _thisLightSystem.intensity, timer / duration);
			yield return null;
		}

		_thisLightSystem.light.intensity = _thisLightSystem.intensity;
		yield return null;
	}
}
