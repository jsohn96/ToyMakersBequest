using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalEyeCard : MonoBehaviour {
	[SerializeField] int _nextSceneIndex = 3;
	bool _transitionIsHappening = false;
	bool _isTransitioning = false;
	Timer _transitionTimer;
	[SerializeField] DragRotation _dragRotationScript;

	[SerializeField] EndFade _endFadeScript;
	[SerializeField] AnimationCurve _slowStartCurve;
	Color _emptyColor;
	Color _tempColor;
	[SerializeField] AudioSource _transitionAudio;
	[SerializeField] MusicBoxSoundEffect _musicBoxCrankSoundScript;

	void Start () {
		//OnTrackingLost();
		_transitionTimer = new Timer(3.0f);
		_emptyColor = Color.black;
		_emptyColor.a = 0.0f;
		_transitionTimer.Reset ();
		_transitionTimer.Pause ();
	}
		

	void FixedUpdate () {
		if (!_transitionIsHappening) {
			if (_dragRotationScript.isDragStart) {
				if (_isTransitioning && !_transitionTimer.IsPaused) {
					_tempColor = Color.Lerp (_emptyColor, Color.black, _slowStartCurve.Evaluate (_transitionTimer.PercentTimePassed));
					_endFadeScript.ChangeColor (_tempColor);
				} 
			} else {
				_tempColor.a -= 0.02f;
				_endFadeScript.ChangeColor (_tempColor);
			}
		}
	}

	void Update(){
		if (!_transitionIsHappening) {
			if (_dragRotationScript.isDragStart) {
				if (_dragRotationScript.GetIsRotating ()) {
					if (_isTransitioning && _transitionTimer.IsOffCooldown) {
						_transitionIsHappening = true;
						_endFadeScript.ChangeColor (Color.black);
						_musicBoxCrankSoundScript.StopCrankSound ();
						StartCoroutine (WaitForTransition ());
					}
					_transitionTimer.Resume ();
					if (_isTransitioning == false) {
					
						_isTransitioning = true;
					}
				} else {
					if (!_transitionTimer.IsPaused) {
						_transitionTimer.Pause ();
					}
				}
			} else {
				_isTransitioning = false;
				_transitionTimer.Reset ();
				_transitionTimer.Pause ();
			}
		}
	}

	IEnumerator WaitForTransition(){
		
		yield return new WaitForSeconds (1.5f);
		_transitionAudio.Play ();
		yield return new WaitForSeconds (1.0f);
		SceneManager.LoadScene (_nextSceneIndex);
	}
}
