using UnityEngine;
using System.Collections;
using TMPro;

public class CreditScroll : MonoBehaviour {
	float speed = 0.02f;
	[SerializeField] float _delay = 3f;
	[SerializeField] float _delaySpeedUpDuration = 6f;
	bool _delayBeforeScroll = false;
	bool _delaySpeedUp = false;
	[SerializeField] RectTransform _creditTransform;

	[SerializeField] TextMeshProUGUI _tmp;
	float textFadeDuration = 0.7f;
	Color _emptyColor;
	Color _goalColor;

	[SerializeField] AudioSource _audioSource;

	void Start(){
		StartCoroutine (DelayBeforeScroll ());
		StartCoroutine (DelaySpeedUp ());
		if (_tmp != null) {
			_goalColor = _tmp.color;
			_emptyColor = _goalColor;
			_emptyColor.a = 0.0f;
			_tmp.color = _emptyColor;
		}

		if (_audioSource != null) {
			Invoke ("PlayMusic", 2f);
		}
	}

	void PlayMusic(){
		_audioSource.Play ();
	}

	void Update(){
		if (Input.GetMouseButton (0)) {
			if (_delaySpeedUp) {
				speed = 0.08f;
			}
		} else if (Input.GetMouseButtonUp (0)) {
			speed = 0.02f;
		}
	}
	
	void FixedUpdate () {
		if (_delayBeforeScroll) {
			_creditTransform.Translate (Vector2.up * speed);
		}
	}

	IEnumerator DelayBeforeScroll(){
		yield return new WaitForSeconds (_delay);
		_delayBeforeScroll = true;
	}

	IEnumerator DelaySpeedUp(){
		yield return new WaitForSeconds (_delaySpeedUpDuration);
		_delaySpeedUp = true;
		if (_tmp != null) {
			StartCoroutine (FadeInSpeedUpText ());
		}
	}

	IEnumerator FadeInSpeedUpText(){
		float timer = 0f;
		while (timer < _delaySpeedUpDuration) {
			timer += Time.deltaTime;
			_tmp.color = Color.Lerp (_emptyColor, _goalColor, timer / _delaySpeedUpDuration);
			yield return null;
		}
		_tmp.color = _goalColor;
		yield return null;
	}
}
