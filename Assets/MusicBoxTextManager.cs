﻿using System.Collections;
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

	int _nodeDancerIsAboutToEnter;

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
		// 5: down the stairs
			if (_nodeDancerIsAboutToEnter == 5) {
				StartCoroutine (Dilate (_textMeshPros [2], _dilateDuration, 1.5f));
			} else if (_nodeDancerIsAboutToEnter == 6) {
				StartCoroutine (FadeOut (_textMeshPros [2], _fadeDuration, 1.5f));
				_textMeshPros [1].gameObject.SetActive(false);
			} else if (_nodeDancerIsAboutToEnter == 7) {
				StartCoroutine (Dilate (_textMeshPros [3], _dilateDuration, 0f));
			StartCoroutine (FadeOut (_textMeshPros [3], _fadeDuration, 2.3f));
				_textMeshPros [2].gameObject.SetActive(false);
			
			}
			 else if (_nodeDancerIsAboutToEnter == 8) {
			StartCoroutine (Dilate (_textMeshPros [4], _dilateDuration, 0.8f));
//			StartCoroutine (FadeOut (_textMeshPros [4], _fadeDuration, 1f));
			StartCoroutine (FadeOut (_textMeshPros [4], _fadeDuration, 5.5f));
			 }
			
		else if (_nodeDancerIsAboutToEnter == 10) {
			StartCoroutine (Dilate (_textMeshPros [5], _dilateDuration, 1.2f));
			StartCoroutine (FadeOut (_textMeshPros [5], _fadeDuration, 6.5f));
//			StartCoroutine (FadeOut (_textMeshPros [4], _fadeDuration, 2f));
		}
//				cnt = 4;
//				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
//				cnt++;
////				_musicBoxCameraManager.ActivateStaticFollow (5f);
//			} else if (_nodeDancerIsAboutToEnter == 9) {
//				_musicBoxCameraManager.ActivateStaticFollow (5f);
//			} else if (_nodeDancerIsAboutToEnter == 10) {
//				cnt = 5;
//				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
//				cnt++;
//				StartCoroutine (DelayedFollowCam (_cameraControlPoints [cnt-1].duration, 3f));
//			}
//			else if (_nodeDancerIsAboutToEnter == 11) {
////				cnt = ;
////				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
////				cnt++;
//				_musicBoxCameraManager.ActivateStaticFollow (5f);
//			}
//			else if (_nodeDancerIsAboutToEnter == 12) {
//				if (!_doubleEntrance12) {
//					cnt = 6;
//					_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
//					cnt++;
//					StartCoroutine (DelayedNextControlPoint (_cameraControlPoints [cnt - 1].duration - 0.8f));
////				_musicBoxCameraManager.ActivateStaticFollow (5f);
//					_doubleEntrance12 = true;
//				} else {
//					cnt = 8;
//					_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
//					cnt++;
//				}
//			}
//			else if (_nodeDancerIsAboutToEnter == 13) {
//				if (!_doubleEntrance13) {
//					_musicBoxCameraManager.ActivateStaticFollow (7f);
//					_doubleEntrance13 = true;
//				} else {
//					_musicBoxCameraManager.ActivateStaticFollow (9f);
//				}
//			}
//			else if (_nodeDancerIsAboutToEnter == 14) {
//				if (!_doubleEntrance14) {
//					_musicBoxCameraManager.ActivateStaticFollow (5f);
//					_doubleEntrance14 = true;
//				} else {
//					_musicBoxCameraManager.ActivateStaticFollow (7f);
//				}
//			}
//			else if (_nodeDancerIsAboutToEnter == 17) {
//				cnt = 9;
//				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
//				cnt++;
//			}
//			else if (_nodeDancerIsAboutToEnter == 18) {
//				cnt = 10;
//				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
//				cnt++;
//			}
//			else if (_nodeDancerIsAboutToEnter == 20) {
//				cnt = 11;
//				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
//				cnt++;
//				StartCoroutine (DelayedNextControlPoint (_cameraControlPoints [cnt - 1].duration - 0.8f));
//			}
//			else if (_nodeDancerIsAboutToEnter == 21) {
//				cnt = 13;
//				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
//				cnt++;
//				StartCoroutine (DelayedNextControlPoint (_cameraControlPoints [cnt - 1].duration - 0.8f));
//			}
//
//			else if (_nodeDancerIsAboutToEnter == 200) {
//				cnt = 15;
//				_musicBoxCameraManager.MoveToWayPoint (_cameraControlPoints [cnt].transform, _cameraControlPoints [cnt].duration, _cameraControlPoints [cnt].fov);
//				cnt++;
////				_musicBoxCameraManager.ActivateStaticFollow ();
//			}
		}
}
