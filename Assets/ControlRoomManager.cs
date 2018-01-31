using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlRoomManager : MonoBehaviour {
	
	[SerializeField] TextMeshPro _tmpInstruction;

	[SerializeField] ControlRoomAudio _controlRoomAudio;

	[SerializeField] Fading _fading;

	[SerializeField] Camera _mainCamera;
	[SerializeField] Camera _peepCamera;
	[SerializeField] Canvas _mainCanvas;
	[SerializeField] Canvas _peepCanvas;

	[SerializeField] AltPeepText _peepText;

	void Start(){
		_peepCamera.enabled = false;
		_peepCanvas.enabled = false;
		_mainCamera.enabled = true;
		_mainCanvas.enabled = true;
	}

	public void LookIntoPeephole(int peepholeIndex){
		_tmpInstruction.enabled = false;
		_controlRoomAudio.PlayZoomAudio ();
		_fading.BeginFade (1);
		StartCoroutine (WaitForCameraSwap (1f));
		_peepText.ChangeText (peepholeIndex);
	}

	public void ZoomOutOfPeephole(){
		_controlRoomAudio.PlayZoomAudio ();
		_fading.BeginFade (1);
		StartCoroutine (WaitForCameraSwap (-1f));
	}

	IEnumerator WaitForCameraSwap(float duration){
		if (duration > 0f) {
			yield return new WaitForSeconds (duration);
			_mainCamera.enabled = false;
			_mainCanvas.enabled = false;
			_peepCamera.enabled = true;
			_peepCanvas.enabled = true;
		} else {
			yield return new WaitForSeconds (-duration);
			_mainCamera.enabled = true;
			_mainCanvas.enabled = true;
			_peepCamera.enabled = false;
			_peepCanvas.enabled = false;
		}
		_fading.BeginFade (-1);
	}
}
