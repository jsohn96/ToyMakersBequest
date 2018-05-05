﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreLighting : MonoBehaviour {
	[SerializeField] Light _viewingLightToBeTurnedOff;

	[SerializeField] Light[] _initialLights;
	[SerializeField] Light[] _nextLights;
	[SerializeField] Light[] _nextLights2;
	[SerializeField] Light[] _nextLights3;
	[SerializeField] Light[] _nextLights4;
	[SerializeField] Light[] _nextLights5;
	[SerializeField] Light[] _nextLights6;
	[SerializeField] Light[] _nextLights7;
	[SerializeField] Light[] _nextLights8;
	[SerializeField] Light[] _nextLights9;
	[SerializeField] Light[] _nextLights10;

	[SerializeField] Light _chestLight;
	[SerializeField] Light _cabinetLight;

	[SerializeField] Light _singularSpotLight;

	void Start(){
		_viewingLightToBeTurnedOff.enabled = false;
		DisableAll ();
	}

	public void MoveToNextLights(){
		for (int i = 0; i < _initialLights.Length; i++) {
			_initialLights [i].enabled = false;
		}
		for (int i = 0; i < _nextLights.Length; i++) {
			_nextLights [i].enabled = true;
		}
	}

	public void DisableAll(){
//		if (_initialLights != null) {
			for (int i = 0; i < _initialLights.Length; i++) {
				_initialLights [i].enabled = false;
			}
//		}
//		if (_nextLights != null) {
			for (int i = 0; i < _nextLights.Length; i++) {
				_nextLights [i].enabled = false;
			}
//		}
//		if (_nextLights2 != null) {
			for (int i = 0; i < _nextLights2.Length; i++) {
				_nextLights2 [i].enabled = false;
			}
//		}
//		if (_nextLights3 != null) {
			for (int i = 0; i < _nextLights3.Length; i++) {
				_nextLights3 [i].enabled = false;
			}
//		}
		for (int i = 0; i < _nextLights4.Length; i++) {
			_nextLights4 [i].enabled = false;
		}
		for (int i = 0; i < _nextLights5.Length; i++) {
			_nextLights5 [i].enabled = false;
		}
		for (int i = 0; i < _nextLights6.Length; i++) {
			_nextLights6 [i].enabled = false;
		}
		for (int i = 0; i < _nextLights7.Length; i++) {
			_nextLights7 [i].enabled = false;
		}
		for (int i = 0; i < _nextLights8.Length; i++) {
			_nextLights8 [i].enabled = false;
		}
		for (int i = 0; i < _nextLights9.Length; i++) {
			_nextLights9 [i].enabled = false;
		}
		for (int i = 0; i < _nextLights10.Length; i++) {
			_nextLights10 [i].enabled = false;
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
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			Set5 ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			Set6 ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			Set7 ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha8)) {
			Set8 ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			Set9 ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			Set10 ();
		}
		if (Input.GetKeyDown (KeyCode.Minus)) {
			Set11 ();
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

	public void Set5()
	{
		DisableAll ();
		for (int i = 0; i < _nextLights4.Length; i++) {
			_nextLights4 [i].enabled = true;
		}
	}

	public void Set6()
	{
		DisableAll ();
		for (int i = 0; i < _nextLights5.Length; i++) {
			_nextLights5 [i].enabled = true;
		}
	}

	public void Set7()
	{
		DisableAll ();
		for (int i = 0; i < _nextLights6.Length; i++) {
			_nextLights6 [i].enabled = true;
		}
	}

	public void Set8()
	{
		DisableAll ();
		for (int i = 0; i < _nextLights7.Length; i++) {
			_nextLights7 [i].enabled = true;
		}
	}

	public void Set9()
	{
		DisableAll ();
		for (int i = 0; i < _nextLights8.Length; i++) {
			_nextLights8 [i].enabled = true;
		}
	}

	public void Set10()
	{
		DisableAll ();
		for (int i = 0; i < _nextLights9.Length; i++) {
			_nextLights9 [i].enabled = true;
		}
	}

	public void Set11()
	{
		DisableAll ();
		for (int i = 0; i < _nextLights10.Length; i++) {
			_nextLights10 [i].enabled = true;
		}
	}

	public void ChestLight(bool on){
		if (on) {
			_chestLight.enabled = true;
		} else {
			_chestLight.enabled = false;
		}
	}

	public void CabinetLight(bool on){
		if (on) {
			_cabinetLight.enabled = true;
		} else {
			_cabinetLight.enabled = false;
		}
	}

	public void SingularSpotLight(bool on){
		if (on) {
			DisableAll ();
			_singularSpotLight.enabled = true;
		} else {
			_singularSpotLight.enabled = false;
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

	[ContextMenu ("Set to Next Light 4")]
	void Debug04()
	{
		Set5();
	}

	[ContextMenu ("Set to Next Light 5")]
	void Debug05()
	{
		Set6();
	}


	[ContextMenu ("Turn All Lights Off")]
	void Debug06()
	{
		DisableAll();
	}
	#endregion
}
