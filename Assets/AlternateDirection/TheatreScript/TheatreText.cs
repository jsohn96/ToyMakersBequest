using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TheatreText : MonoBehaviour {
	[SerializeField] AltTheatre _myTheatre;

	TextMeshProUGUI _textMeshPro;
	[SerializeField] TextMeshProUGUI _markBackgroundTMP;

	[SerializeField] AudioSource _audioSource;

	[SerializeField] AudioClip[] _voClips;
	float [] _voClipLengths;

	[SerializeField] TheatreCameraControl _theatreCameraControl;

	[SerializeField] TheatreZoomHandle[] _theatreZoomHandles;

	[SerializeField] AudienceHeadFollow[] _audience;
	[SerializeField] TheatreLighting _theatreLighting;
	[SerializeField] TheatreMusic _theatreMusic;
	[SerializeField] TheatreSound _theatreSound;

	string[] _strings = new string[]{
		"I can still remember the voice from the loudspeaker", //0
		"echoing throughout the stage.", //1
		"Ladies and gentlemen, welcome to the show.",//2
		"You are about to witness Agnes and Dora,", //3
		"the greatest escapologists of our time, in", //4
		"\"The Secrets Under Water\".", //5
		"The show always began with Dora entering the water.", //6
		"As the doors close, I would begin holding my breath,", //7
		"wishing for her safe escape.", //8
		"I would count.", //9
		"One.", //10
		"Two.", //11
		"Three.", //12
		"And to my relief,", //13
		"she always made it out unscathed.", //14
		"The crowd would always cheer,", //15
		"and on one especially grand performance,", //16
		"we shared a kiss.", //17
		"What a mistake that was.", //18
		"The crowd quickly turned on us with gasps and disdain.", //19
		"They're queer!", //20
		"That's just wrong!", //21
		"They should be ashamed!", //22
		"Dora and I were noticeably shaken,", //23
		"but we brushed it off because we had each other.", //24
		"And the next day, we performed again.", //25
		"", //26
		"The performance began with Dora entering the water.", //27
		"Just like every other day.", //28
		"The doors would close as I held my breath.", //29
		"I would count.", //30
		"One.", //31
		"Two.", //32
		"Three.", //33
		"But this was not like every other day.", //34
		"Today, things were different.", //35

		"I tried to save her", // 36
		"but she wasn't breathing.", //37

		"She remained motionless in my arms.", //38
		"But then,", //39
		"with a cough", //40
		"she came back", //41
		"Her most daring escape ever." //42
	};
	int cnt = 0;

	IEnumerator _disappearCoroutine;
	int _textLength;

	void Start(){
		_textMeshPro = GetComponent<TextMeshProUGUI> ();
//		_textMeshPro.text = _strings [0];

		_textLength = _voClips.Length;
		_voClipLengths = new float[_textLength];
		for (int i = 0; i < _textLength; i++) {
			_voClipLengths[i] = _voClips[i].length + 0.5f;
		}
	}

	public IEnumerator DelayTriggerText(float duration, int index = -1){
		yield return new WaitForSeconds(duration);
		TriggerText (index);
	}


	public void TriggerText(int index = -1){
		if(index == -1){
			cnt++;
		} else {
			cnt = index;
		}
		if (_disappearCoroutine != null) {
			StopCoroutine (_disappearCoroutine);
		}
		if (cnt >= _textLength) {
			return;
		}
		_textMeshPro.text = _strings [cnt];
		_markBackgroundTMP.text = "<mark=#000000FF><color=#00000000>      "+_strings [cnt]+"</color></mark>";
		switch (cnt) {
		case 0:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			_myTheatre.CheckStateMachine ();
			break;
		case 1:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			_myTheatre.BringInMusic ();

			break;
		case 2:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();

			break;
		case 3:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();

			break;
		case 4:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();

			break;
		case 5:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			StartCoroutine (FinishGreet(_voClipLengths[cnt]- 1f));
			break;
		case 6:
			// dancer enters water
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			_audience [0].PlayClap ();
			_audience [1].PlayClap ();
			break;
		case 7:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 8:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 9:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 10:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 11:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 12:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			_myTheatre.ActivateBothTankDoors ();
			break;
		case 13:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 14:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt] + 2.5f));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			_theatreSound.PlayApplauseSound ();
			break;
		case 15:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt] + 2.5f));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 16:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 17:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 18:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt] + 1f));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			_theatreCameraControl.ChangeFOV (15f, _voClipLengths [cnt] + 4f);
			break;
		case 19:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt] + 1.0f));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			_theatreLighting.Set8 ();
			_theatreSound.PlayLightSwitch ();
			_theatreZoomHandles [0].Initialize ();
			_theatreZoomHandles [1].Initialize ();
			_theatreZoomHandles [2].Initialize ();
			break;
		case 20:
			_myTheatre.MoveToNext ();
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
//			_theatreCameraControl.LookAtBird (1);
			_theatreSound.PlayCrowCawSound();
			_theatreZoomHandles [0].LensIn ();
			_audience [0].PlayKissReaction ();
			break;
		case 21:
			_myTheatre.MoveToNext ();
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
//			_theatreCameraControl.LookAtBird (2);
			_theatreSound.PlayCrowCawSound();
			_theatreZoomHandles [1].LensIn ();
			_audience [1].PlayKissReaction ();
			break;
		case 22:
			_myTheatre.MoveToNext ();
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
//			_theatreCameraControl.LookAtBird (3);
			_theatreSound.PlayCrowCawSound();
			_theatreZoomHandles [2].LensIn ();
			_audience [2].PlayKissReaction ();
			break;
		case 23:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
//			_theatreCameraControl.LookAtBird (0);
			_theatreZoomHandles [0].ClearOut ();
			_theatreZoomHandles [1].ClearOut ();
			_theatreZoomHandles [2].ClearOut ();
			_theatreLighting.SingularSpotLight (true);
			_theatreSound.PlayLightSwitch ();
			break;
		case 24:
			_myTheatre.MoveToNext ();
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 25:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_theatreCameraControl.ChangeFOV (30f, _voClipLengths[cnt]);
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 26:
			_myTheatre.MoveToNext ();
//			TriggerText (27);
			break;
		case 27:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 28:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
//			_myTheatre.MoveToNext ();
			break;
		case 29:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();

			break;
		case 30:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 31:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 32:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			_theatreMusic.PrepareDevelop (MusicVerses.Outro);
			break;
		case 33:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 34:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			StartCoroutine (DelayedTankActivation (_voClipLengths [cnt]));
			break;
		case 35:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		// Part 2 Break Point
		case 36:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt] + 10f));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 37:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt] + 3f));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 38:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 39:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 40:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 41:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 42:
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		default:
			_disappearCoroutine = DisappearAfterDuration (3f, cnt);
			StartCoroutine (_disappearCoroutine);
			break;
		}
	}

	IEnumerator FinishGreet(float duration){
		yield return new WaitForSeconds (duration);
		_myTheatre.MagicianFinishGreet ();
	}

	IEnumerator DelayedTankActivation(float duration){
		yield return new WaitForSeconds (duration);
		_myTheatre.ActivateBothTankDoors ();
	}

	IEnumerator CallNextAfterDuration(float duration){
		yield return new WaitForSeconds (duration);
		TriggerText ();
	}

	IEnumerator DisappearAfterDuration(float duration, int cntWhenCalled){
		yield return new WaitForSeconds (duration);
		if (cntWhenCalled == cnt) {
			_textMeshPro.text = "";
			_markBackgroundTMP.text = "";
		}
	}
}
