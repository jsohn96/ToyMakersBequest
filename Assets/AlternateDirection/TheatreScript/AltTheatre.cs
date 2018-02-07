using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use part of old music box script 


public enum TheatreState{
	waitingToStart = -1,
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
	public static TheatreState currentSate = TheatreState.waitingToStart;
	[SerializeField] TheatreMagician magician;
	TheatreChest chest;
	TheatreCabinet cabinet;
	PathNetwork network;
	[Header("Water Tank")]
	[SerializeField] Transform _watertank;
	float _waterTankDuration = 3f;
	[SerializeField] Vector3 _tankTopPos, _tankBottomPos;
	[Header("Starting Platform")]
	[SerializeField] Transform _startPlatform;
	float _platformDuration = 3f;
	[SerializeField] Vector3 _platformBeginPos, _platformEndPos;

	// Use this for initialization
	void Awake () {
		chest = FindObjectOfType<TheatreChest> ().GetComponent<TheatreChest> ();
		cabinet = FindObjectOfType<TheatreCabinet> ().GetComponent<TheatreCabinet> ();
		network = FindObjectOfType<PathNetwork> ().GetComponent<PathNetwork> ();
	}

	void Start(){
		_watertank.localPosition = _tankTopPos;
		_startPlatform.localPosition = _platformBeginPos;
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
			magician.GoToStart ();
			StartCoroutine (LerpPosition (_startPlatform, _platformBeginPos, _platformEndPos, _platformDuration, 1f));
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

	IEnumerator LerpPosition (Transform transform, Vector3 origin, Vector3 goal, float duration, float initialDelay = 0f, System.Action action = null){
		yield return new WaitForSeconds (initialDelay);
		float timer = 0f;
		while (duration > timer) {
			timer += Time.deltaTime; 
			transform.localPosition = Vector3.Slerp (origin, goal, timer / duration);
			yield return null;
		}
		transform.localPosition = goal;
		yield return null;
		if (action != null) {
			action ();
		}
	}

	void SlidingDoorHandle(SlidingDoorFinished e){
		if (e.IsOpen) {
			if (currentSate == TheatreState.waitingToStart) {
				currentSate++;
				CheckStateMachine ();
			}
		}
	}

	void OnEnable(){
		Events.G.AddListener<SlidingDoorFinished> (SlidingDoorHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<SlidingDoorFinished> (SlidingDoorHandle);
	}
}
