using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickUpKeyCredit : MonoBehaviour {
	float speed = 0.02f;
	[SerializeField] float _delay = 3f;
	[SerializeField] float _delaySpeedUpDuration = 6f;
	bool _delayBeforeScroll = false;
	bool _delaySpeedUp = false;
	[SerializeField] RectTransform _creditTransform;
	bool _stopForPickUp = false;
	[SerializeField] Button _button;
	[SerializeField] Fading _fadeScript;

	[SerializeField] AudioSource _pickUpSound;

	[SerializeField] bool _isForKey = true;
	bool _readyToEnd = false;

	void Start(){
		StartCoroutine (DelayBeforeScroll ());
		StartCoroutine (DelaySpeedUp ());
	}

	void Update(){
		if (Input.GetMouseButton (0)) {
			if (_delaySpeedUp) {
				speed = 0.08f;
			}
		} else if (Input.GetMouseButtonUp (0)) {
			speed = 0.02f;
		}

		if (_readyToEnd) {
			if (Input.GetMouseButtonDown (0)) {
				StartCoroutine (ChangeLevel ());
			}
		}
	}

	void FixedUpdate () {
		if (_delayBeforeScroll && !_stopForPickUp) {
			_creditTransform.Translate (Vector2.up * speed);

			if (_creditTransform.anchoredPosition.y >= 0f) {
				_stopForPickUp = true;
				if (_isForKey) {
					_button.interactable = true;
				} else {
					_readyToEnd = true;
				}
			}
		}
	}

	IEnumerator DelayBeforeScroll(){
		yield return new WaitForSeconds (_delay);
		_delayBeforeScroll = true;
	}

	IEnumerator DelaySpeedUp(){
		yield return new WaitForSeconds (_delaySpeedUpDuration);
		_delaySpeedUp = true;
	}

	public void KeyPickUp(){
		AltCentralControl._currentState = AltStates.keyUnlock;
		InventorySystem._instance.AddItem (items.finalKey);
		GetComponent<Image> ().enabled = false;
		_pickUpSound.Play ();
		StartCoroutine (ChangeLevel ());
	}


	IEnumerator ChangeLevel(){
		yield return new WaitForSeconds(1.5f);
		float fadeTime = _fadeScript.BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		if (_isForKey) {
			SceneManager.LoadScene ("ControlRoom");
		} else {
			SceneManager.LoadScene (0);
		}
	}
}
