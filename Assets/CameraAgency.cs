using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAgency : MonoBehaviour {

	float maxDragSpeed = 0.2f;
	private Vector3 dragOrigin;
	float _constraint = 3f;
	Vector3 _tempRotation;

	bool _isAgencying = false;

	void Update()
	{

		//TODO: Only enable when nothing of relevance is clicked

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit,  Mathf.Infinity)){
				if (hit.collider.gameObject.tag != "RotateCircle") {
					dragOrigin = Input.mousePosition;
					_isAgencying = true;
				}
			}
			else {
				dragOrigin = Input.mousePosition;
				_isAgencying = true;
			}
		}

		if (!Input.GetMouseButton (0)) {
			_isAgencying = false;
		}

		if(_isAgencying){
		float x = Input.mousePosition.x - dragOrigin.x;
		float y = Input.mousePosition.y - dragOrigin.y;

		Debug.Log ("x: " + x);
		Debug.Log ("y: " + y);

	//	Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
		//Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
		if (x != 0) {
			transform.Rotate (Vector3.up, (x/Screen.width) * maxDragSpeed);
		} 
		//else if (x < 0) {
		//	transform.Rotate (Vector3.up, -Time.deltaTime * maxDragSpeed);
		//}



		if (y != 0) {
			transform.Rotate (Vector3.right, -y/Screen.width * maxDragSpeed);
		}

		_tempRotation = transform.localRotation.eulerAngles;

		float tempFloatX = Mathf.Abs(_tempRotation.x);
		float tempFloatY = Mathf.Abs(_tempRotation.y);
		if (tempFloatX > 180.0f) {
			tempFloatX -= 360.0f;
		}
		if (tempFloatY > 180.0f) {
			tempFloatY -= 360.0f;
		}

		if (Mathf.Abs (tempFloatX) > _constraint) {
			_tempRotation.x = (tempFloatX > 0) ? _constraint : 360.0f - _constraint;
		}
		if (Mathf.Abs (tempFloatY) > _constraint) {
			_tempRotation.y = (tempFloatY > 0) ? _constraint : 360.0f - _constraint;
		}
		_tempRotation.z = 0.0f;

		transform.localRotation = Quaternion.Euler (_tempRotation);

		//transform.Rotate (Vector3.up, Time.deltaTime);

		//transform.Translate(move, Space.World); 
		}
	}
}
