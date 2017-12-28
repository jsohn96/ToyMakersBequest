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
	[SerializeField] ToggleAction[] _toggleAction;
	float _toggleActionLength = 0f;

	[Header("Sequencing Variables")]
	[SerializeField] bool _sequenced = false;
	public bool _isActivated = false;
	[SerializeField] TextMeshPro _textToStay;
	[SerializeField] InteractGearButton _nextInteractGear;
	SpriteRenderer _gearSprite;
	[SerializeField] float delayBeforeActivationDuration = 1.0f;

	void Awake(){
		_gearSprite = GetComponent<SpriteRenderer> ();
		_toggleActionLength = _toggleAction.Length;
	}

	void Start(){
		_originColor = _linkedTextMeshPro.color;
		_emptyColor = _originColor;
		_emptyColor.a = 0.0f;
		_linkedTextMeshPro.color = _emptyColor;
		_textFadeTimer = new Timer (0.3f);

	}

	void OnEnable(){
		if (!_sequenced || _isActivated) {
			_isOn = true;
		} else {
			_textToStay.color = _emptyColor;
			_gearSprite.enabled = false;
		}
	}

	void OnDisable() {
		_isOn = false;
		_linkedTextMeshPro.color = _emptyColor;
		if (_sequenced) {
			if (_nextInteractGear != null) {
				_nextInteractGear._isActivated = false;
			}
		}
	}
	
	void FixedUpdate () {
		if (_isOn) {
			transform.Rotate (Vector3.forward, -0.8f);
		}
	}

	public void ActivateSequenceFunction(){
		_isActivated = true;
		_gearSprite.enabled = true;
		_isOn = true;
		StartCoroutine (FadeInText(_textToStay));
		StartCoroutine (FadeInSprite (_gearSprite));
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
			_fadeInCoroutine = FadeInText (_linkedTextMeshPro);
			StartCoroutine (_fadeInCoroutine);
			if (_toggleActionLength != 0) {
				for (int i = 0; i < _toggleActionLength; i++) {
					_toggleAction[i].ToggleActionOn ();	
				}
			}
			//Sequencing Code below
			if (_nextInteractGear != null) {
				StartCoroutine (DelaySequenceActivation ());
			}
		}
	}

	IEnumerator DelaySequenceActivation(){
		yield return new WaitForSeconds (delayBeforeActivationDuration);
		_nextInteractGear.ActivateSequenceFunction();
	}

	IEnumerator FadeInText(TextMeshPro textMeshPro){
		if (_fadeOutCoroutine != null) {
			StopCoroutine (_fadeOutCoroutine);
		}
		_tempColor = textMeshPro.color;
		_textFadeTimer.Reset ();
		while (!_textFadeTimer.IsOffCooldown) {
			textMeshPro.color = Color.Lerp (_tempColor, _originColor, _textFadeTimer.PercentTimePassed);
			yield return null;
		}
		textMeshPro.color = _originColor;
		yield return null;
	}

	IEnumerator FadeInSprite(SpriteRenderer sprite){
		_textFadeTimer.Reset ();
		float timer = 0f;
		float duration = 0.3f;
		Color white = Color.white;
		while (timer < duration) {
			timer += Time.deltaTime;
			sprite.color = Color.Lerp (_emptyColor, white, timer/duration);
			yield return null;
		}
		sprite.color = white;
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
