using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxEnvelopeInteractive : BookInteractive {

	[SerializeField] Transform _envelopeLidPivot;
	[SerializeField] Transform _weddingLetterTransform;

	Quaternion _originRotation;
	Quaternion _goalRotation;
	Timer _openEnvelopeTimer;
	Vector3 _goalPosition = new Vector3(0.35f, 0.026f, 0.657f);
	Vector3 _originPosition;


	Vector3 _letterGoalPosition = new Vector3(0.3475119f, 0.02687199f, 1.007f);
	Vector3 _letterOriginPosition;

	void Start(){
		_originRotation = _envelopeLidPivot.localRotation;
		_goalRotation = Quaternion.Euler (-180f, 0f, 0f);
		_openEnvelopeTimer = new Timer (0.5f);
		_originPosition = _envelopeLidPivot.localPosition;
		_letterOriginPosition = _letterGoalPosition;
		_letterOriginPosition.z = 0.2351711f;
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.A)) {
			Interact ();
		}
	}

	public override void Interact(){
		base.Interact ();
		StartCoroutine (OpenEnvelope ());
	}

	IEnumerator OpenEnvelope(){
		_openEnvelopeTimer.CooldownTime = 0.5f;
		_openEnvelopeTimer.Reset ();
		while (!_openEnvelopeTimer.IsOffCooldown) {
			_envelopeLidPivot.localRotation = Quaternion.Lerp (_originRotation, _goalRotation, _openEnvelopeTimer.PercentTimePassed);
			_envelopeLidPivot.localPosition = Vector3.Lerp (_originPosition, _goalPosition, _openEnvelopeTimer.PercentTimePassed);
			yield return null;
		}
		_envelopeLidPivot.localRotation = _goalRotation;
		_envelopeLidPivot.localPosition = _goalPosition;

		_openEnvelopeTimer.CooldownTime = 0.8f;
		_openEnvelopeTimer.Reset ();
		while (!_openEnvelopeTimer.IsOffCooldown) {
			_weddingLetterTransform.localPosition = Vector3.Lerp (_letterOriginPosition, _letterGoalPosition, _openEnvelopeTimer.PercentTimePassed);
			yield return null;
		}
		_weddingLetterTransform.localPosition = _letterGoalPosition;

		yield return null;
	}


}
