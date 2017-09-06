using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour {
	public int identifier
	{
		get {return _identifier; }
		set {_identifier = value; }
	}

	public Vector3 position
	{
		get {return _position; }
		set {_position = value; }
	}

	public Vector3 eulerRotation
	{
		get { return _eulerRotation; }
		set { _eulerRotation = value; }
	}

	public Transform parent
	{
		get { return _parent; }
		set { _parent = value; }
	}

	public bool occupied
	{
		get {return _occupied; }
		set {_occupied = value; }
	}

	Vector3 _position;
	Vector3 _eulerRotation;
	[SerializeField] int _identifier = 0;
	Transform _parent;
	[SerializeField] bool _occupied = false;

	void Awake(){
		_parent = transform;
	}

	void Update(){
		if (_position != transform.position) {
			_position = transform.position;
		}
		if (_eulerRotation != transform.rotation.eulerAngles) {
			_eulerRotation = transform.rotation.eulerAngles;
		}
		if (_occupied) {
			Transform pickupableObject = transform.GetChild (0);
			if (_position != pickupableObject.position) {
				pickupableObject.position = _position;
			}
			if (_eulerRotation != pickupableObject.rotation.eulerAngles) {
				pickupableObject.rotation = Quaternion.Euler (_eulerRotation);
			}
		}
	}
}
