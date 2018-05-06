using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public enum MusicVerses{
	Intro,
	Verse1,
	Verse2,
	Verse3,
	Verse4,
	Outro
}

public class TheatreMusic : AudioSourceController {
	MusicVerses _currentVerse = MusicVerses.Intro;
	MusicVerses _targetVerse = MusicVerses.Intro;
	bool _develop = false;

	[SerializeField] AudioSystem _ambientRoomTone;


	[SerializeField] SimpleMusicPlayer[] _simpleMusicPlayer;
	[SerializeField] AudioSource _theatreMusic1;
	[SerializeField] AudioSource _theatreMusic2;
	[SerializeField] AudioSystem _celloSkratch;

	/* 0: Long Cello Skratch
	 * 1: Verse 1 Opening
	 * 2: Verse 1 Loop
	 * 3: Verse 2 Loop
	 * 4: Verse 4 Opening
	 * 5: Verse 4 Loop
	 * 6: Outro Opening
	 * 7: Outro Loop
	 * 8: Outro End
	*/
	[SerializeField] AudioClip[] _theatreMusicClips;
	bool _loopableState = false;
	bool _a1Vacant = true;

	void Start(){
//		Play (_ambientRoomTone);
		Koreographer.Instance.RegisterForEvents ("TheatreIntroTrack", TransitionMusic);
		Koreographer.Instance.RegisterForEvents ("Verse1DevelopmentTrack", TransitionMusic);
		Koreographer.Instance.RegisterForEvents ("Verse2DevelopmentTrack", TransitionMusic);
		Koreographer.Instance.RegisterForEvents ("Verse4DevelopmentTrack", TransitionMusic);
	}

	public void BeginMusic(){
		StartCoroutine (DelayMusicStart ());
	}

	IEnumerator DelayMusicStart(){
		yield return new WaitForSeconds (1f);
//		Play (_theatreMusic);
		_simpleMusicPlayer [0].Play ();
		yield return new WaitForSeconds (3f);
		Stop (_celloSkratch);
	}

	public void PlayCelloSkratch(){
		Stop (_ambientRoomTone);
		Play (_celloSkratch);
	}


	public void EndMusic(){
//		Stop (_theatreMusic);
		Play (_ambientRoomTone);
	}

	public void PrepareDevelop(MusicVerses musicVerses){
		_develop = true;
	}
		
	void Update(){
		if (_loopableState) {
			if (_a1Vacant) {
				if (!_theatreMusic2.isPlaying) {
					LoopableTransitionHandle ();
				}
			} else {
				if (!_theatreMusic1.isPlaying) {
					LoopableTransitionHandle ();
				}
			}
		}
	}

	void TransitionMusic(KoreographyEvent e){
		if (_currentVerse == MusicVerses.Intro) {
			FillAndPlay (1);
			_loopableState = true;
			_simpleMusicPlayer [0].Stop ();
		} else if (_currentVerse == MusicVerses.Verse1) {
			FillAndPlay (3);
			_loopableState = true;
			_simpleMusicPlayer [1].Stop ();
		} else if (_currentVerse == MusicVerses.Verse2) {
			FillAndPlay (4);
			_loopableState = true;
//			_simpleMusicPlayer [2].Stop ();
		} else if (_currentVerse == MusicVerses.Verse4) {
			_currentVerse = MusicVerses.Outro;
			FillAndPlay (6);
			_loopableState = true;
//			_simpleMusicPlayer [3].Stop ();
		}

	}

	void FillAndPlay(int index){
		Debug.Log ("filled and will play");
		if (_a1Vacant) {
			_theatreMusic1.clip = _theatreMusicClips [index];
			_theatreMusic1.Play ();
			_a1Vacant = false;
		} else {
			_theatreMusic2.clip = _theatreMusicClips [index];
			_theatreMusic2.Play ();
			_a1Vacant = true;
		}
	}

	void LoopableTransitionHandle(){
		if (_develop) {
			
			if (_currentVerse == MusicVerses.Intro) {
				Debug.Log ("will develop1");
				_simpleMusicPlayer [1].Play ();
				_currentVerse = MusicVerses.Verse1;
				_loopableState = false;
				_develop = false;
			} else if (_currentVerse == MusicVerses.Verse1) {
				Debug.Log ("will develop2");
				_simpleMusicPlayer [2].Play ();
				_currentVerse = MusicVerses.Verse2;
				_loopableState = false;
				_develop = false;
			} else if (_currentVerse == MusicVerses.Verse2) {
				Debug.Log ("will develop3");
				_simpleMusicPlayer [3].Play ();
				_currentVerse = MusicVerses.Verse4;
				_loopableState = false;
				_develop = false;
			} else if (_currentVerse == MusicVerses.Outro) {
				FillAndPlay (8);
				_loopableState = false;
				_develop = false;
			}
		} else {
			Debug.Log ("will loop");
			if (_currentVerse == MusicVerses.Intro) {
				Debug.Log ("will Loop1");
				FillAndPlay (2);
			}  else if (_currentVerse == MusicVerses.Verse1) {
				Debug.Log ("will Loop2");
				FillAndPlay (3);
			} else if (_currentVerse == MusicVerses.Verse2) {
				Debug.Log ("will Loop3");
				FillAndPlay (5);
			} else if (_currentVerse == MusicVerses.Outro) {
				FillAndPlay (7);
			}
		}
	}
}
