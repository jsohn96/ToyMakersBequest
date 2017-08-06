using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour {
	Camera _mainCamera;
	bool _carrying;
	GameObject _carriedObject;
	[SerializeField] float _distance;
	[SerializeField] float _smooth;

	void Awake () {
			_mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if(_carrying){
			Carry(_carriedObject);
			CheckDrop();
		} else {
			Pickup();
		}
	}

	void RotateObject(){
		_carriedObject.transform.Rotate(5,10,15);
	}

	void Carry(GameObject carriedObject){
		carriedObject.transform.position = Vector3.Lerp(carriedObject.transform.position, _mainCamera.transform.position + _mainCamera.transform.forward * _distance, Time.deltaTime * _smooth);
	}

	void Pickup(){
		if(Input.GetMouseButtonDown(0)){
			Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)){
				Pickupable p = hit.collider.GetComponent<Pickupable>();
				if(p != null){
					_carrying = true;
					_carriedObject = p.gameObject;
					p.GetComponent<Rigidbody>().isKinematic = true;
				}
			}
		}
	}

	void CheckDrop(){
		if(Input.GetMouseButtonDown(0)){
			DropObject();
		}
	}

	void DropObject(){
		_carrying =false;
		_carriedObject.GetComponent<Rigidbody>().isKinematic = false;
		_carriedObject = null;
	}
}
