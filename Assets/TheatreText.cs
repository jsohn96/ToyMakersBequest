using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TheatreText : MonoBehaviour {
	[SerializeField] AltTheatre _myTheatre;

	TextMeshProUGUI _textMeshPro;
	[SerializeField] TextMeshProUGUI _markBackgroundTMP;

	string[] _strings = new string[]{
		"I can still remember the voice from the loudspeaker",
		"Come witness Agnes and Dora",
		"the greatest escapologists of our time in",
		"The Secrets Under Water",
		"The performance always began with Dora entering the water",
		"As the doors close, I would begin holding my breathe", //5
		"wishing for her safe escape",
		"I would count",
		"1",
		"2",
		"3", //10
		"and to my relief",
		"she always made it out unscathed",
		"The crowd would always cheer",
		"and on one especially grand performance",
		"we shared a kiss", //15 
		// what a mistake that was...
		//The crowd quickly turned on us with gasps and disdain
		//They're Queer!
		// That's just wrong
		// They should be ashamed.
		// Dora and I were noticeably shaken
		// but we brushed it off because we had each other
		"and the next day, we performed again",
		//just like every other day
		"The performance began with Dora entering the water",
		// just like every other day
		//"As the doors closed, I began holding my breathe
		"and I would count",
		"1",
		"2",
		"3",
		"But this was not like every other day",
		"Today, things were different"
	};
	int cnt = 0;

	IEnumerator _disappearCoroutine;


	void Start(){
		_textMeshPro = GetComponent<TextMeshProUGUI> ();
		_textMeshPro.text = _strings [0];
	}


	public void TriggerText(){
		cnt++;
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
