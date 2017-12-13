using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractGearButton : BookInteractive {

	bool _isOn = false;
	[SerializeField] TextMeshPro _linkedTextMeshPro; 
	Color _originColor;
	Color _tempColor;
	Color _emptyColor;
	Timer _textFadeTimer;
	IEnumerator _fadeInCoroutine;
	IEnumerator _fadeOutCoroutine;
	[SerializeField] TextMeshPro _textToHide;
	[SerializeField] ToggleAction _toggleAction;

	void Start(){
		_originColor = _linkedTextMeshPro.color;
		_emptyColor = _originColor;
		_emptyColor.a = 0.0f;
		_linkedTextMeshPro.color = _emptyColor;
		_textFadeTimer = new Timer (0.3f);
	}

	void OnEnable(){
		_isOn = true;
	}

	void OnDisable() {
		_isOn = false;
		_linkedTextMeshPro.color = _emptyColor;
	}
	
	void FixedUpdate () {
		if (_isOn) {
			transform.Rotate (Vector3.forward, -0.8f);
		}
	}

	public override void Interact(){
		base.Interact ();
		Events.G.Raise (new NotebookInteractionEvent ());
		_isOn = !_isOn;
		if (_isOn) {
			_fadeOutCoroutine = FadeOutText ();
			StartCoroutine (_fadeOutCoroutine);
		} else {
			if (_textToHide != null && _textToHide.enabled) {
				_textToHide.enabled = false;
			}
			_fadeInCoroutine = FadeInText ();
			StartCoroutine (_fadeInCoroutine);
			if (_toggleAction != null) {
				_toggleAction.ToggleActionOn ();
			}
		}
	}

	IEnumerator FadeInText(){
		if (_fadeOutCoroutine != null) {
			StopCoroutine (_fadeOutCoroutine);
		}
		_tempColor = _linkedTextMeshPro.color;
		_textFadeTimer.Reset ();
		while (!_textFadeTimer.IsOffCooldown) {
			_linkedTextMeshPro.color = Color.Lerp (_tempColor, _originColor, _textFadeTimer.PercentTimePassed);
			yield return null;
		}
		_linkedTextMeshPro.color = _originColor;
		yield return null;
	}

	IEnumerator FadeOutText(){
		if (_fadeInCoroutine != null) {
			StopCoroutine (_fadeInCoroutine);
		}
		_tempColor = _linkedTextMeshPro.color;
		_textFadeTimer.Reset ();
		while (!_textFadeTimer.IsOffCooldown) {
			_linkedTextMeshPro.color = Color.Lerp (_tempColor, _emptyColor, _textFadeTimer.PercentTimePassed);
			yield return null;
		}
		_linkedTextMeshPro.color = _emptyColor;
		yield return null;
	}
}
