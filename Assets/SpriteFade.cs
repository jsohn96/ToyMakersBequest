using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFade : MonoBehaviour {
	[SerializeField] bool _flicker;
	[SerializeField] bool _initFadeIn;
	[SerializeField] SpriteRenderer _sprite;
	[SerializeField] MinMax _flickerRange;
	[SerializeField] float _fadeDuration = 1.5f;

	bool _isOff = false;

	Color _originColor;
	Color _emptyColor;
	Color _maxFlickerColor;
	Color _minFlickerColor;

	IEnumerator _fadeCoroutine;
	bool _coroutineIsOngoing = false;
	float _flickerDuration = 2.0f;
	float _pingPongTime = 0f;
	bool _flickerIn = false;
	[SerializeField] bool specificUseCaseForZoetrope = false;

	void Start () {
		_originColor = _sprite.color;
		_emptyColor = _originColor;
		_emptyColor.a = 0.0f;
		_sprite.color = _emptyColor;

		if (_flicker) {
			_maxFlickerColor = _originColor;
			_maxFlickerColor.a = _flickerRange.Max;
			_minFlickerColor = _originColor;
			_minFlickerColor.a = _flickerRange.Min;
			_pingPongTime = _flickerDuration;
		}

		if (_initFadeIn) {
			_fadeCoroutine = FadeSpriteIn (1f);
			StartCoroutine (_fadeCoroutine);
		}
	}
	
	void FixedUpdate () {
		if (!_isOff) {
			if (!_coroutineIsOngoing && _flicker) {
				_sprite.color = Color.Lerp (_minFlickerColor, _maxFlickerColor, _pingPongTime / _flickerDuration);
				if (_pingPongTime >= _flickerDuration) {
					_flickerIn = false;
				} else if (_pingPongTime <= 0f) {
					_flickerIn = true;
				}
				if (_flickerIn) {
					_pingPongTime += Time.deltaTime;
				} else {
					_pingPongTime -= Time.deltaTime;
				}
			}
		}
	}

	IEnumerator FadeSpriteIn(float delay){
		_coroutineIsOngoing = true;
		yield return new WaitForSeconds (delay);
		float timer = 0f;
		_isOff = false;
		Color tempColor = _sprite.color;
		while (timer < _fadeDuration) {
			timer += Time.deltaTime;
			_sprite.color = Color.Lerp (tempColor, _originColor, timer / _fadeDuration);
			if (!_coroutineIsOngoing) {
				_coroutineIsOngoing = true;
			}
			yield return null;
		}
		_sprite.color = _originColor;
		_coroutineIsOngoing = false;
		yield return null;
	}

	IEnumerator FadeSpriteOut(){
		if (_fadeCoroutine != null) {
			StopCoroutine (_fadeCoroutine);
		}
		float timer = 0f;
		Color tempColor = _sprite.color;
		_coroutineIsOngoing = true;
		while (timer < _fadeDuration) {
			timer += Time.deltaTime;
			_sprite.color = Color.Lerp (tempColor, _emptyColor, timer / _fadeDuration);
			if (!_coroutineIsOngoing) {
				_coroutineIsOngoing = true;
			}
			yield return null;
		}
		_sprite.color = _emptyColor;
		_coroutineIsOngoing = false;
		_isOff = true;
		yield return null;
	}

	void OnMouseDown(){
		if (specificUseCaseForZoetrope) {
			specificUseCaseForZoetrope = false;
			StartCoroutine(FadeSpriteOut ());
		}
	}
}
