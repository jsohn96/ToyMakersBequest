using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextContentTracker : MonoBehaviour {
	public bool _isDisplaying = false;

	[SerializeField] TextMeshProUGUI[] _textMeshPro;
	[SerializeField] float[] _height;
	[SerializeField] RectTransform _thisRectTransform;
	Vector3 _defaultRectTransformPos;
	Vector2 _tempSizeDelta;

	[SerializeField] Image _entireBackImage;
	[SerializeField] Image _uiBackground;
	[SerializeField] Color _backGroundColor;
	Color _emptyColor;
	Color _uiMaskTempColor;
	Color _uiBackgroundTempColor;

	IEnumerator _uiCoroutine;

	[SerializeField] Image _uiMask;

	[SerializeField] GameObject _scrollView;

	// Use this for initialization
	void Start () {
		_defaultRectTransformPos = Vector3.zero;
		_emptyColor = Color.black;
		_emptyColor.a = 0.0f;
		if(!_isDisplaying){
			//_entireBackImage.color = _emptyColor;
			_uiBackground.color = _emptyColor;
			_uiMask.color = _emptyColor;
			_scrollView.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			DisplayUI (1);
		}
	}
		
	void ReadjustTextUI(int index){
		_textMeshPro [index].enabled = true;
		_tempSizeDelta = _thisRectTransform.sizeDelta;
		_tempSizeDelta.y = _height [index];
		_thisRectTransform.sizeDelta = _tempSizeDelta;
	}

	public void DisplayUI(int index){
		_scrollView.SetActive (true);
		ReadjustTextUI (index);
		if (_uiCoroutine != null) {
			StopCoroutine (_uiCoroutine);
		}
		_uiCoroutine = FadeUIIn ();
		StartCoroutine (_uiCoroutine);
		_uiMask.color = _backGroundColor;
	}

	public void CloseUI(){
		if (_isDisplaying) {
			_isDisplaying = false;
			_uiMask.color = _emptyColor;
			_thisRectTransform.anchoredPosition3D = _defaultRectTransformPos;
			if (_uiCoroutine != null) {
				StopCoroutine (_uiCoroutine);
			}
			_uiCoroutine = FadeUIOut ();
			StartCoroutine (_uiCoroutine);
		}
	}

	IEnumerator FadeUIIn(){
		_uiBackgroundTempColor = _uiBackground.color;
		float timer = 0f;
		float duration = 1f;
		while (timer < duration) {
			timer += Time.deltaTime;
			_uiBackground.color = Color.Lerp (_uiBackgroundTempColor, _backGroundColor, timer / (duration));
			_entireBackImage.color = _uiBackground.color;
			yield return null;
		}
		_uiBackground.color = _backGroundColor;
		_entireBackImage.color = _backGroundColor;
		_isDisplaying = true;
		yield return null;
	}

	IEnumerator FadeUIOut(){
		_uiBackgroundTempColor = _uiBackground.color;
		float timer = 0f;
		float duration = 0.5f;
		while (timer < duration) {
			timer += Time.deltaTime;
			_uiBackground.color = Color.Lerp (_uiBackgroundTempColor, _emptyColor, timer / (duration));
			yield return null;
		}
		_uiBackground.color = _emptyColor;
		_entireBackImage.color = _emptyColor;
		_scrollView.SetActive (false);
		yield return null;
	}
}
