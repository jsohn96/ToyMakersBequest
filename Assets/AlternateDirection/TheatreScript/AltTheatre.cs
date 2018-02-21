using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use part of old music box script 


public enum TheatreState{
	waitingToStart = -1,
	startShow = 0,
	readyForDancerTank = 1,
	dancerInTank = 2,
	magicianBoardTank = 3,
	waterTankDescend = 4,
	magicianPrepareFrog = 5,
	magicianLeft = 6,
	frogJump = 7,
	lookDownIntoTank = 8,
	CloseTank1 = 9,
	CloseTank2 = 10,
	magicianRight = 11,
	dancerShowUp = 12,
	dancerKissing = 13,
	audienceLeave = 14,
	magicianReturnToPosition = 15,
	dancerDescend = 16,
	dancerLocked = 17,
	theatreEnd = 18
}

public class AltTheatre : LevelManager {
	[SerializeField] TheatreCameraControl _theatreCameraControl;

	public static TheatreState currentSate = TheatreState.waitingToStart;
	[SerializeField] TheatreMagician magician;
	[SerializeField] TheatreDancer _dancer;	
	TheatreChest chest;
	TheatreCabinet cabinet;
	PathNetwork network;

	[Header("Water Tank")]
	[SerializeField] Transform _watertank;
	[SerializeField] TheatreWaterTank _theatreWaterTank;
	float _waterTankDuration = 6f;
	[SerializeField] Vector3 _tankTopPos, _tankBottomPos;
	[Header("Starting Platform")]
	[SerializeField] Transform _startPlatform;
	float _platformDuration = 3f;
	[SerializeField] Vector3 _platformBeginPos, _platformEndPos;

	[Header("Interactive Scene Objects")]
	[SerializeField] TheatreCabinet _theatreCabinet;
	[SerializeField] TheatreChest _theatreChest;

	[SerializeField] TheatreWaterTankDoors _tankDoor1, _tankDoor2;

	[SerializeField] AudioSource _theatreMusic;

	[SerializeField] TraversalUI _traversalUI;
	[SerializeField] TheatreLighting _theatreLighting;
	[SerializeField] TheatreSound _theatreSound;


	// Use this for initialization
	void Awake () {
		currentSate = TheatreState.waitingToStart;
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

	void Start(){
		_watertank.localPosition = _tankTopPos;
		_startPlatform.localPosition = _platformBeginPos;
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

		if (Input.GetKeyDown (KeyCode.Z)) {
			Time.timeScale = 5.0f;
		} else if (Input.GetKeyUp (KeyCode.Z)) {
			Time.timeScale = 1.0f;
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

	public void CheckStateMachine(){
		switch (currentSate) {
		case TheatreState.startShow:
			_theatreLighting.MoveToNextLights ();
			_theatreMusic.Play ();
			_theatreSound.PlayLightSwitch ();
			//magician.GoToStart ();
			StartCoroutine (LerpPosition (_startPlatform, _platformBeginPos, _platformEndPos, _platformDuration, 4f, ()=>{
				magician.PointToCenter(true);
				MoveToNext();
			}));
			magician.GoToStart();
			// call back function? 
			break;
		case TheatreState.readyForDancerTank:
			_theatreWaterTank.Activate (true);
			break;
		case TheatreState.dancerInTank:
			_dancer.FirstDancerEnterTank ();
			break;
		case TheatreState.magicianBoardTank:
			_traversalUI.FadeIn ();
			_theatreWaterTank.Activate (false);
			_theatreWaterTank.OpenLid (false);
			magician.StepOnTank ();
			magician.PointToCenter (false);
			_theatreCameraControl.EnableScrollFOV();
			break;
		case TheatreState.waterTankDescend:
			StartCoroutine (LerpPosition (_watertank, _tankTopPos, _tankBottomPos, _waterTankDuration, 0f, ()=>{
				MoveToNext();
			}));
			break;
		case TheatreState.magicianPrepareFrog:
			magician.StepOffTank ();
			break;
		case TheatreState.magicianLeft:
			magician.PointToLeft (true);
			chest.Activate(true);
			break;
		case TheatreState.frogJump:
			magician.PointToLeft (false);
			// frog.jumpout
			//magician go back to center position 
			//MoveToNext();
			break;
		case TheatreState.lookDownIntoTank:
			_traversalUI.FadeOut ();
			_theatreCameraControl.MoveCameraToLookAtTank ();
			_tankDoor1.Activate (true);
			_tankDoor2.Activate (true);
			//MoveToNext();
			break;
		case TheatreState.magicianRight:
			_traversalUI.FadeIn ();
			_theatreCameraControl.EnableScrollFOV();
			_tankDoor1.Activate (false);
			_tankDoor2.Activate (false);
			magician.PointToRight (true);
			_dancer.HideDancer (true);
			// dancer.enterScene();
			cabinet.Activate (true);

			break;
		case TheatreState.dancerShowUp:
			magician.PointToRight (false);
			// dancer.showUp();
			_dancer.HideDancer (false);
			// enable network connection 
			network.SetPathActive(true);
			break;
		case TheatreState.dancerKissing:
			magician.EnterKissPosition ();
			break;
		case TheatreState.audienceLeave:
			_dancer.ElevateTankPlatform ();
			MoveToNext ();
			break;
		case TheatreState.magicianReturnToPosition:
			
			magician.ExitKissPosition ();
			break;
		case TheatreState.dancerDescend:
			_traversalUI.FadeOut ();
			_theatreWaterTank.OpenLid (true);
			_dancer.SecondDancerEnterTank ();
			_theatreCameraControl.MoveCameraToLookAtTank ();
			break;
		case TheatreState.dancerLocked:
			_theatreWaterTank.OpenLid (false);
			_tankDoor1.FinalActivation (true);
			_tankDoor2.FinalActivation (true);
			break;
		case TheatreState.theatreEnd:
			_traversalUI.FadeIn ();
			_dancer.HideDancer (true);
			_tankDoor1.OpenTankCall ();
			_tankDoor2.OpenTankCall ();
			_theatreMusic.Stop ();
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

	void TriggerKiss(PathStateManagerEvent e){
		if (e.activeEvent == PathState.ReachPathEnd) {
			if (currentSate == TheatreState.dancerKissing - 1) {
				currentSate += 1;
				CheckStateMachine ();
				Debug.Log ("Current Theatre State: " + currentSate);
			}
		}
	}

	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (TriggerKiss);
	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (TriggerKiss);
	}
}
