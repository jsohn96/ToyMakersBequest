using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTestScript : MonoBehaviour {
	AudioSource _audioSource;
	Quaternion _originRot;
	Quaternion _otherRot;

	Vector3 _tempAngle;
	Timer _rotateTimer;

	public bool _isOther = true;

	public bool _peg = false;
	public Transform _inside;
	Vector3 _position;
	Vector3 _originPos;

	public bool _removeCircle = false;
	// Use this for initialization
	void Start () {
		_audioSource = GetComponent<AudioSource> ();
		_originRot = transform.rotation;
		_tempAngle = _originRot.eulerAngles;
		_tempAngle.x += 180.0f;
		_otherRot = Quaternion.Euler (_tempAngle);
		if (!_peg) {
			_inside = transform.GetChild (0);
		}
		transform.rotation = _otherRot;
		if (_removeCircle) {
			_originPos = transform.position;
			_position = transform.position;
			_position.x = 4f;

		}
	}


	public void PlayNote(){
		_audioSource.Play ();
	}
	
	// Update is called once per frame
	void OnMouseOver() {
		if (!_peg) {
			if (Input.GetMouseButtonDown (0)) {
				if (_isOther) {
					transform.rotation = _originRot;
					_isOther = false;
					if (_removeCircle) {
						transform.position = _position;
					}
				} else {
					transform.rotation = _otherRot;

						_isOther = true;
					if (!_removeCircle) {
						PlayNote ();
					}

					if (_removeCircle) {
						transform.position = _originPos;
					}
				}

			}
		}
	}

	IEnumerator DelayThing(){
		yield return new WaitForSeconds (1.0f);
		_isOther = true;
	}
}
