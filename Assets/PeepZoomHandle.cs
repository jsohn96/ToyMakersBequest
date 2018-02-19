using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepZoomHandle : MonoBehaviour {
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

	void Start(){
		_expandedRadius = Screen.height / 2f;
		_zeroVector2 = new Vector2 (-_expandedRadius, -_expandedRadius);
		_expandedRadius = -_zeroVector2.x;

		_left = Screen.width * -1.5f;
		_bottom = Screen.height;
		_originPosMax.x = _left + _expandedRadius - _expandedRadius*2f;
		_originPosMax.y = _bottom + _expandedRadius + _expandedRadius*2f;
		_originPosMin.x = _left - _expandedRadius - _expandedRadius*2f;
		_originPosMin.y = _bottom -_expandedRadius + _expandedRadius*2f;

		_originRTMax = _zeroVector2;
		_originRTMin = -_zeroVector2;

		_rectTransform.offsetMax = _originPosMax;
		_rectTransform.offsetMin = _originPosMin;
		KeepRenderTextureStaticByOffset ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (_coroutine != null) {
				StopCoroutine (_coroutine);
			}
			_coroutine = FlipLensIn (true);
			StartCoroutine (_coroutine);
		} 
		if (Input.GetKeyUp (KeyCode.Space)) {
			if (_coroutine != null) {
				StopCoroutine (_coroutine);
			}
			_coroutine = FlipLensIn (false);
			StartCoroutine (_coroutine);
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
	}
}
