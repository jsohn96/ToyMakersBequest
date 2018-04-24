using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceHeadFollow : MonoBehaviour {
	[SerializeField] bool _isHeadIKActive = true;
	[SerializeField] Transform _target; 
	Animator _audienceAnim;

	// Use this for initialization
	void Awake () {
		
		_audienceAnim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnAnimatorIK()
	{
		Debug.Log ("On animator IK");
		if(_audienceAnim) {
			if (_isHeadIKActive) {
				//if the IK is active, set the position and rotation directly to the goal. 
				_audienceAnim.SetLookAtWeight(1);
				_audienceAnim.SetLookAtPosition(_target.position);
				// Set the look target position, if one has been assigned
				if(_target != null) {

				}    
			
			}

		}
			
	}    
}
