using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDrawer : MonoBehaviour {
	bool _isOpening = false;
	[SerializeField] Vector3 _closePos;
	[SerializeField] Vector3 _openPos;
	Vector3 _tempPos;
	Timer _drawerTimer = new Timer (1.0f);

	AudioSource _audioSource;

	int _boxLayerMask = 1 << 10;

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
			if(Physics.Raycast(ray, out hit, Mathf.Infinity ,_boxLayerMask)){
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
