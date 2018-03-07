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

	string[] _strings = new string[]{
		"I can still remember the voice from the loudspeaker", //0
		"Echoing throughout the stage", //1
		"Ladies and gentlemen, welcome to the show",//2
		"You are about to witness Agnes and Dora", //3
		"the greatest escapologists of our time in", //4
		"The Secrets Under Water", //5
		"The show always began with Dora entering the water", //6
		"As the doors close, I would begin holding my breathe", //7
		"wishing for her safe escape", //8
		"I would count", //9
		"1", //10
		"2", //11
		"3", //12
		"and to my relief", //13
		"she always made it out unscathed", //14
		"The crowd would always cheer", //15
		"and on one especially grand performance", //16
		"we shared a kiss", //17
		"what a mistake that was", //18
		"The crowd quickly turned on us with gasps and disdain", //19
		"They're Queer!", //20
		"That's just wrong", //21
		"They should be ashamed", //22
		"Dora and I were noticeably shaken", //23
		"but we brushed it off because we had each other", //24
		"and the next day, we performed again", //25
		"", //26
		"The performance began with Dora entering the water", //27
		"just like every other day", //28
		"the doors would close as I held my breathe", //29
		"I would count", //30
		"1", //31
		"2", //32
		"3", //33
		"But this was not like every other day", //34
		"Today, things were different" //35
	};
	int cnt = 0;

	IEnumerator _disappearCoroutine;


	void Start(){
		_textMeshPro = GetComponent<TextMeshProUGUI> ();
//		_textMeshPro.text = _strings [0];

		int voClipsLength = _voClips.Length;
		_voClipLengths = new float[voClipsLength];
		for (int i = 0; i < voClipsLength; i++) {
			_voClipLengths[i] = _voClips[i].length + 0.5f;
		}
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
		_textMeshPro.text = _strings [cnt];
		_markBackgroundTMP.text = "<mark=#000000A0><color=#000000A0>"+_strings [cnt]+"</color></mark>";
		switch (cnt) {
		case 0:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 1:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 2:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			_myTheatre.SetLight (2);
			_myTheatre.BringInMusic ();
			_myTheatre.CheckStateMachine ();
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
			break;
		case 7:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 8:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
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
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 14:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt] + 2.5f));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
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
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]+ 2.0f));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 18:
			_myTheatre.SetLight (6);
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]+1f));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 19:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt] + 1.0f));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 20:
			_myTheatre.MoveToNext ();
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 21:
			_myTheatre.MoveToNext ();
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 22:
			_myTheatre.MoveToNext ();
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 23:
			_myTheatre.SetLight (0);
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 24:
			_myTheatre.MoveToNext ();
			StartCoroutine (DisappearAfterDuration (_voClipLengths [cnt], cnt));
			_audioSource.clip = _voClips [cnt];
			_audioSource.Play ();
			break;
		case 25:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
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
			_myTheatre.MoveToNext ();
			break;
		case 29:
			StartCoroutine (CallNextAfterDuration (_voClipLengths [cnt]));
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
