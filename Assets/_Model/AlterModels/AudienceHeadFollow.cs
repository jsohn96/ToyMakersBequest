using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// include the audience head turn 
// audience behaviour 

public class AudienceHeadFollow : AudioSourceController {
	[SerializeField] bool _isHeadIKActive = true;
	[SerializeField] string _reactName;
	[SerializeField] Transform _target; 
	Animator _audienceAnim;

	[SerializeField] AudioSystem _clapSound;

	// Use this for initialization
	void Awake () {
		
		_audienceAnim = GetComponent<Animator> ();
	}
	
	public void MakeClapSound(){
		SwapClip (_clapSound, false);
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

	public void PlayClap(){
		_audienceAnim.Play ("Clap");
	}

	public void PlayKissReaction(){
		_audienceAnim.Play (_reactName);
	}

	public void OnClickAudience(){
		if (_isHeadIKActive) {
			//if the IK is active, set the position and rotation directly to the goal. 
			_audienceAnim.SetLookAtWeight(1);
			_audienceAnim.SetLookAtPosition(Camera.main.transform.position);
			// Set the look target position, if one has been assigned
			if(_target != null) {

			}    

		}
		
	}
}
