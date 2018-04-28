using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheatrePhoto : MonoBehaviour {

	[SerializeField] Image _image;
	[SerializeField] AnimationCurve _flashCurve;
	Color _fullColor = new Color (1f, 1f, 1f, 1f);
	Color _emptyColor = new Color (1f, 1f, 1f, 0f);

//	Vector3 _endPos = new Vector3 (-1.61f, 12.489f, 18.082f);
//	Vector3 _endRot = new Vector3 (14.42f, -90f, 0f);
//	Quaternion _endRotQuat;

	[SerializeField] AudioSource _audioSource;

	[SerializeField] SpriteRenderer _photoSpriteRenderer;
	[SerializeField] BoxCollider _boxCollider;

	[SerializeField] AltTheatre _myTheatre;


	public void TakeFlashPhoto(){
		StartCoroutine (FlashPhotoCoroutine ());
	}

	IEnumerator FlashPhotoCoroutine(){
		float duration = 0.5f;
		float timer = 0f;
		bool once = false;
		while (duration > timer) {
			timer += Time.deltaTime;
			_image.color = Color.Lerp (_emptyColor, _fullColor, _flashCurve.Evaluate (timer / duration));

			if (!once) {
				if ((timer / duration) > 0.1f) {
					_audioSource.Play ();
					_photoSpriteRenderer.enabled = true;
					once = true;
				}
			}

			yield return null;
		}
		_image.color = _emptyColor;
		_image.enabled = false;
		_boxCollider.enabled = true;
		yield return null;
	}

	void OnTouchDown(Vector3 hit){
		_myTheatre.MoveToNext ();
		StartCoroutine (PutAwayPhoto ());
	}


	IEnumerator PutAwayPhoto(){
		_boxCollider.enabled = false;
//		_endRotQuat = Quaternion.Euler (_endRot);
//		Vector3 originPos = transform.position;
//		Quaternion originRot = transform.rotation;
//		Vector3 tempPos;
//		Quaternion tempRot;
		float duration = 1f;
		float timer = 0f;
		float tempLinMap;
		while (duration > timer) {
			timer += Time.deltaTime;
			tempLinMap = timer / duration;
			_photoSpriteRenderer.color = Color.Lerp (_fullColor, _emptyColor, tempLinMap);
//			tempPos = Vector3.Lerp (originPos, _endPos, tempLinMap);
//			tempRot = Quaternion.Lerp (originRot, _endRotQuat, tempLinMap);
//			transform.SetPositionAndRotation (tempPos, tempRot);
			yield return null;
		}
//		transform.SetPositionAndRotation (_endPos, _endRotQuat);
		_photoSpriteRenderer.color = _emptyColor;
		yield return null;
	}
}
