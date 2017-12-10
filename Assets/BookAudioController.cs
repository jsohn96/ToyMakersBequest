using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAudioController : AudioSourceController {
	[SerializeField] AudioSystem _pageFlipAudioSystem;
	bool _liftedThePage = false;
	[SerializeField] AudioSystem _interactAudioSystem;
	[SerializeField] AudioSystem[] _musicBoxTitleAudioSystems = new AudioSystem[2];

	public void PlayPageLift(){
		_pageFlipAudioSystem.audioSource.clip = _pageFlipAudioSystem.clips [0];
		_pageFlipAudioSystem.audioSource.Play ();
		_liftedThePage = true;
	}
	public void PlayPageDrop(){
		if (_liftedThePage) {
			_pageFlipAudioSystem.audioSource.clip = _pageFlipAudioSystem.clips [1];
			_pageFlipAudioSystem.audioSource.Play ();
			_liftedThePage = false;
		}
	}

	void InteractedWithNotebookObject(NotebookInteractionEvent e){
		_interactAudioSystem.audioSource.Play ();
	}

	public void TitleBoxMoving(int value){
		// 0: no song, 1: 1 song, 2: both song
		if (value == 0) {
			Stop (_musicBoxTitleAudioSystems [0]);
			Stop (_musicBoxTitleAudioSystems [1]);
		} else if (value == 1) {
			if (!_musicBoxTitleAudioSystems [0].audioSource.isPlaying) {
				_musicBoxTitleAudioSystems [0].audioSource.volume = 0.0f;
				_musicBoxTitleAudioSystems [1].audioSource.volume = 0.0f;
				Play(_musicBoxTitleAudioSystems [0]);
				_musicBoxTitleAudioSystems [1].audioSource.Play ();
			} else {
				AdjustVolume (_musicBoxTitleAudioSystems [1], _musicBoxTitleAudioSystems[1].fadeDuration, 0.0f, _musicBoxTitleAudioSystems[1].audioSource.volume, true);
			}
		} else if (value == 2) {
			AdjustVolume (_musicBoxTitleAudioSystems [1], _musicBoxTitleAudioSystems[1].fadeDuration, _musicBoxTitleAudioSystems[1].volume, _musicBoxTitleAudioSystems[1].audioSource.volume);
		}
	}

	void OnEnable(){
		Events.G.AddListener<NotebookInteractionEvent> (InteractedWithNotebookObject);

	}
	void OnDisable(){
		Events.G.RemoveListener<NotebookInteractionEvent> (InteractedWithNotebookObject);

	}
}
