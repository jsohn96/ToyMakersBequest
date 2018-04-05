using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraversalUI : MonoBehaviour {

	[SerializeField] Image[] _buttonImages;
	[SerializeField] ButtonSystem[] _buttonSystem;
	[SerializeField] BoxCollider[] _tapSoundLayerCollider;
	[SerializeField] BoxCollider[] _touchInputLayerCollider;

	int _buttonImagesLength;
	Color _fullColor = new Color(1.0f, 1f, 1f, 1f);
	Color _emptyColor = new Color(1f,1f,1f,0f);

	bool _isOn = false;

	void Start(){
		_buttonImagesLength = _buttonImages.Length;
		for (int i = 0; i < _buttonImagesLength; i++) {
			_buttonImages [i].raycastTarget = false;
			_tapSoundLayerCollider [i].enabled = false;
			_touchInputLayerCollider [i].enabled = false;
		}
	}

	void Update(){
		if (_isOn) {
			for (int i = 0; i < _buttonImagesLength; i++) {
				_buttonSystem [i].AnimateArrow ();
			}
		}
	}

	public void FadeIn(bool isLowerPriority = false, int buttonIndex = 4){
		if (isLowerPriority) {
			if (!_isOn) {
				return;
			}
		} else {
			_isOn = true;
		}
		StartCoroutine (FadeButtons (true, buttonIndex));
	}

	IEnumerator FadeButtons(bool fadeIn, int buttonIndex){
		float timer = 0f;
		float duration = 0.8f;
		Color tempColor = _buttonImages [0].color;
		while (timer < duration) {
			timer += Time.deltaTime;
			if(buttonIndex == 4){
				for (int i = 0; i < _buttonImagesLength; i++) {
					if (fadeIn) {
						_buttonImages [i].color = Color.Lerp (tempColor, _fullColor, timer / duration);
					} else {
						_buttonImages [i].color = Color.Lerp (tempColor, _emptyColor, timer / duration);
					}
				}
			} else {
				if(fadeIn){
					_buttonImages [buttonIndex].color = Color.Lerp (tempColor, _fullColor, timer / duration);
				} else {
					_buttonImages [buttonIndex].color = Color.Lerp (tempColor, _emptyColor, timer / duration);
				}
			}
			yield return null;
		}
		if(buttonIndex == 4){
			for (int i = 0; i < _buttonImagesLength; i++) {
				if (fadeIn) {
					_buttonImages [i].raycastTarget = true;
					_tapSoundLayerCollider [i].enabled = true;
					_touchInputLayerCollider [i].enabled = true;
					_buttonImages [i].color = _fullColor;
				} else {
					_buttonImages [i].color = _emptyColor;
				}
			}
		} else {
			if (fadeIn) {
				_buttonImages [buttonIndex].raycastTarget = true;
				_tapSoundLayerCollider [buttonIndex].enabled = true;
				_touchInputLayerCollider [buttonIndex].enabled = true;
				_buttonImages [buttonIndex].color = _fullColor;
			} else {
				_buttonImages [buttonIndex].color = _emptyColor;
			}
		}
	}

	public void FadeOut(bool isLowerPriority = false, int buttonIndex = 4){
		if (!isLowerPriority) {
			_isOn = false;
		}
		if(buttonIndex == 4){
			for (int i = 0; i < _buttonImagesLength; i++) {
				_buttonImages [i].raycastTarget = false;
				_tapSoundLayerCollider [i].enabled = false;
				_touchInputLayerCollider [i].enabled = false;
			}
		} else {
			_buttonImages[buttonIndex].raycastTarget = false;
			_tapSoundLayerCollider [buttonIndex].enabled = false;
			_touchInputLayerCollider [buttonIndex].enabled = false;
		}
		StartCoroutine (FadeButtons (false, buttonIndex));

	}
}
