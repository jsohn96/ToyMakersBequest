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
		"Come witness Agnes and Dora", //2
		"the greatest escapologists of our time in", //3
		"The Secrets Under Water", //4
		"The show always began with Dora entering the water", //5
		"As the doors close, I would begin holding my breathe", //6
		"wishing for her safe escape", //7
		"I would count", //8
		"1", //9
		"2", //10
		"3", //11
		"and to my relief", //12
		"she always made it out unscathed", //13
		"The crowd would always cheer", //14
		"and on one especially grand performance", //15
		"we shared a kiss", //16
		"what a mistake that was", //17
		"The crowd quickly turned on us with gasps and disdain", //18
		"They're Queer!", //19
		"That's just wrong", //20
		"They should be ashamed", //21
		"Dora and I were noticeably shaken", //22
		"but we brushed it off because we had each other", //23
		"and the next day, we performed again", //24
		"just like every other day", //25
		"The performance began with Dora entering the water", //26
		"just like every other day", //27
		"the doors would close as I held my breathe", //28
		"I would count", //29
		"1", //30
		"2", //31
		"3", //32
		"But this was not like every other day", //33
		"Today, things were different" //34
	};
	int cnt = 0;

	IEnumerator _disappearCoroutine;


	void Start(){
		_textMeshPro = GetComponent<TextMeshProUGUI> ();
		_textMeshPro.text = _strings [0];

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
		Debug.Log (cnt);
		_textMeshPro.text = _strings [cnt];
		_markBackgroundTMP.text = "<mark=#000000A0><color=#000000A0>"+_strings [cnt]+"</color></mark>";
		switch (cnt) {
		case 1:
			StartCoroutine (CallNextAfterDuration (3f));
			break;
		case 2:
			StartCoroutine (CallNextAfterDuration (3f));
			break;
		case 5:
			StartCoroutine (CallNextAfterDuration (4f));
			break;
		case 6:
			StartCoroutine (CallNextAfterDuration (3f));
			break;
		case 7:
			StartCoroutine (CallNextAfterDuration (2f));
			break;
		case 8:
			StartCoroutine (CallNextAfterDuration (1f));
			break;
		case 9:
			StartCoroutine (CallNextAfterDuration (1f));
			break;
		case 10:
			_disappearCoroutine = DisappearAfterDuration (1f, cnt);
			_myTheatre.ActivateBothTankDoors ();
			StartCoroutine (_disappearCoroutine);
			break;
		case 12:
			_disappearCoroutine = DisappearAfterDuration (3f, cnt);
			StartCoroutine (_disappearCoroutine);
			StartCoroutine (CallNextAfterDuration (8f));
			break;
		case 13:
			StartCoroutine (CallNextAfterDuration (3f));
			break;
		case 18:
			StartCoroutine (CallNextAfterDuration (2f));
			break;
		case 19:
			StartCoroutine (CallNextAfterDuration (1.2f));
			break;
		case 20:
			StartCoroutine (CallNextAfterDuration (1.2f));
			break;
		case 21:
			StartCoroutine (CallNextAfterDuration (1.2f));
			break;
		case 22:
			StartCoroutine (_disappearCoroutine);
			_myTheatre.ActivateBothTankDoors ();
			break;
		default:
			_disappearCoroutine = DisappearAfterDuration (3f, cnt);
			StartCoroutine (_disappearCoroutine);
			break;
		}
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
