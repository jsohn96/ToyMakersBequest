using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettings : MonoBehaviour {
	bool _settingsOpen = false;
	[SerializeField] RectTransform _settingsGear;
	[SerializeField] AltDirectionUI _UIManager;

	Vector3 _rotateAxis;

	void Start(){
		_rotateAxis = Vector3.back;
	}

	public void OpenSettings(){
		_settingsOpen = true;
		_UIManager.WindowScroll (false);
		Time.timeScale = 0f;
	}

	void Update(){
		if (_settingsOpen) {
			_settingsGear.Rotate (_rotateAxis *2f);
		}
	}
}
