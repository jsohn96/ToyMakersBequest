using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishWashing : MonoBehaviour {
	float _totalAngle;

	float _originalAngle;
	float _angleIncrement;
	bool _gotIncrement = false;
	float _tempAngleDifference = 0f;

	[SerializeField] int _howManyCycles = 3;
	int _currentCycleCount = 0;

	[SerializeField] SpriteRenderer _dirtySprite;
	float _spriteAlphaValue;
	float _spriteIncrement;
	Color _spriteTempColor;
	Color _goalTempColor;

	IEnumerator _dishFadeCoroutine;
	[SerializeField] BoxCollider _boxCollider;
	[SerializeField] DragRotation _dragRotation;

	[SerializeField] DishWashingSoundEffect _dishWashingSoundEffect;

	void Start(){
		_originalAngle = transform.eulerAngles.z;
		_spriteAlphaValue = _dirtySprite.color.a;
		_spriteIncrement = _spriteAlphaValue / _howManyCycles;
	}

	void DragRotationHandle(DragRotationEvent e){
		if (!_gotIncrement) {
			GetIncrement (e.isDesiredDirection);
		}
		if (e.isDesiredDirection) {
			_totalAngle += _angleIncrement;
		} else {
			_totalAngle -= _angleIncrement;
		}

		if (_totalAngle > 360f) {
			Debug.Log ("Great");
			HandleCycle ();
			_totalAngle -= 360f;
		} else if (_totalAngle < -360f) {
			Debug.Log ("Nope");
			_totalAngle += 360f;
			NextDish (false);
		}
	}

	void HandleCycle(){
		if (_currentCycleCount < _howManyCycles) {
			_currentCycleCount++;
			if (_dishFadeCoroutine != null) {
				StopCoroutine (_dishFadeCoroutine);
			}
			_dishFadeCoroutine = CleanDownDish ();
			StartCoroutine (_dishFadeCoroutine);
		}
	}

	IEnumerator CleanDownDish(){
		_spriteTempColor = _dirtySprite.color;
		_goalTempColor = _spriteTempColor;
		_goalTempColor.a = _spriteAlphaValue - _spriteIncrement * _currentCycleCount;
		float timer = 0f;
		float duration = 0.5f;
		while (timer < duration) {
			timer += Time.deltaTime;
			_dirtySprite.color = Color.Lerp (_spriteTempColor, _goalTempColor, timer / duration);
			yield return null;
		}
		_dirtySprite.color = _goalTempColor;
		yield return null;
		if (_currentCycleCount == _howManyCycles) {
			NextDish (true);
		}
	}

	void NextDish(bool success){
		if (!success) {
			_dishWashingSoundEffect.PlayShatterPlateSound ();
		}
		_dragRotation.enabled = false;
		_boxCollider.enabled = false;
		_currentCycleCount = 0;
		_totalAngle = 0f;
		Debug.Log ("Bring On The Next Dish");
	}

	void GetIncrement(bool correctDirection){
		_tempAngleDifference = Mathf.Abs (transform.eulerAngles.z - _originalAngle);
		if (_tempAngleDifference > 180f) {
			_tempAngleDifference = 360f - _tempAngleDifference;
		}
		_angleIncrement = _tempAngleDifference;
		_gotIncrement = true;
	}

	void OnEnable(){
		Events.G.AddListener<DragRotationEvent> (DragRotationHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<DragRotationEvent> (DragRotationHandle);
	}

}
