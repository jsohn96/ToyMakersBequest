using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FestivalDebugKeys : MonoBehaviour {
	[SerializeField] int _startSceneBuildIndex;
	[SerializeField] int _gameSetUpSceneBuildIndex;


	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			SceneManager.LoadScene (_startSceneBuildIndex);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			SceneManager.LoadScene (_gameSetUpSceneBuildIndex);
		}
		if(Input.GetKeyDown(KeyCode.R)){
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}
}
