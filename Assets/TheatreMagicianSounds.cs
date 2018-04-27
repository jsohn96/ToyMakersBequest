using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreMagicianSounds : MonoBehaviour {
	[SerializeField] TheatreSound _theatreSound;

	public void PlayMagicRevealSound(){
		_theatreSound.PlayMagicRevealSound ();
	}
}
