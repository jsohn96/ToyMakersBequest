using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogSwirlTest : MonoBehaviour {
	[SerializeField] Transform _frogTransform;
	[SerializeField] Transform _centerPoint;
	float _counter = 0.0f;
	float _counterDuration = 20.0f;

	[SerializeField] MinMax _frogSwimRange = new MinMax(12.402f, 12.612f);
	[SerializeField] AnimationCurve _swimHeightCurve;

	Vector3 _tempPosition;

	float _maxSpeed = 60.0f;

	bool _activateSwirl = false;
	[SerializeField] Animator _frogAnimator;

	[SerializeField] Transform _frogKey;
	Vector3 _keyRotateAmount = new Vector3 (0f, 10f, 0f);

	public void ActivateSwirl(){
		_frogAnimator.enabled = false;
		_activateSwirl = true;
	}

	void Update () {
		if (_activateSwirl) {
			_counter += Time.deltaTime;
			_frogTransform.RotateAround (_centerPoint.position, _centerPoint.up, _maxSpeed * Time.deltaTime);
			_tempPosition = _frogTransform.position;
			_tempPosition.y = Mathf.Lerp(_frogSwimRange.Max, _frogSwimRange.Min, _swimHeightCurve.Evaluate(_counter/_counterDuration));
			_frogTransform.position = _tempPosition;

			if(_counter/_counterDuration>=1f){
				_counter = 0f;
			}
			_frogKey.RotateAround (_frogKey.position, _frogKey.right, Time.deltaTime * 180f);
		}
	}

	public void ShrinkFrog(){
		StartCoroutine (ShrinkFrogCoroutine ());
	}

	IEnumerator ShrinkFrogCoroutine(){
		float timer = 0f;
		float duration = 1.5f;
		Vector3 zeroVector3 = new Vector3 (0f, 0f, 0f);
		Vector3 frogScale = _frogTransform.localScale;
		while (timer < duration) {
			timer += Time.deltaTime;
			_frogTransform.localScale = Vector3.Lerp (frogScale, zeroVector3, timer / duration);
			yield return null;
		}
		_frogTransform.gameObject.SetActive (false);
		yield return null;
	}
}
