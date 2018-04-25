using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class TMPcrawl {
	public AudioClip[] voClips;
	public float[] voLengths;
	public TextMeshProUGUI[] textMeshPros;
	public Vector2[] arrowPositions;
	public TMPcrawl (
		AudioClip[] _voClips,
		float[] _voLengths,
		TextMeshProUGUI[] _textMeshPros,
		Vector2[] _arrowPositions
	){
		voClips = _voClips;
		voLengths = _voLengths;
		textMeshPros = _textMeshPros;
		arrowPositions = _arrowPositions;
	}
}

public class TextContentTracker : MonoBehaviour {
	public bool _isDisplaying = false;

	[SerializeField] TMPcrawl[] _tmpCrawls;
	[SerializeField] TextMeshProUGUI[] _textMeshPro;
	[SerializeField] float[] _height;
	[SerializeField] RectTransform _thisRectTransform;
	Vector3 _defaultRectTransformPos;
	Vector2 _tempSizeDelta;

	[SerializeField] Image _entireBackImage;
	[SerializeField] Image _uiBackground;
	[SerializeField] Color _backGroundColor;
	Color _emptyColor;
	Color _whiteColor;
	Color _uiMaskTempColor;
	Color _uiBackgroundTempColor;

	IEnumerator _uiCoroutine;

	[SerializeField] Image _uiMask;

	[SerializeField] GameObject _scrollView;

	[SerializeField] AudioSource _audioSource;

	int _txtCnt = 0;
	int _currentIndex;
	int _currentIndexLength;
	bool _nextArrowOnce = false;

	[SerializeField] Image _nextArrow;
	Timer _audioTimer;

	[SerializeField] Image _backButton;

	[SerializeField] TouchInput _touchInput;

	// Use this for initialization
	void Start () {
		_defaultRectTransformPos = Vector3.zero;
		_emptyColor = Color.black;
		_emptyColor.a = 0.0f;
		_whiteColor = Color.white;
		if(!_isDisplaying){
			//_entireBackImage.color = _emptyColor;
			_uiBackground.color = _emptyColor;
			_uiMask.color = _emptyColor;
			_scrollView.SetActive (false);
		}
		_audioTimer = new Timer (5.0f);
	}
	
	// Update is called once per frame
//	void Update () {
//		if (Input.GetKeyDown (KeyCode.Space)) {
//			DisplayUI (1);
//		}
//		if (Input.GetKeyDown (KeyCode.L)) {
//			ShowNextText ();
//		}

//		if (_nextArrow.enabled) {
//			_nextArrow.color = Color.Lerp (_emptyColor, _whiteColor, Mathf.PingPong(Time.time, 1f));
//		}
//		if (!_nextArrowOnce && _audioTimer.IsOffCooldown && _txtCnt < _currentIndexLength ) {
//			_nextArrow.enabled = true;
//			_nextArrowOnce = true;
//		}
//	}

//	public void ShowNextText(){
//		if (_nextArrowOnce) {
//			StartCoroutine (FadeTMPIn (_tmpCrawls [_currentIndex].textMeshPros [_txtCnt]));
//			_audioTimer.CooldownTime = _tmpCrawls [_currentIndex].voLengths [_txtCnt];
//			_audioTimer.Reset ();
//			_nextArrowOnce = false;
//			if (_audioSource.isPlaying) {
//				_audioSource.Stop ();
//			}
//			_audioSource.clip = _tmpCrawls [_currentIndex].voClips [_txtCnt];
//			_audioSource.Play ();
//
//			_nextArrow.enabled = false;
//			if (_txtCnt < _currentIndexLength - 1) {
//				_nextArrow.rectTransform.anchoredPosition = _tmpCrawls [_currentIndex].arrowPositions [_txtCnt];
//			} else {
//				_backButton.enabled = true;
//			}
//			_txtCnt++;
//		}
//	}
		
	void ReadjustTextUI(int index){
		_textMeshPro [index].enabled = true;
		_tempSizeDelta = _thisRectTransform.sizeDelta;
		_tempSizeDelta.y = _height [index];
		_thisRectTransform.sizeDelta = _tempSizeDelta;
	}

	public void DisplayUI(int index){
		if (_touchInput) {
			_touchInput.enabled = false;
		}

		_backButton.enabled = true;
		_currentIndex = index;
		_currentIndexLength = _tmpCrawls [_currentIndex].textMeshPros.Length;
		_txtCnt = 0;
//		ShowNextText ();
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
			if (_touchInput) {
				_touchInput.enabled = true;
			}
			_isDisplaying = false;
			_uiMask.color = _emptyColor;
			for (int i = 0; i < _txtCnt; i++) {
				_tmpCrawls [_currentIndex].textMeshPros [i].color = _emptyColor;
				_tmpCrawls [_currentIndex].textMeshPros [i].enabled = false;
			}
//			_nextArrow.enabled = false;
			_nextArrowOnce = false;
			_thisRectTransform.anchoredPosition3D = _defaultRectTransformPos;
			if (_uiCoroutine != null) {
				StopCoroutine (_uiCoroutine);
			}
			_uiCoroutine = FadeUIOut ();
			StartCoroutine (_uiCoroutine);
		}
	}

	IEnumerator FadeTMPIn(TextMeshProUGUI tmp){
		tmp.color = _emptyColor;
		tmp.enabled = true;
		float timer = 0f;
		float duration = 1f;
		while (timer < duration) {
			timer += Time.deltaTime;
			tmp.color = Color.Lerp (_emptyColor, _whiteColor, timer / (duration));
			yield return null;
		}
		tmp.color = _whiteColor;
		yield return null;
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
