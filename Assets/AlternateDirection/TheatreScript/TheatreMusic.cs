using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreMusic : AudioSourceController {
	[SerializeField] AudioSystem _theatreMusic;


	public void BeginMusic(){
		StartCoroutine (DelayMusicStart ());
	}

	IEnumerator DelayMusicStart(){
		yield return new WaitForSeconds (1f);
		Play (_theatreMusic);
	}


	public void EndMusic(){
		Stop (_theatreMusic);
	}
}
