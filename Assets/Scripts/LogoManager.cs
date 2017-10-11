using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour {
	[SerializeField] Fading _fade;
	Timer _fadeTimer;
	[SerializeField] float _logoTime = 2.0f;
	[SerializeField] AudioSource _audioSource;
	bool _called = false;
	bool _barkCalled = false;

	Timer _barkTimer;
	[SerializeField] float _barkTime = 0.5f;
	// Use this for initialization
	void Start () {
		_fadeTimer = new Timer (_logoTime);
		_barkTimer = new Timer (_barkTime);
		_fadeTimer.Reset ();
		_barkTimer.Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_fadeTimer.IsOffCooldown || Input.anyKeyDown) {
			if (!_called) {
				_called = true;
				if (!_barkCalled) {
					_barkCalled = true;
					if (!_audioSource.isPlaying) {
						_audioSource.Play ();
					}
				}
				StartCoroutine (ChangeLevel ());
			}
		}

		if (_barkTimer.IsOffCooldown && !_barkCalled) {
			_barkCalled = true;
			if (!_audioSource.isPlaying) {
				_audioSource.Play ();
			}
		}


		if (0 < Input.touchCount) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				if (!_called) {
					_called = true;
					StartCoroutine (ChangeLevel ());
				}
			}
		}
	}


	IEnumerator ChangeLevel(){
		yield return new WaitForSeconds(0.5f);
		float fadeTime = _fade.BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene (1);
	}
}
