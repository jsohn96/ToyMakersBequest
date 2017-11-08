using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlPoint : MonoBehaviour {
	[SerializeField] float _fov = 0.0f;
	[SerializeField] float _duration = 3.0f;
	public float fov{ get{return _fov; }}
	public float duration{ get{return _duration; }}
}
