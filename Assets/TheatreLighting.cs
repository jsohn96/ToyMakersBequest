using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreLighting : MonoBehaviour {
	[SerializeField] Light[] _initialLights;
	[SerializeField] Light[] _nextLights;
	[SerializeField] Light[] _nextLights2;
	[SerializeField] Light[] _nextLights3;
	// Use this for initialization
	public void MoveToNextLights(){
		for (int i = 0; i < _initialLights.Length; i++) {
			_initialLights [i].enabled = false;
		}
		for (int i = 0; i < _nextLights.Length; i++) {
			_nextLights [i].enabled = true;
		}
	}

	void DisableAll(){
		if (_initialLights != null) {
			for (int i = 0; i < _initialLights.Length; i++) {
				_initialLights [i].enabled = false;
			}
		}
		if (_nextLights != null) {
			for (int i = 0; i < _nextLights.Length; i++) {
				_nextLights [i].enabled = false;
			}
		}
		if (_nextLights2 != null) {
			for (int i = 0; i < _nextLights2.Length; i++) {
				_nextLights2 [i].enabled = false;
			}
		}
		if (_nextLights3 != null) {
			for (int i = 0; i < _nextLights3.Length; i++) {
				_nextLights3 [i].enabled = false;
			}
		}
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Alpha1)) {

			Set1 ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			Set2 ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			Set3 ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			Set4 ();
		}
	}

	public void Set1()
	{
		DisableAll ();
		for (int i = 0; i < _initialLights.Length; i++) {
			_initialLights [i].enabled = true;
		}
	}

	public void Set2()
	{
		DisableAll ();
		for (int i = 0; i < _nextLights.Length; i++) {
			_nextLights [i].enabled = true;
		}
	}

	public void Set3()
	{
		DisableAll ();
		for (int i = 0; i < _nextLights2.Length; i++) {
			_nextLights2 [i].enabled = true;
		}
	}

	public void Set4()
	{
		DisableAll ();
		for (int i = 0; i < _nextLights3.Length; i++) {
			_nextLights3 [i].enabled = true;
		}
	}

	#region Debug
	[ContextMenu ("Set to Initial")]
	void Debug00()
	{
		Set1();
	}

	[ContextMenu ("Set to Next Light")]
	void Debug01()
	{
		Set2();
	}

	[ContextMenu ("Set to Next Light 2")]
	void Debug02()
	{
		Set3();
	}

	[ContextMenu ("Set to Next Light 3")]
	void Debug03()
	{
		Set4();
	}
	#endregion
}
