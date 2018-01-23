using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderKnobTouchDown : MonoBehaviour {
	[SerializeField] SliderScript _sliderScript;

	void OnTouchDown(Vector3 point){
		_sliderScript.KnobInitialization ();
	}
}
