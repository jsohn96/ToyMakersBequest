using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TheatreStarsFade : MonoBehaviour {

	[SerializeField] Image _fadeImage;
	Color _imageColor;

	float[] _fadeValues = new float[]{0.3f, 0.6f, 0.7f, 0.85f, 1f};
	int _whichValue = -1;

	IEnumerator _fadeCoroutine;

	void Start(){
		_imageColor = _fadeImage.color;
	}

	void OnEnable(){
		Events.G.AddListener<TheatreFadeOutStarsEvent> (FadeAway);
	}

	void OnDisable(){
		Events.G.RemoveListener<TheatreFadeOutStarsEvent> (FadeAway);
	}

	void FadeAway(TheatreFadeOutStarsEvent e){
		if (_whichValue < 4) {
			if (_fadeCoroutine != null) {
				StopCoroutine (_fadeCoroutine);
			}
			_fadeCoroutine = FadeCoroutine();
			StartCoroutine (_fadeCoroutine);
		}
	}

	IEnumerator FadeCoroutine(){
		_whichValue++;
		float duration = 1f;
		float timer = 0f;

		_imageColor.a = _fadeValues [_whichValue];
		Color currentColor = _fadeImage.color;

		while (duration > timer) {
			timer += Time.deltaTime;
			_fadeImage.color = Color.Lerp (currentColor, _imageColor, timer / duration);
			yield return null;
		}
		_fadeImage.color = _imageColor;

		if (_whichValue >= 4) {
			yield return new WaitForSeconds (2f);
			SceneManager.LoadScene (7);
		}
		yield return null;
	}
}
