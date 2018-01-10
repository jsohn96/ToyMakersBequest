using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientLightController : MonoBehaviour {
	[SerializeField] Color _ambientColor;
	Color _originColor;

	void Start(){
//		_originColor = Color.black;
		_originColor = RenderSettings.ambientLight;
	}

	void MBCameraStateHandle(MBCameraStateManagerEvent e){
		if (e.activeState == MusicBoxCameraStates.intro) {
			StartCoroutine (LerpAmbientLight ());
		}
	}

	IEnumerator LerpAmbientLight(){
		_originColor = RenderSettings.ambientLight;
		float timer = 0.0f;
		float duration = 6.0f;
		while (duration > timer) {
			timer += Time.deltaTime;
			RenderSettings.ambientLight = Color.Lerp (_originColor, _ambientColor, timer / duration);
			yield return null;
		}
		RenderSettings.ambientLight = _ambientColor;
		yield return null;
	}


	void OnEnable(){
		Events.G.AddListener<MBCameraStateManagerEvent> (MBCameraStateHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<MBCameraStateManagerEvent> (MBCameraStateHandle);
	}
}
