using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreCoin : MonoBehaviour {
	bool _pickupable = false;
	[SerializeField] BoxCollider _coinBoxCollider;
	[SerializeField] BoxCollider _parentBoxCollider;
	[SerializeField] shaderGlowCustom _parentShaderGlowCustom;

	public void BeginGlow(){
		_parentBoxCollider.enabled = false;
		_parentShaderGlowCustom.enabled = false;
		GetComponent<shaderGlowCustom> ().enabled = true;
		_pickupable = true;
		_coinBoxCollider.enabled = true;
	}

	void OnTouchDown(Vector3 point){
		if (_pickupable) {
			gameObject.SetActive (false);
		}
	}
}
