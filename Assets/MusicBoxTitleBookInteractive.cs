using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxTitleBookInteractive : BookInteractive {

	[SerializeField] Transform _dancer;
	[SerializeField] Transform _dancerPos;
	[SerializeField] Transform _spotLightTransform;
	[SerializeField] Transform _rotateAroundPivot;
	[SerializeField] BookAudioController _bookAudioController;
	Light _spotLight;
	MinMax _spotLightAngle = new MinMax(65f, 80f);
	MinMax _dancerRotateAngleBound= new MinMax (20f, 120f);
	Quaternion _originRotation;
	bool _beginRotating = false;
	bool _beginTurningAround = false;


	public override void Interact(){
		base.Interact ();
		_beginTurningAround = !_beginTurningAround;
		if (_beginTurningAround) {
			_bookAudioController.TitleBoxMoving (2);
		} else {
			_bookAudioController.TitleBoxMoving (1);
		}
	}

	void Start(){
		_originRotation = _rotateAroundPivot.rotation;
	}

	void OnEnable() {
		_bookAudioController.TitleBoxMoving (1);
		_spotLight = _spotLightTransform.GetComponent<Light> ();

		float tempYRot = _rotateAroundPivot.rotation.eulerAngles.y;
		tempYRot = tempYRot > 180.0f ? (tempYRot - 360f) * -1f : tempYRot;
		_spotLight.spotAngle = MathHelpers.LinMap (_dancerRotateAngleBound.Min, _dancerRotateAngleBound.Max, _spotLightAngle.Min, _spotLightAngle.Max, Mathf.Clamp(tempYRot, _dancerRotateAngleBound.Min, _dancerRotateAngleBound.Max));
		_beginRotating = true;
	}

	void OnDisable(){
		_bookAudioController.TitleBoxMoving (0);
		_rotateAroundPivot.rotation = _originRotation;
		_beginRotating = false;
		_beginTurningAround = false;
	}

	void FixedUpdate () {
		if (_beginRotating) {
			_dancer.Rotate (Vector3.up, 1.2f);
		}
		if (_beginTurningAround) {
			_rotateAroundPivot.Rotate (Vector3.up, 0.9f);
			float tempYRot = _rotateAroundPivot.rotation.eulerAngles.y;
			tempYRot = tempYRot > 180.0f ? (tempYRot - 360f) * -1f : tempYRot;
			_spotLight.spotAngle = MathHelpers.LinMap (_dancerRotateAngleBound.Min, _dancerRotateAngleBound.Max, _spotLightAngle.Min, _spotLightAngle.Max, Mathf.Clamp(tempYRot, _dancerRotateAngleBound.Min, _dancerRotateAngleBound.Max));
		}
		_spotLightTransform.LookAt (_dancerPos);
	}
}
