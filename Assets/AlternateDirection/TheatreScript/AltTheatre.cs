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

public class AltTheatre : LevelManager {
	TheatreState currentSate = TheatreState.none;
	[SerializeField] TheatreMagician magician;
	TheatreChest chest;
	TheatreCabinet cabinet;
	PathNetwork network;


	// Use this for initialization
	void Awake () {
		chest = FindObjectOfType<TheatreChest> ().GetComponent<TheatreChest> ();
		cabinet = FindObjectOfType<TheatreCabinet> ().GetComponent<TheatreCabinet> ();
		network = FindObjectOfType<PathNetwork> ().GetComponent<PathNetwork> ();
	}

	public override void PickUpCharm(){
		base.PickUpCharm ();
		AltCentralControl._freedom = true;
		AltCentralControl._currentState = (AltStates)((int)AltCentralControl._currentState+1);
		InventorySystem._instance.AddItem (items.dancerCharm);
		Events.G.Raise (new PickedUpItem (items.dancerCharm));
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

	public void MoveToNext(){
		if(currentSate < TheatreState.theatreEnd){
			currentSate += 1;
			CheckStateMachine();
			Debug.Log("Current Theatre State: " + currentSate);
		}

	}

	void CheckStateUpdate(){
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
			magician.GoToStart();
			// call back function? 
			break;
		case TheatreState.magicianLeft:
			// magician.pointLeft()
			chest.Activate();
			break;
		case TheatreState.frogJump:
			// frog.jumpout
			//magician go back to center position 
			MoveToNext();
			break;
		case TheatreState.magicianRight:
			// magician.pointRight();
			// dancer.enterScene();
			cabinet.Activate ();

			break;
		case TheatreState.dancerShowUp:
			// dancer.showUp();
			// enable network connection 
			network.SetPathActive(true);
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
