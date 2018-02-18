using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettings : MonoBehaviour {
	bool _settingsOpen = true;
	[SerializeField] RectTransform _settingsGear;
	[SerializeField] AltDirectionUI _UIManager;

	Vector3 _rotateAxis;
	bool _scrollingDoorMoving = true;

	void Start(){
		_rotateAxis = Vector3.back;
	}

	public void OpenSettings(){
		if (!_scrollingDoorMoving) {
			_scrollingDoorMoving = true;
			if (!_settingsOpen) {
				_settingsOpen = true;
				_UIManager.WindowScroll (true);
				AltCentralControl._instance.PauseGameTime (false);
			} else {
				_settingsOpen = false;
				_UIManager.WindowScroll (false);
			}
		}
	}

	void Update(){
		if (_scrollingDoorMoving) {
			float directionalMultiplier = _settingsOpen ? 1f : -1f;
			_settingsGear.Rotate (_rotateAxis * 100f * Time.unscaledDeltaTime * directionalMultiplier);
		}
	}


	void DoorSlidingDone(SlidingDoorFinished e){
		if (!_settingsOpen) {
			AltCentralControl._instance.PauseGameTime (true);
		}
		_scrollingDoorMoving = false;
	}

	void OnEnable(){
		Events.G.AddListener<SlidingDoorFinished> (DoorSlidingDone);
	}

	void OnDisable(){
		Events.G.RemoveListener<SlidingDoorFinished> (DoorSlidingDone);
	}
}
