using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltPeephole : MonoBehaviour {
	[SerializeField] TextMeshPro _tmpInstruction;

	[SerializeField] ControlRoomAudio _controlRoomAudio;

	[SerializeField] Fading _fading;

	[SerializeField] Camera _mainCamera;
	[SerializeField] Camera _peepCamera;

	[SerializeField] AltPeepText _peepText;

	[Header("Which Peephole? 0-3")]
	public int _peepHoleIndex;

	bool _thisPeepHoleActivated = false;

	void Start(){
		_peepCamera.enabled = false;
		CheckPeepHoleActivation ();
	}

	void Update(){
		//TODO: Take this out of Update
		CheckPeepHoleActivation ();
	}

	void CheckPeepHoleActivation(){
		if ((int)AltCentralControl._currentState >= _peepHoleIndex) {
			_thisPeepHoleActivated = true;
		} else {
			_thisPeepHoleActivated = false;
		}
	}

	void OnMouseDown(){
		if (_thisPeepHoleActivated) {
			_tmpInstruction.enabled = false;
			_controlRoomAudio.PlayZoomAudio ();
			_fading.BeginFade (1);
			StartCoroutine (WaitForThisManySeconds (1f));
			_peepText.ChangeText (_peepHoleIndex);
		}
	}

	IEnumerator WaitForThisManySeconds(float duration){
		if (duration > 0f) {
			yield return new WaitForSeconds (duration);
			_mainCamera.enabled = false;
			_peepCamera.enabled = true;
		} else {
			yield return new WaitForSeconds (-duration);
			_mainCamera.enabled = true;
			_peepCamera.enabled = false;
		}
		_fading.BeginFade (-1);
	}

	public void ZoomOut(){
		_controlRoomAudio.PlayZoomAudio ();
		_fading.BeginFade (1);
		StartCoroutine (WaitForThisManySeconds (-1f));
	}
}
