using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAudioController : AudioSourceController {
	[SerializeField] AudioSystem _pageFlipAudioSystem;
	bool _liftedThePage = false;
	[SerializeField] AudioSystem _interactAudioSystem;

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

	void OnEnable(){
		Events.G.AddListener<NotebookInteractionEvent> (InteractedWithNotebookObject);

	}
	void OnDisable(){
		Events.G.RemoveListener<NotebookInteractionEvent> (InteractedWithNotebookObject);

	}
}
