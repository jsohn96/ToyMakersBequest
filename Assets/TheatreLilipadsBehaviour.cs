using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreLilipadsBehaviour : MonoBehaviour {
	[SerializeField] lilipadAnimationBehaviour[] _lilipads;
	Vector3 _originPos;
	Vector3 _finalPos;
	bool _isUp = false;


	// Use this for initialization
	void Awake () {
		_originPos = transform.position;
		_finalPos = _originPos;
		_finalPos.y += 0.013f;
	}

	public void GoUp(){
		if (!_isUp) {
			_isUp = true;
			StartCoroutine (MoveLilyPadsUp ());
		}
	}

	IEnumerator MoveLilyPadsUp(){
		float timer = 0f;
		float duration = 1f;
		while (duration > timer) {
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp (_originPos, _finalPos, timer/duration);
			yield return null;
		}
		transform.position = _finalPos;
		yield return null;
	}

	public IEnumerator FlipBack(){
		for (int i = 0; i < 6; i++) {
			_lilipads [i].Flipback ();
			yield return new WaitForSeconds (0.3f);
		}
		
	}
}
