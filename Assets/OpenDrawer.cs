using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDrawer : MonoBehaviour {
	bool _isOpening = false;
	Vector3 _closePos = new Vector3(1.12570f, -0.3427f, 0.59532f);
	Vector3 _openPos = new Vector3(0.8395f, -0.3427f, 0.59532f);
	Vector3 _tempPos;
	Timer _drawerTimer = new Timer (1.0f);

	AudioSource _audioSource;

	// Use this for initialization
	void Start () {
		_audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_isOpening){
			transform.localPosition = Vector3.Lerp(_tempPos, _openPos, _drawerTimer.PercentTimePassed);
		} else {
			transform.localPosition = Vector3.Lerp(_tempPos, _closePos, _drawerTimer.PercentTimePassed);
		}


			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
		if(Input.GetMouseButtonDown(0)){
			if(Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Box"))){
				if(hit.collider.gameObject.tag == "Drawer"){
					_tempPos = transform.localPosition;
					_isOpening = !_isOpening;
					_drawerTimer.Reset();
					_audioSource.Play();
				}
			}
		}
	}

}
