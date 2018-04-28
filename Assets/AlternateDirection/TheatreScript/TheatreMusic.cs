using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreMusic : AudioSourceController {
	[SerializeField] AudioSystem _theatreMusic;

	[SerializeField] AudioSystem _ambientRoomTone;

	void Start(){
		Play (_ambientRoomTone);
	}

	public void BeginMusic(){
		Stop (_ambientRoomTone);
		StartCoroutine (DelayMusicStart ());
	}

	IEnumerator DelayMusicStart(){
		yield return new WaitForSeconds (1f);
		Play (_theatreMusic);
	}


	public void EndMusic(){
		Stop (_theatreMusic);
		Play (_ambientRoomTone);
	}
}
