using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraversalUI : MonoBehaviour {

	[SerializeField] Image[] _buttonImages;
	int _buttonImagesLength;
	Color _fullColor = new Color(1.0f, 1f, 1f, 1f);
	Color _emptyColor = new Color(1f,1f,1f,0f);

	void Start(){
		_buttonImagesLength = _buttonImages.Length;
		for (int i = 0; i < _buttonImagesLength; i++) {
			_buttonImages [i].raycastTarget = false;
		}
	}

	public void FadeIn(){
		StartCoroutine (FadeButtons (true));
	}

	IEnumerator FadeButtons(bool fadeIn){
		float timer = 0f;
		float duration = 0.8f;
		while (timer < duration) {
			timer += Time.deltaTime;
			for (int i = 0; i < _buttonImagesLength; i++) {
				if (fadeIn) {
					_buttonImages [i].color = Color.Lerp (_emptyColor, _fullColor, timer / duration);
				} else {
					_buttonImages [i].color = Color.Lerp (_fullColor, _emptyColor, timer / duration);
				}
			}
			yield return null;
		}
		for (int i = 0; i < _buttonImagesLength; i++) {
			if (fadeIn) {
				_buttonImages [i].raycastTarget = true;
				_buttonImages [i].color = _fullColor;
			} else {
				_buttonImages [i].color = _emptyColor;
			}
		}
	}

	public void FadeOut(){
		for (int i = 0; i < _buttonImagesLength; i++) {
			_buttonImages [i].raycastTarget = false;
		}
		StartCoroutine (FadeButtons (false));

	}
}
