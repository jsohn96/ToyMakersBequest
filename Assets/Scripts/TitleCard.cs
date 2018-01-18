using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCard : MonoBehaviour {

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			StartCoroutine (ChangeLevel ());
		}


	}


	//StartCoroutine(ChangeLevel());
	IEnumerator ChangeLevel(){
		yield return new WaitForSeconds (0.5f);
		float fadeTime = GameObject.Find ("Fade").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene (4);
	}
}
