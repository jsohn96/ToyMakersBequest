using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInherit : AudioSourceController {
	[Header("Header Test")][Space(5)]
	[SerializeField] AudioSystem _musicSystem;

	void Update () {
		if (Input.GetKeyDown (KeyCode.E)) {
			_musicSystem.fadeDuration = 5f;

		}


		if (Input.GetKeyDown (KeyCode.S)) {
			AudioManager.instance.RandomizePitchFromRange (_musicSystem);
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			Pause (_musicSystem);
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			Play (_musicSystem);
		}

		if (Input.GetKeyDown (KeyCode.U)) {
			Resume (_musicSystem);
		}
		if (Input.GetKeyDown (KeyCode.Y)) {
			SwapClip (_musicSystem, true);
		}
	}
}
