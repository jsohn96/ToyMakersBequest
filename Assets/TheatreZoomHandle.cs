using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreZoomHandle : MonoBehaviour {
	float _left;
	float _bottom;
	Vector2 _zeroVector2;
	Vector2 _originPosMin;
	Vector2 _originPosMax;
	[SerializeField] RectTransform _rectTransform;

	[SerializeField] RectTransform _RTTransform;
	Vector2 _originRTMin;
	Vector2 _originRTMax;
	Vector2 _tempRTMin;
	Vector2 _tempRTMax;

	[SerializeField] RectTransform _outerGearImageTransform;

	[SerializeField] AnimationCurve _gearZoomCurve;

	float _expandedRadius;
	IEnumerator _coroutine;

	[SerializeField] AudioSource _slideAudioSource;
	[SerializeField] AudioClip[] _slideClips = new AudioClip[2];

	//Getting a reference to enable it on Play
	[SerializeField] GameObject _uiMaskObject;

	[SerializeField] int _identity = 0;
	[SerializeField] Camera _closeUpCamera;

	bool _clearOut = false;

	public void Initialize(){
//		_expandedRadius = Screen.height / 2f;
		_expandedRadius = Screen.height;
		_zeroVector2 = new Vector2 (-_expandedRadius, -_expandedRadius);
		_expandedRadius = -_zeroVector2.x;
//		_expandedRadius = -_zeroVector2.x * 2f;

		_left = Screen.width * -1.5f;
		_bottom = Screen.height;
	
		_originPosMax.x = _left + _expandedRadius - _expandedRadius * 2f;
		_originPosMax.y = _bottom + _expandedRadius + _expandedRadius * 2f;
		_originPosMin.x = _left - _expandedRadius - _expandedRadius * 2f;
		_originPosMin.y = _bottom - _expandedRadius + _expandedRadius * 2f;
		

		_originRTMax = _zeroVector2;
		_originRTMin = -_zeroVector2;

		_rectTransform.offsetMax = _originPosMax;
		_rectTransform.offsetMin = _originPosMin;

		KeepRenderTextureStaticByOffset ();

		_uiMaskObject.SetActive (true);
	}

	public void LensIn(){
		_closeUpCamera.enabled = true;
		if (_coroutine != null) {
			StopCoroutine (_coroutine);
		}
		_coroutine = FlipLensIn (true);
		StartCoroutine (_coroutine);
	}

	public void LensOut(){
		if (_coroutine != null) {
			StopCoroutine (_coroutine);
		}
		_coroutine = FlipLensIn (false);
		StartCoroutine (_coroutine);
	}

	public void ClearOut(){
		_clearOut = true;
		if (_identity == 0) {
			LensOut ();
		} else {
			gameObject.SetActive (false);
			_closeUpCamera.enabled = false;
		}
	}

	void KeepRenderTextureStaticByOffset(){
		_tempRTMax = _originRTMax + (_originPosMax - _rectTransform.offsetMax);
		_tempRTMin = _originRTMin + (_originPosMin - _rectTransform.offsetMin);
		_tempRTMax.x -= _left -_expandedRadius*2f;
		_tempRTMin.x -= _left -_expandedRadius*2f;
		_tempRTMin.y -= _bottom + _expandedRadius*2f;
		_tempRTMax.y -= _bottom + _expandedRadius*2f;
		_RTTransform.offsetMax = _tempRTMax;
		_RTTransform.offsetMin = _tempRTMin;
	}


	IEnumerator FlipLensIn(bool flipIn){
		float timer = 0f;
		float duration = 0.5f;

		if (flipIn) {
			_slideAudioSource.clip = _slideClips [0];
		} else {
			_slideAudioSource.clip = _slideClips [1];
		}
		_slideAudioSource.Play ();

		Vector3 tempOffsetMax = _rectTransform.offsetMax;
		Vector3 tempOffsetMin = _rectTransform.offsetMin;
		while (timer < duration) {
			timer += Time.deltaTime;
			if (flipIn) {
				_rectTransform.offsetMax = Vector2.Lerp (tempOffsetMax, -_zeroVector2, _gearZoomCurve.Evaluate (timer / duration));
				_rectTransform.offsetMin = Vector2.Lerp (tempOffsetMin, _zeroVector2, _gearZoomCurve.Evaluate (timer / duration));
			} else {
				_rectTransform.offsetMax = Vector2.Lerp (tempOffsetMax, _originPosMax, (timer / duration));
				_rectTransform.offsetMin = Vector2.Lerp (tempOffsetMin, _originPosMin, (timer / duration));
			}
			_outerGearImageTransform.Rotate (Vector3.forward * Time.deltaTime * 1000f);
			KeepRenderTextureStaticByOffset ();
			yield return null;
		}
		if (_clearOut) {
			gameObject.SetActive (false);
			_closeUpCamera.enabled = false;
		}
		yield return null;
	}
}
