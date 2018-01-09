using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshProToggle : ToggleAction {
	[SerializeField] TextMeshPro _textMeshPro;
	bool _isOn = false;
	[SerializeField] bool _defaultOn = false;
	Color _emptyColor;
	Color _fullColor;

	void Start() {
		_fullColor = _textMeshPro.color;
		_emptyColor = _fullColor;
		_emptyColor.a = 0.0f;

		if (_defaultOn) {
			ToggleActionOn ();
		} else {
			_textMeshPro.color = _emptyColor;
		}
	}

	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			_isOn = !_isOn;
			if (_isOn) {
				_textMeshPro.color = _fullColor;
			} else {
				_textMeshPro.color = _emptyColor;
			}
		}


	}

	public override void ToggleActionOn(){
		base.ToggleActionOn ();
		_isOn = true;
		_textMeshPro.color = _fullColor;
	}

	void OnEnable(){
	}

	void OnDisable() {
		_isOn = false;
		_textMeshPro.color = _emptyColor;
	}
}
