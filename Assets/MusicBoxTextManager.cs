using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicBoxTextManager : MonoBehaviour {
	MusicBoxCameraStates _currentCameraState;
	[SerializeField] TextMeshPro[] _textMeshPros;
	[SerializeField] TextMeshPro[] _textMeshProsLayer2;

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

	int _nodeDancerIsAboutToEnter;
	bool _doubleEntrance14 = false;

	void Start () {
		_fullColor = Color.white;
		_emptyColor = _fullColor;
		_emptyColor.a = 0.0f;

		for (int i = 0; i < _textMeshPros.Length; i++) {
			_textMeshPros [i].font = _staticOffAsset;
		}
		for (int i = 0; i < _textMeshProsLayer2.Length; i++) {
			_textMeshProsLayer2 [i].font = _staticOffAsset;
		}
		_dilateTextMaterials[0].SetFloat (ShaderUtilities.ID_FaceDilate, -1f);
		_dilateTextMaterials[1].SetFloat (ShaderUtilities.ID_FaceDilate, -1f);

		StartCoroutine (Dilate (_textMeshPros[0], _dilateDuration, 0.0f));
	}
	
	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (PathStateManagerHandle);
		Events.G.AddListener<MBCameraStateManagerEvent> (MBCameraStateHandle);
		Events.G.AddListener<DancerOnBoard> (DancerOnBoardHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (PathStateManagerHandle);
		Events.G.RemoveListener<MBCameraStateManagerEvent> (MBCameraStateHandle);
		Events.G.RemoveListener<DancerOnBoard> (DancerOnBoardHandle);
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
			StartCoroutine (FadeOut (_textMeshPros [1], _fadeDuration, 0f));
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

	IEnumerator FadeOut(TextMeshPro textMeshPro, float duration, float delay) {
		yield return new WaitForSeconds (delay);
		float timer = 0f;
		while (timer < duration) {
			timer += Time.deltaTime;
			textMeshPro.color = Color.Lerp (_fullColor, _emptyColor, timer / duration);
			yield return null;
		}

		textMeshPro.color = _emptyColor;
		yield return null;
	}

	void DancerOnBoardHandle(DancerOnBoard e){
		_nodeDancerIsAboutToEnter = e.NodeIdx;
		Debug.Log(_nodeDancerIsAboutToEnter);
		if (_nodeDancerIsAboutToEnter < 100) {
			// 5: down the stairs
			if (_nodeDancerIsAboutToEnter == 5) {
				StartCoroutine (Dilate (_textMeshPros [2], _dilateDuration, 1.5f));
			} else if (_nodeDancerIsAboutToEnter == 6) {
				StartCoroutine (FadeOut (_textMeshPros [2], _fadeDuration, 1.5f));
				_textMeshPros [1].gameObject.SetActive (false);
			} else if (_nodeDancerIsAboutToEnter == 7) {
				StartCoroutine (Dilate (_textMeshPros [3], _dilateDuration, 0f));
				StartCoroutine (FadeOut (_textMeshPros [3], _fadeDuration, 2.3f));
				_textMeshPros [2].gameObject.SetActive (false);
			
			} else if (_nodeDancerIsAboutToEnter == 8) {
				StartCoroutine (Dilate (_textMeshPros [4], _dilateDuration, 0.8f));
//			StartCoroutine (FadeOut (_textMeshPros [4], _fadeDuration, 1f));
				StartCoroutine (FadeOut (_textMeshPros [4], _fadeDuration, 5.5f));
			} else if (_nodeDancerIsAboutToEnter == 10) {
				StartCoroutine (Dilate (_textMeshPros [5], _dilateDuration, 1.2f));
				StartCoroutine (FadeOut (_textMeshPros [5], _fadeDuration, 6.5f));
//			StartCoroutine (FadeOut (_textMeshPros [4], _fadeDuration, 2f));
			} else if (_nodeDancerIsAboutToEnter == 14) {
				if (!_doubleEntrance14) {
					StartCoroutine (Dilate (_textMeshPros [6], _dilateDuration, 3f));
					StartCoroutine (FadeOut (_textMeshPros [6], _fadeDuration, 6.5f));
//			StartCoroutine (FadeOut (_textMeshPros [4], _fadeDuration, 2f));
					_doubleEntrance14 = true;
				}
			} else if (_nodeDancerIsAboutToEnter == 16) {
				StartCoroutine (Dilate (_textMeshPros [7], _dilateDuration, 0f));
			
//			StartCoroutine (FadeOut (_textMeshPros [4], _fadeDuration, 2f));
			} else if (_nodeDancerIsAboutToEnter == 17) {
				StartCoroutine (FadeOut (_textMeshPros [7], _fadeDuration, 1.5f));
			} else if (_nodeDancerIsAboutToEnter == 18) {
				StartCoroutine (Dilate (_textMeshPros [8], _dilateDuration, 3.4f));
				StartCoroutine (Dilate (_textMeshPros [9], _dilateDuration, 4.5f));
				StartCoroutine (Dilate (_textMeshPros [10], _dilateDuration, 5.6f));

			} else if (_nodeDancerIsAboutToEnter == 19) {
				StartCoroutine (FadeOut (_textMeshPros [8], _fadeDuration, 2f));
				StartCoroutine (FadeOut (_textMeshPros [9], _fadeDuration, 2.8f));
				StartCoroutine (FadeOut (_textMeshPros [10], _fadeDuration, 3.6f));
//			StartCoroutine (Dilate (_textMeshPros [9], _dilateDuration, 0f));
//			StartCoroutine (Dilate (_textMeshPros [10], _dilateDuration, 1f));
			} else if (_nodeDancerIsAboutToEnter == 20) {
				StartCoroutine (Dilate (_textMeshPros [11], _dilateDuration, 5f));
				StartCoroutine (Dilate (_textMeshPros [12], _dilateDuration, 6.2f));
			}
			else if (_nodeDancerIsAboutToEnter == 22) {
				StartCoroutine (Dilate (_textMeshProsLayer2 [0], _dilateDuration, 4.5f));
				StartCoroutine (Dilate (_textMeshProsLayer2 [1], _dilateDuration, 5.4f));
				StartCoroutine (Dilate (_textMeshProsLayer2 [2], _dilateDuration, 6.3f));

				StartCoroutine (FadeOut (_textMeshPros [11], _fadeDuration, 3.5f));
				StartCoroutine (FadeOut (_textMeshPros [12], _fadeDuration, 3.5f));
			}
		} else {
			if (_nodeDancerIsAboutToEnter == 201 || _nodeDancerIsAboutToEnter == 202) {
				StartCoroutine (FadeOut (_textMeshProsLayer2 [0], _fadeDuration, 0.5f));
				StartCoroutine (FadeOut (_textMeshProsLayer2 [1], _fadeDuration, 0.5f));
				StartCoroutine (FadeOut (_textMeshProsLayer2 [2], _fadeDuration, 0.5f));
			}
		}
	}
}
