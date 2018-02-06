using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPulse : MonoBehaviour {
	Vector3 _screenPos;
	Vector3 _worldPos;
	Quaternion _tempRotation;
	[SerializeField] Camera _touchCamera;
	[SerializeField] ParticleSystem[] _particle = new ParticleSystem[3];
	int _particleCnt = 0;
	float _cameraZPos;


	public static TouchPulse instance = null;

	void Awake () {
		//assign an instance of this gameobject if it hasn't been assigned before
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);
	}

	void Start(){
		_cameraZPos = _touchCamera.transform.position.z;
	}

	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			_screenPos = Input.mousePosition;
			_screenPos.z = _cameraZPos;
			_worldPos = _touchCamera.ScreenToWorldPoint (_screenPos);
//			_tempRotation = _touchCamera.transform.rotation;
			if (_particleCnt >= 3) {
				_particleCnt = 0;
			}
			_particle[_particleCnt].transform.position = _worldPos;
			_particle [_particleCnt].Play ();
			_particleCnt++;
		}
	}
}
