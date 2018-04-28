using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreMagicianSounds : MonoBehaviour {
	[SerializeField] TheatreSound _theatreSound;
	[SerializeField] TheatrePhoto _theatrePhoto;

	public void PlayMagicRevealSound(){
		_theatreSound.PlayMagicRevealSound ();
	}

	public void ShootPhoto(){
		_theatrePhoto.TakeFlashPhoto ();
	}
}
