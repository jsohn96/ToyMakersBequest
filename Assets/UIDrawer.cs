using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDrawer : MonoBehaviour {
	bool _isOpening = false;
	[SerializeField] Vector3 _closePos;
	[SerializeField] Vector3 _openPos;

	[SerializeField] Camera _uiCamera;
	[SerializeField] Transform _mainCamera;
	Vector3 _mainCamDefaultPos = new Vector3(0.0f, 0.0f, -10.0f);
	Vector3 _mainCamShiftPos = new Vector3(1.7f, 0.0f, -10.0f);

	Vector3 _tempPos;
	Vector3 _mainCamTempPos;
	Timer _drawerTimer = new Timer (1.0f);

	AudioSource _audioSource;

	int _uiDrawerLayerMask = 1 << 11;
	// ignore raycast mask
	int _ignoreLayerMask = 1 << 2;
	int _finalLayerMask;

	// Use this for initialization
	void Start () {
		_finalLayerMask = _uiDrawerLayerMask | _ignoreLayerMask;
		_audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		if(_isOpening){
			transform.localPosition = Vector3.Lerp(_tempPos, _openPos, _drawerTimer.PercentTimePassed);
			_mainCamera.localPosition = Vector3.Lerp(_mainCamTempPos, _mainCamShiftPos, _drawerTimer.PercentTimePassed);
		} else {
			transform.localPosition = Vector3.Lerp(_tempPos, _closePos, _drawerTimer.PercentTimePassed);
			_mainCamera.localPosition = Vector3.Lerp(_mainCamTempPos, _mainCamDefaultPos, _drawerTimer.PercentTimePassed);
		}


		Ray ray = _uiCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		Ray rayMain = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitMain;
		if(Input.GetMouseButtonDown(0)){
			if(Physics.Raycast(ray, out hit, Mathf.Infinity ,_finalLayerMask)){
				if(hit.collider.gameObject.tag == "Drawer"){
					_tempPos = transform.localPosition;
					_mainCamTempPos = _mainCamera.localPosition;
					_isOpening = !_isOpening;
					_drawerTimer.Reset();
					_audioSource.Play();
				}
			} else if(Physics.Raycast(rayMain, out hitMain, Mathf.Infinity ,_finalLayerMask)){
				if(hitMain.collider.gameObject.tag == "Drawer"){
					_tempPos = transform.localPosition;
					_mainCamTempPos = _mainCamera.localPosition;
					_isOpening = !_isOpening;
					_drawerTimer.Reset();
					_audioSource.Play();
				}
			}
		}
	}

}
