using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using TMPro;

public class MusicNotePlay : ToggleAction {
	[SerializeField] GameObject[] _musicNotes;
	[SerializeField] SimpleMusicPlayer _simpleMusicPlayer;
	int _musicNoteCounter = 0;
	int _musicNoteLength;
	Color _emptyColor;
	Color _fullColor;
	Color _tempColor;

	[SerializeField] ToggleAction[] _toggleAction;
	bool _once = false;
	float _toggleActionLength = 0;

	void Start () {
		_toggleActionLength = _toggleAction.Length;
		_musicNoteLength = _musicNotes.Length;
		Koreographer.Instance.RegisterForEvents ("MusicBoxNotebook", ToggleNoteOn);
		_fullColor = Color.black;
		_emptyColor = _fullColor;
		_emptyColor.a = 0.0f;
	}

	void ToggleNoteOn(KoreographyEvent koreoEvent){
		if (_musicNoteLength > _musicNoteCounter) {
			_musicNotes [_musicNoteCounter].SetActive (true);
			_musicNoteCounter++;
			if (!_once && _musicNoteCounter >= _musicNoteLength) {
				_once = true;
				if (_toggleActionLength != 0) {
					for (int i = 0; i < _toggleActionLength; i++) {
						_toggleAction[i].ToggleActionOn ();	
					}
				}
			}
		}
	}


	public override void ToggleActionOn(){
		base.ToggleActionOn ();
		StartCoroutine (MusicStartDelay ());
	}

	IEnumerator MusicStartDelay(){
		yield return new WaitForSeconds (1.0f);
		_simpleMusicPlayer.Play ();
	}

	void OnDisable() {
		_simpleMusicPlayer.Stop ();
		_musicNoteCounter = 0;
		for (int i = 0; i < _musicNoteLength; i++) {
			_musicNotes [i].SetActive (false);
		}
	}


	IEnumerator FadeInText(TextMeshPro textMeshPro){
		_tempColor = textMeshPro.color;
		float timer = 0f;
		float duration = 0.3f;
		while (timer < duration) {
			timer += Time.deltaTime;
			textMeshPro.color = Color.Lerp (_tempColor, _fullColor, timer/duration);
			yield return null;
		}
		textMeshPro.color = _fullColor;
		yield return null;
	}
}
