using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use part of old music box script 


public enum TheatreState{
	none = -1,
	startShow = 0,
	magicianLeft,
	frogJump,
	magicianRight,
	dancerShowUp,
	dancerKissing,
	audienceLeave,
	dancerDescend,
	dancerLocked,
	theatreEnd
}

public class AltTheatre : MonoBehaviour {
	TheatreState currentSate = TheatreState.none;
	[SerializeField] GameObject magician;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.Space)){
			if(currentSate < TheatreState.theatreEnd){
				currentSate += 1;
				CheckStateMachine();
				Debug.Log("Current Theatre State: " + currentSate);
			}

		}
		#endif

		CheckStateUpdate ();
		
	}

	public void CheckStateUpdate(){
		if (currentSate == TheatreState.magicianLeft) {
			Debug.Log ("Waiting for player input to continue");
		} else if (currentSate == TheatreState.magicianRight) {
			Debug.Log ("Waiting for player input to continue");
		} else if (currentSate == TheatreState.dancerShowUp) {
			Debug.Log ("Waiting for player to connect nodes");
		}
	
	}

	void CheckStateMachine(){
		switch (currentSate) {
		case TheatreState.startShow:
			// magician.enter();
			// call back function? 
			break;
		case TheatreState.magicianLeft:
			// magician.pointLeft()
			// frog.enterScene();
			break;
		case TheatreState.frogJump:
			// frog.jumpout
			break;
		case TheatreState.magicianRight:
			// magician.pointRight();
			// dancer.enterScene();
			break;
		case TheatreState.dancerShowUp:
			// dancer.showUp();
			// enable network connection 
			break;
		case TheatreState.dancerKissing:
			// frog.jumpout
			break;
		case TheatreState.audienceLeave:
			
			break;
		case TheatreState.dancerDescend:
			
			break;
		case TheatreState.dancerLocked:
			
			break;


		}
	}
}
