using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoetropeAnimation : MonoBehaviour {
	[SerializeField] Animator _anim;
	float duration = 360f;
	[SerializeField] Transform _transformToObserve;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float animPlaybackVal = 1 - Mathf.Abs(DampAngle (_transformToObserve.localEulerAngles.z)) / duration;
		//animPlaybackVal = AnimationOptimizer (animPlaybackVal);
		_anim.Play ("crank", -1, animPlaybackVal);	
	}

	float DampAngle(float angle){
		if (angle >= 360) {
			angle -= 360;
			return DampAngle (angle);
		} else if (angle < 0) {
			angle += 360;
			return DampAngle (angle);
		} else {
			return angle;
			//break;
		}

		//return angle;
	}
}
