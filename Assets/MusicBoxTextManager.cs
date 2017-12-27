using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicBoxTextManager : MonoBehaviour {
	MusicBoxCameraStates _currentCameraState;
	[SerializeField] TextMeshPro[] _textMeshPros;

	[SerializeField] TMP_FontAsset _staticOnAsset;
	[SerializeField] Material _staticOnTextMaterial;
	[SerializeField] TMP_FontAsset _staticOffAsset;
	[SerializeField] Material _staticOffTextMaterial;
	[SerializeField] TMP_FontAsset[] _dilateAssets = new TMP_FontAsset[2];
	[SerializeField] Material[] _dilateTextMaterials = new Material[2];
	int _lastDilateUsed = 1;
	//dilate goes from -1 to 1

	Color _emptyColor;
	Color _fullColor;
	float _fadeDuration = 0.3f;
	float _dilateDuration = 0.7f;

	void Start () {
		_fullColor = Color.white;
		_emptyColor = _fullColor;
		_emptyColor.a = 0.0f;

		for (int i = 0; i < _textMeshPros.Length; i++) {
			_textMeshPros [i].font = _staticOffAsset;
		}
		_dilateTextMaterials[0].SetFloat (ShaderUtilities.ID_FaceDilate, -1f);
		_dilateTextMaterials[1].SetFloat (ShaderUtilities.ID_FaceDilate, -1f);

		StartCoroutine (Dilate (_textMeshPros[0], _dilateDuration, 0.0f));

	}
	
	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (PathStateManagerHandle);
		Events.G.AddListener<MBCameraStateManagerEvent> (MBCameraStateHandle);
	}

	void OnDisable(){
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
			StartCoroutine (Dilate (_textMeshPros [1], _dilateDuration, lerpDuration));
			break;
		case MusicBoxCameraStates.activation:
			StartCoroutine (FadeOut (_textMeshPros [1], _fadeDuration));
			_textMeshPros [0].gameObject.SetActive(false);
			break;
		default:
			break;
		}
	}

	IEnumerator Dilate(TextMeshPro textMeshPro, float duration, float delay){
		yield return new WaitForSeconds (delay);
		float timer = 0f;
		if (_lastDilateUsed == 1) {
			textMeshPro.font = _dilateAssets [0];
			_lastDilateUsed = 0;
		} else {
			textMeshPro.font = _dilateAssets [1];
			_lastDilateUsed = 1;
		}
		float dilateValue;
		while (timer < duration) {
			timer += Time.deltaTime;
			dilateValue = Mathf.Lerp (-1.0f, 0.0f, timer / duration);
			_dilateTextMaterials[_lastDilateUsed].SetFloat (ShaderUtilities.ID_FaceDilate, dilateValue);
			yield return null;
		}
		textMeshPro.font = _staticOnAsset;
		_dilateTextMaterials[_lastDilateUsed].SetFloat (ShaderUtilities.ID_FaceDilate, -1.0f);
		yield return null;
	}

	IEnumerator FadeOut(TextMeshPro textMeshPro, float duration) {
		float timer = 0f;
		while (timer < duration) {
			timer += Time.deltaTime;
			textMeshPro.color = Color.Lerp (_fullColor, _emptyColor, timer / duration);
			yield return null;
		}

		textMeshPro.color = _emptyColor;
		yield return null;
	}
}
