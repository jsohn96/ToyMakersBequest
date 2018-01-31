using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AltDirectionUI : MonoBehaviour {
	public void SceneChange(int sceneIndex){
		StartCoroutine (ChangeLevel (sceneIndex));
	}


	IEnumerator ChangeLevel(int sceneIndex){
		yield return new WaitForSeconds(0.2f);
		Fading fadeScript = GameObject.Find ("Fade").GetComponent<Fading> ();
		float fadeTime = fadeScript.BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene (sceneIndex);
	}
}
