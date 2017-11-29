using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBNetCatcher : MonoBehaviour {
	[SerializeField] Animator _netAnim;
	float duration = 180f;
	float _originalVal;
	// Use this for initialization
	void Start () {
		//_netAnim.Play ("catch");
	}
	
	// Update is called once per frame
	void Update () {
		CheckAnimationProgress ();
		_originalVal = transform.localEulerAngles.z;
	}

	void CheckAnimationProgress(){
		float animPlaybackVal = 1 - Mathf.Abs(DampAngle (transform.localEulerAngles.z)-180) / duration;
		//animPlaybackVal = AnimationOptimizer (animPlaybackVal);
		print ("Catcher progress " + animPlaybackVal);
		_netAnim.Play ("catch", -1, animPlaybackVal);
	}

	// map angle to [0,2*PI)
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

	float AnimationOptimizer(float prg){
		float tempVal = prg * 10f;
		print ("tempval " + tempVal);
		tempVal = Mathf.Floor (prg) + 1f;
		print ("tempval " + tempVal*0.1f);
		if (tempVal <= 10) {
			return tempVal * 0.1f;
		} else {
			return 1f;
		}



	}
}
