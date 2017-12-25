using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextFadeSequence : MonoBehaviour {
	[SerializeField] TextMeshProUGUI[] _textMeshProSequence;
	int _howManyTMPsInSequence = 0;

	Timer _fadeInTimer;
	float _fadeInDuration = 1.0f;
	Timer _fadeOutTimer;
	float _fadeOutDuration = 0.4f;
	Color _emptyColor;
	Color _originColor;

	int _index = 0;

	bool _readyForNext = false;
	[SerializeField] int _nextScene = 0;
	[SerializeField] Fading _fadeScript;

	AsyncOperation _async;

	void Start () {
		_async = SceneManager.LoadSceneAsync (_nextScene);
		_async.allowSceneActivation = false;

		_howManyTMPsInSequence = _textMeshProSequence.Length;
		_fadeInTimer = new Timer (_fadeInDuration);
		_fadeOutTimer = new Timer (_fadeOutDuration);
		_originColor = Color.white;
		_emptyColor = _originColor;
		_emptyColor.a = 0.0f;

		if (_howManyTMPsInSequence != 0) {
			for (int i = 0; i < _howManyTMPsInSequence; i++) {
				_textMeshProSequence [i].color = _emptyColor;
			}
			StartCoroutine (FadeTMPIn (_index));
		} else {
			_readyForNext = true;
		}
	}
	
	void Update () {
		if (_readyForNext && Input.GetMouseButtonDown (0)) {
			_readyForNext = false;
			_index++;
			StartCoroutine(FadeTMPIn (_index));
		}
	}

	IEnumerator FadeTMPIn(int index) {
		if (index != 0) {
			_fadeOutTimer.Reset ();
			while (!_fadeOutTimer.IsOffCooldown) {
				_textMeshProSequence [index - 1].color = Color.Lerp (_originColor, _emptyColor, _fadeOutTimer.PercentTimePassed);
				yield return null;
			}
			_textMeshProSequence [index - 1].color = _emptyColor;
			yield return null;
		}

		if (index < _howManyTMPsInSequence) {
			_fadeInTimer.Reset ();
			while (!_fadeInTimer.IsOffCooldown) {
				_textMeshProSequence [index].color = Color.Lerp (_emptyColor, _originColor, _fadeInTimer.PercentTimePassed);
				yield return null;
			}
			_textMeshProSequence [index].color = _originColor;
			_readyForNext = true;
			yield return null;
		} else {
			StartCoroutine (ChangeLevel ());
			yield return null;
		}
	}

	IEnumerator ChangeLevel(){
		float fadeTime = _fadeScript.BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		_async.allowSceneActivation = true;
	}
}
