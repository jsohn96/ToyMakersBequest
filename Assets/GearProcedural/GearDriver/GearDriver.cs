// GearDriver.cs (C)2015 by Alexander Schlottau, Hamburg, Germany
//   simulates procedural gear and worm gear objects at runtime.


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GearDriver : MonoBehaviour {

	[Serializable]
	public class Settings {

		public bool isMotor, isShaft, isWorm = false, invWormOut = false;
		public bool updateOnce = false;
		public bool updateLive = false;
		public float motorSpeed = 0.0f;
		public List<GearDriver> outputTo;
	}

	public  Settings settings;

	// use 'motorSpeedRPM' to set speed of motor gear from other scripts or playmaker 
	//  during gameplay.
	//
	//  For Playmaker:
	//   use an Action -> Unity Object -> Set Property
	//   As Target object select 'gear'
	//	 As Property select 'motorSpeedRPM'
	//   Then set a value for the speed or select another variable you stored in playmaker

	public float motorSpeedRPM {
		get { return actualSpeed / 6.0f;}
		set { if (settings.isMotor)
				settings.motorSpeed = value * 6.0f; }
	}

	private float actualSpeed = 0.0f;
	private int error = 0;
	private bool lastUpdState = false;

	void Start () {
		
		if (GetComponent<ProceduralWormGear> () != null) 
			settings.isWorm = true;
		else
			if (GetComponent<ProceduralGear> () == null)
				settings.isShaft = true;
			else
				settings.isShaft = false;
		
		if (settings.isMotor) {
			error++;
			UpdateConnections ( settings.isShaft? 0 : GetTeethCountFromGearScript(), settings.motorSpeed, error, settings.updateLive);
		}
	}
	
	private int GetTeethCountFromGearScript() {
		
		if (GetComponent<ProceduralWormGear> () != null)
			if (GetComponent<ProceduralWormGear> ().prefs.lr)
				return settings.invWormOut?1:-1;
			else
				return settings.invWormOut?-1:1;
		else
			if (GetComponent<ProceduralGear> () != null)
				return GetComponent<ProceduralGear> ().prefs.teethCount;
			else
				return 0;
	}

	void Update () {
	
		if (settings.isMotor)
			DriveMotor ();
		
		if (!settings.updateLive)
			gameObject.transform.Rotate (Vector3.up * Time.deltaTime * -actualSpeed);
	}

	private void DriveMotor() {

		if (settings.updateOnce || settings.updateLive || lastUpdState)
			UpdatePowerchain ();

	}
	
	public void UpdateConnections(int _otherTeethCount, float _speed, int _error, bool _updateRotation) {

		if (!settings.isMotor) {
			if (error == _error) {
				Debug.LogWarning ("GearDriver.cs : Get two inputs on " + gameObject.name + " . Check connections for loop.");
				this.enabled = false;
				return;
			}
			settings.updateLive = _updateRotation;
		}
		error = _error;

		int tc = 0;
		if (_otherTeethCount == 0) {
			if (!settings.isShaft)
				_otherTeethCount = -GetTeethCountFromGearScript(); 
			else
				_otherTeethCount = 1;
		}
		if (!settings.isShaft) {
			tc = GetTeethCountFromGearScript(); 
			actualSpeed = (float)_otherTeethCount / (float)tc * -_speed;
		}
		else
			actualSpeed = _speed;

		for (int i = 0; i < settings.outputTo.Count; i++) {
			if (settings.outputTo[i] != null) {
				settings.outputTo[i].UpdateConnections (tc, actualSpeed, error, _updateRotation);
			} else {
				settings.outputTo.RemoveAt(i);
			}
		}

		if (_updateRotation)
			gameObject.transform.Rotate (Vector3.up * Time.deltaTime * -actualSpeed);
	}
	
	public void UpdatePowerchain() {

		error = error>8?0:error+=2;
		lastUpdState = settings.updateLive;
		UpdateConnections (settings.isShaft? 0 : GetTeethCountFromGearScript(), settings.motorSpeed, error, lastUpdState);
		settings.updateOnce = false;
	}
		
}
