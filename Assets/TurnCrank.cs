using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCrank : MonoBehaviour {
	[SerializeField] Camera _mainCamera;
	public bool _isHoldingCrank = false;
	float _crankTurnSensitivity = 1000.0f;
	int _traversalExclusionLayerMask = 1 << 8;

	[SerializeField] Transform[] _otherGears;
	[SerializeField] AudioClip[] _crankClips;
	AudioSource _audioSource;
	// Use this for initialization
	void Awake () {
		_traversalExclusionLayerMask = ~_traversalExclusionLayerMask;
		_audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			
			Ray ray = _mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, _traversalExclusionLayerMask)) {
				if (hit.collider.name == "Crank") {
					_isHoldingCrank = true;
					Debug.Log ("click works");
				}
			}
		} else if (Input.GetMouseButtonUp(0)) {
			_isHoldingCrank = false;
		}

		if (_isHoldingCrank) {
			if (Input.GetAxis ("Mouse ScrollWheel") > 0f) {
				PlayCrankSound ();

				transform.Rotate (Vector3.right * Time.deltaTime * _crankTurnSensitivity);

				for (int i = 0; i < _otherGears.Length; i++) {
					_otherGears [i].Rotate (Vector3.down * Time.deltaTime * 300.0f);
				}
			} else if (Input.GetAxis ("Mouse ScrollWheel") < 0f) {
				PlayCrankSound ();
				transform.Rotate (Vector3.left * Time.deltaTime * _crankTurnSensitivity);

				for (int i = 0; i < _otherGears.Length; i++) {
					_otherGears [i].Rotate (Vector3.up * Time.deltaTime * 300.0f);
				}
			}
		}
	}

	void PlayCrankSound(){
		if (!_audioSource.isPlaying) {
			int index = Random.Range (0, 4);
			_audioSource.clip = _crankClips [index];
			_audioSource.Play ();
		}
	}
}
