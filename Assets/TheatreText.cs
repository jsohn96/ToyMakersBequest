using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TheatreText : MonoBehaviour {
	[SerializeField] AltTheatre _myTheatre;

	TextMeshProUGUI _textMeshPro;
	string[] _strings = new string[]{
		"I can still remember the voice from the loudspeaker",
		"Come witness the daring escape",
		"Performed by Agnes and Dora",
		"in, The Secrets Under Water",
		"The performance would always begin with Dora entering the water",
		"I would always hold my breathe as the doors closed", //5
		"wondering if she would make it out",
		"I would always count",
		"1",
		"2",
		"3", //10
		"and to my relief",
		"she always did",
		"The crowd would always cheer",
		"and for a moment, everything would feel perfect",
		"until the kiss",
		"The next day, everything went back to normal",
		"The performance began with Dora entering the water",
		"and I would count",
		"1",
		"2",
		"3",
		"But something was not right"
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
		_textMeshPro.text = _strings [cnt];
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
		case 16:
			break;
		case 17:
			break;
		case 18:
			break;
		case 19:
			break;
		case 20:
			break;
		case 21:
			break;
		case 22:
			break;
		case 23:
			break;
		case 24:
			break;
		case 25:
			break;
		case 26:
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
		}
	}
}
