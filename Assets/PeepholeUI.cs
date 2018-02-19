using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PeepholeUI : MonoBehaviour {

	[SerializeField] Fading _fadeScript;

	public void ExitPeephole(){
		StartCoroutine (ChangeLevel ());
	}


	IEnumerator ChangeLevel(){
		yield return new WaitForSeconds(0.5f);
		float fadeTime = _fadeScript.BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene ("ControlRoom");
	}
}
