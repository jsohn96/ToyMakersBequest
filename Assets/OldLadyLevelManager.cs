using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldLadyLevelManager : LevelManager {
	[SerializeField] Camera _mainCamera;
	[SerializeField] Transform _nextCameraView;
	float _cameraLerpDuration = 3f;
	Vector3 _tempPos;
	Quaternion _tempRot;

	public void MoveToNextCameraView(){
		StartCoroutine (LerpCameraPos());
	}

	public override void PickUpCharm(){
		base.PickUpCharm ();
		AltCentralControl._regret = true;
		AltCentralControl._currentState = (AltStates)((int)AltCentralControl._currentState+1);
		InventorySystem._instance.AddItem (items.oldLadyCharm);
		Events.G.Raise (new PickedUpItem (items.oldLadyCharm));
	}

	IEnumerator LerpCameraPos(){
		float timer = 0f;
		_tempPos = _mainCamera.transform.position;
		_tempRot = _mainCamera.transform.rotation;
		while (timer < _cameraLerpDuration) {
			timer += Time.deltaTime;
			_mainCamera.transform.SetPositionAndRotation (
				Vector3.Slerp(_tempPos, _nextCameraView.position, timer/_cameraLerpDuration),
				Quaternion.Slerp(_tempRot, _nextCameraView.rotation, timer/_cameraLerpDuration)
			);
			yield return null;
		}
		_mainCamera.transform.SetPositionAndRotation (_nextCameraView.position, _nextCameraView.rotation);
		yield return null;
	}
}
