using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCrank : MonoBehaviour {
	[SerializeField] Camera _mainCamera;
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


			if (Input.GetAxis ("Mouse ScrollWheel") > 0f ) {
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
			if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)){
				PlayCrankSound ();
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
