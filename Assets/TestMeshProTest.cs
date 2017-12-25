using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestMeshProTest : MonoBehaviour {
	Color _emptyColor;
	Color _fullColor;
	TextMeshPro _textMeshPro;
	[SerializeField] Material _material;
	float _dilateOrigin = 0.0f;
	float _dilateGoal = -1.0f;

	Timer _fadeTimer;
	bool _isTrigger = false;

	// Use this for initialization
	void Start () {
		_textMeshPro = GetComponent<TextMeshPro> ();
		_material = GetComponent<MeshRenderer>().material;
		_textMeshPro.color = Color.white;
		_fullColor = _textMeshPro.color;
		_emptyColor = _fullColor;
		_emptyColor.a = 0.0f;
		_fadeTimer = new Timer (2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			_fadeTimer.Reset ();
			_isTrigger = true;
		}
		if (_isTrigger) {
			if (!_fadeTimer.IsOffCooldown) {
				_textMeshPro.color = Color.Lerp (_fullColor, _emptyColor, _fadeTimer.PercentTimePassed* (3f/2f));
				float tempDilate = Mathf.Lerp (_dilateOrigin, _dilateGoal, _fadeTimer.PercentTimePassed);
				_material.SetFloat (ShaderUtilities.ID_FaceDilate, tempDilate);
			} else {
				_textMeshPro.color = _emptyColor;
				_material.SetFloat (ShaderUtilities.ID_FaceDilate, _dilateGoal);
			}
		}
	}
}
