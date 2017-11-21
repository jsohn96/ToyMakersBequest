using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInteractive : BookInteractive {
	[SerializeField] AudioSystem _introVO;
	[SerializeField] TextMeshPro[] _textMeshPros;
	[SerializeField] TMP_FontAsset _glowAsset;
	[SerializeField] TMP_FontAsset _nonGlowAsset;
	[SerializeField] Material _nonGlowTextMaterial;
	[SerializeField] Material _glowTextMaterial;
	Timer _fadeTimer;
	Timer _glowTimer;
	int _onWhichVO = 0;

	bool _beginVOSequence = false;
	float _dilateGoal = -0.246f;
	float _originDialte = 0.0f;

	IEnumerator _tempCoroutine;

	void Start () {
		_fadeTimer = new Timer (1f);
		_glowTimer = new Timer (0.5f);

		for (int i = 0; i < _introVO.clips.Length-1; i++) {
			_textMeshPros [i].font = _nonGlowAsset;
		}
		_nonGlowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, _originDialte);
		_glowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, _originDialte);
	}
	
	public override void Interact(){
		base.Interact ();
		Reset ();
		_tempCoroutine = DelayBeforeVoBegin ();
		StartCoroutine (_tempCoroutine);
	}

	void OnEnable(){
		for (int i = 0; i < _introVO.clips.Length-1; i++) {
			_textMeshPros [i].font = _nonGlowAsset;
		}
		_nonGlowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, _originDialte);
		_glowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, _originDialte);
	}

	void OnDisable(){
		Reset();
	}

	void Reset(){
		_onWhichVO = 0;
		if (_tempCoroutine != null) {
			StopCoroutine (_tempCoroutine);
		}
		_beginVOSequence = false;
	}

	IEnumerator DelayBeforeVoBegin(){
		_fadeTimer.Reset ();
		_textMeshPros [_onWhichVO].font = _glowAsset;
		float tempDilate;
		while (!_fadeTimer.IsOffCooldown) {
			tempDilate = Mathf.Lerp (_originDialte, _dilateGoal, _fadeTimer.PercentTimePassed);
			_nonGlowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, tempDilate);
			yield return null;
		}
		_nonGlowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, _dilateGoal);

		_introVO.audioSource.clip = _introVO.clips [_onWhichVO];
		//yield return new WaitForSeconds (0.5f);
		_introVO.audioSource.Play ();
		_beginVOSequence = true;

		yield return null;
	}

	IEnumerator EndVO(){
		Debug.Log ("call me");
		_onWhichVO = 0;
		_textMeshPros [_onWhichVO].font = _nonGlowAsset;
		_glowTimer.Reset ();
		float tempDilate;
		while (!_glowTimer.IsOffCooldown) {
			tempDilate = Mathf.Lerp (_dilateGoal, _originDialte, _glowTimer.PercentTimePassed);
			_nonGlowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, tempDilate);
			yield return null;
		}
		_nonGlowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, _originDialte);
		yield return null;
	}

	IEnumerator PlayNext(){
		_onWhichVO++;
		_glowTimer.Reset ();
		float tempDilate;
		while (!_glowTimer.IsOffCooldown) {
			tempDilate = Mathf.Lerp (_originDialte, _dilateGoal, _glowTimer.PercentTimePassed);
			_glowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, tempDilate);
			yield return null;
		}
		_glowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, _originDialte);

		_textMeshPros [_onWhichVO - 1].font = _nonGlowAsset;
		_textMeshPros [_onWhichVO].font = _glowAsset;
		_introVO.audioSource.clip = _introVO.clips [_onWhichVO];
		_introVO.audioSource.Play ();
		_beginVOSequence = true;
		_glowTimer.Reset ();
		while (!_glowTimer.IsOffCooldown) {
			tempDilate = Mathf.Lerp (_dilateGoal, _originDialte, _glowTimer.PercentTimePassed);
			_glowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, tempDilate);
			yield return null;
		}
		_glowTextMaterial.SetFloat (ShaderUtilities.ID_FaceDilate, _originDialte);
		yield return null;
	}

	void FixedUpdate() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Interact ();
		}

		if (_beginVOSequence) {
			if (!_introVO.audioSource.isPlaying) {
				if (_onWhichVO == _introVO.clips.Length-1) {
					_beginVOSequence = false;
					_tempCoroutine = EndVO ();
					StartCoroutine (_tempCoroutine);
				}
				else if (_onWhichVO < _introVO.clips.Length - 1) {
					_beginVOSequence = false;
					_tempCoroutine = PlayNext ();
					StartCoroutine (_tempCoroutine);
				}
			}
		}
	}
}
